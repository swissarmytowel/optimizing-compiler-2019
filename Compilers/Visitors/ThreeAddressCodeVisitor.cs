using System;
using System.Collections.Generic;
using System.Linq;
using ProgramTree;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Visitors
{
    /// <summary>
    /// Visitor to generate Three-address code from current language code
    /// </summary>
    public class ThreeAddressCodeVisitor : AutoVisitor
    {
        /// <summary>
        /// TAC container
        /// For details look at the class definition at SimpleLang.TACode.ThreeAddressCode.cs
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public ThreeAddressCode TACodeContainer { get; }

        public ThreeAddressCodeVisitor()
        {
            TACodeContainer = new ThreeAddressCode();
        }
        
        /// <summary>
        /// Managing tac generation for trivial expression cases, that are already TAC-like
        /// E.g. a = 1 + b, while(a == 0) so on.
        /// </summary>
        /// <param name="node">Expression node to be parsed</param>
        /// <returns>Identifier of the last tmp variable generated</returns>
        private string ManageTrivialCases(ExprNode node)
        {
            switch (node)
            {
                case IdNode idNode:
                    return idNode.ToString();

                case IntNumNode intNumNode:
                    return intNumNode.ToString();

                case BoolNode boolNode:
                    return boolNode.ToString();
                
                case FunctionNode funcNode:
                    return funcNode.ToString();
                
                case DoubleNumNode doubleNode:
                    return doubleNode.ToString();

                default:
                    // if the default case is hit, Expr is complex, and TAC simplification can't be done
                    return GenerateThreeAddressLine(node);
            }
        }
        public override void VisitAssignNode(AssignNode a)
        {

            string rightPartExpression = null;
            // If should try to simplify the TAC for a trivial assignment 
            rightPartExpression = ManageTrivialCases(a.Expr);
            TACodeContainer.PushNode(new TacAssignmentNode()
            {
                LeftPartIdentifier = a.Id.Name,
                FirstOperand = rightPartExpression
            });
        }
        
        /// <summary>
        /// Method that recursively generates TAC for a given complex expression
        /// </summary>
        /// <param name="expression">Expression to be decomposed</param>
        /// <returns>Last tmp identifier left from TAC decomposition</returns>
        private string GenerateThreeAddressLine(ExprNode expression)
        {
            // This is used to merge the label from previous if/while/for node parsing
            // From an empty node to current parsed
            string label = null;
            if (TACodeContainer.Last != null && TACodeContainer.Last.Value.IsUtility)
            {
                label = TACodeContainer.Last.Value.Label;
                TACodeContainer.RemoveNode(TACodeContainer.Last.Value);
            }
            // Main switcher
            switch (expression)
            {
                // Trivial cases. Each switch branch generate simple corresponding node
                case IdNode idNode:
                {
                    return TACodeContainer.CreateAndPushIdNode(idNode, label);
                }
                case IntNumNode intNumNode:
                {
                    return TACodeContainer.CreateAndPushIntNumNode(intNumNode, label);
                }
                case BoolNode boolNode:
                {
                    return TACodeContainer.CreateAndPushBoolNode(boolNode, label);
                }
                case UnOpNode unOpNode:
                {
                    var unaryExp = ManageTrivialCases(unOpNode.Unary);
                    
                    var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
                    TACodeContainer.PushNode(new TacAssignmentNode()
                    {
                        Label = label,
                        LeftPartIdentifier = tmpName,
                        FirstOperand = null,
                        Operation = unOpNode.Op,
                        SecondOperand = unaryExp
                    });
                    return tmpName;
                }
                // Complex case, when a part of an expr is a binary operation
                case BinOpNode binOpNode:
                {
                    
                    var leftPart = ManageTrivialCases(binOpNode.Left);
                    var rightPart = ManageTrivialCases(binOpNode.Right);
                    
                    // Creating and pushing the resulting binOp between 
                    // already generated above TAC variables
                    var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
                    TACodeContainer.PushNode(new TacAssignmentNode()
                    {
                        Label = label,
                        LeftPartIdentifier = tmpName,
                        FirstOperand = leftPart,
                        Operation = binOpNode.Op,
                        SecondOperand = rightPart
                    });
                    return tmpName;
                }
                case LogicNotNode logicNotNode:
                {
                    var unaryExp = ManageTrivialCases(logicNotNode.LogExpr);
                    
                    var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
                    TACodeContainer.PushNode(new TacAssignmentNode()
                    {
                        Label = label,
                        LeftPartIdentifier = tmpName,
                        FirstOperand = null,
                        Operation = "!",
                        SecondOperand = unaryExp
                    });
                    return tmpName;
                }
            }
            // Defaulting return. If no known code constructions are encountered
            return default(string);
        }

        public override void VisitIfNode(IfNode c)
        {
            var lastNodeBeforeGeneration = TACodeContainer.Last;
            // Separate conditional expression from the rest of the generation
            // As we will need the resulting last tmp ID for conditional goto jump later
            var conditionalExpression = GenerateThreeAddressLine(c.Expr);
            
            // Generating 'else' clause and exiting labels
            var mainIfBlockStartLabel = TmpNameManager.Instance.GenerateLabel();
            var exitingLabel = TmpNameManager.Instance.GenerateLabel();

            TACodeContainer.PushNode(new TacIfGotoNode()
            {
                Condition = conditionalExpression,
                TargetLabel = mainIfBlockStartLabel
            });
            
            // Checking if 'else' block exists and traversing it to generate TAC
            c.Stat2?.Visit(this);
            
            // Creating goto jump towards an exit of the loop. That's the case, when we hit 'else'
            TACodeContainer.PushNode(new TacGotoNode()
            {
                TargetLabel = exitingLabel
            });
            
            // Pushing main if block starting label to denote entry point for conditional jump
            TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = mainIfBlockStartLabel,
                IsUtility = true
            });
            
            // Traversing main if block and generating TAC
            c.Stat1.Visit(this);
            
            // Placing the exit label at the end of if-else section
            TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = exitingLabel,
                IsUtility = true
            });
           
            ClashUtilityLabels(lastNodeBeforeGeneration);
        }
        
        /// <summary>
        /// Merge utility labels to lead to code lines instead of empty nodes
        /// </summary>
        /// <param name="lastNodeBeforeGeneration">Last node present before a new construction was generated</param>
        private void ClashUtilityLabels(LinkedListNode<TacNode> lastNodeBeforeGeneration)
        {
            var nodesToRemove = new List<TacNode>();
            lastNodeBeforeGeneration = lastNodeBeforeGeneration ?? TACodeContainer.First;
            while (lastNodeBeforeGeneration != null)
            {
                if (lastNodeBeforeGeneration.Value is TacEmptyNode label)
                {
                    if (lastNodeBeforeGeneration.Next != null)
                    {
                        lastNodeBeforeGeneration.Next.Value.Label = label.Label;
                        nodesToRemove.Add(label);
                    }
                }
                lastNodeBeforeGeneration = lastNodeBeforeGeneration.Next;
            }
            TACodeContainer.RemoveNodes(nodesToRemove);
        }

        public override void VisitWhileNode(WhileNode c)
        {
            var lastNodeBeforeGeneration = TACodeContainer.Last;

        // Label to the initial conditional jump check (right above bool expression under while())
            var conditionalCheckLabel = TmpNameManager.Instance.GenerateLabel();
            TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = conditionalCheckLabel,
                IsUtility = true
            });

            // Separate conditional expression from the rest of the generation
            // As we will need the resulting last tmp ID for conditional goto jump later
            var conditionalExpression = GenerateThreeAddressLine(c.Expr);
            
            // Creating starting and ending labels
            var endOfWhileStatementLabel = TmpNameManager.Instance.GenerateLabel();
            var startOfWhileBodyLabel = TmpNameManager.Instance.GenerateLabel();

            // Create conditional jump statement at the starting position of while
            TACodeContainer.PushNode(new TacIfGotoNode()
            {
                Condition = conditionalExpression,
                TargetLabel = startOfWhileBodyLabel
            });
            
            // Exiting goto jump. If condition is false -- then this line will execute
            // And jump out of while body
            TACodeContainer.PushNode(new TacGotoNode()
            {
                TargetLabel = endOfWhileStatementLabel
            });
            
            // Main body entry point
            TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = startOfWhileBodyLabel,
                IsUtility = true
            });
            
            // Traversing while block contents and generating TAC
            c.Stat.Visit(this);
           
            // Placing upward jump to the entry point of the while statement
            TACodeContainer.PushNode(new TacGotoNode()
            {
                TargetLabel = conditionalCheckLabel
            });
           
            // Placing exiting label at the end of while
            TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = endOfWhileStatementLabel,
                IsUtility = true
            });
            
            ClashUtilityLabels(lastNodeBeforeGeneration);
        }

        public override void VisitForNode(ForNode c)
        {
            var lastNodeBeforeGeneration = TACodeContainer.Last;

            // Traversing initial counter assignment and generating TAC
            c.Assign.Visit(this);
            string conditionalExpression;
            
            // Setting entry point label
            var startOfForStatementLabel = TmpNameManager.Instance.GenerateLabel();
            TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = startOfForStatementLabel,
                IsUtility = true
            });
            
            // Traversing main for block and generating TAC
            c.Stat.Visit(this);
            TACodeContainer.PushNode(new TacAssignmentNode()
            {
                LeftPartIdentifier = c.Assign.Id.Name,
                FirstOperand = c.Assign.Id.Name,
                Operation = "+",
                SecondOperand = "1"
            });
            
            // Switcher to fix the trivial border problem, where (i=0 to 10) is 
            // Interpreted as  | L1: t1 = 10
            //                 | L2: if t1 goto ...
            switch (c.Expr)
            {
                case IdNode idNode:
                    conditionalExpression = idNode.Name;
                    break;
                case IntNumNode intNumNode:
                    conditionalExpression = intNumNode.Num.ToString();
                    break;
                default:
                    conditionalExpression = GenerateThreeAddressLine(c.Expr);
                    break;
            }
            // Generating ID for loop conditional expression and creating corresponding TAC node
            var conditionalExpressionId = TmpNameManager.Instance.GenerateTmpVariableName();
            
            TACodeContainer.PushNode(new TacAssignmentNode()
            {
                LeftPartIdentifier = conditionalExpressionId,
                FirstOperand = c.Assign.Id.Name,
                Operation = "<",
                SecondOperand = conditionalExpression
            });
            // Creating conditional jump towards a label of loop entry point
            TACodeContainer.PushNode(new TacIfGotoNode()
            {
                Condition = conditionalExpressionId,
                TargetLabel = startOfForStatementLabel
            });
            
            ClashUtilityLabels(lastNodeBeforeGeneration);
        }

        public override void VisitEmptyNode(EmptyNode w)
        {
            TACodeContainer.CreateAndPushEmptyNode(w);
        }

        public override void VisitGotoNode(GotoNode gt)
        {
            TACodeContainer.PushNode(new TacGotoNode()
            {
                IsUtility = false,
                TargetLabel = "L" + gt.L.Inum
            });
        }

        public override void VisitLabelNode(LabelNode l)
        {
            TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = "L" + l.Inum
            });
        }

        public override string ToString() => TACodeContainer.ToString();
    }
}
