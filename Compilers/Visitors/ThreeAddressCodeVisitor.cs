// Undefine, if the following behavior is desired:
// a = 1 =>
// L1: t1 = 1
// L2: a = t1

#define SINGLE_TAC_ASSIGN_COMMANDS_REQUIRED

using ProgramTree;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Visitors
{
    /// <summary>
    /// Visitor to generate Three-address code from current language code
    /// </summary>
    internal class ThreeAddressCodeVisitor : AutoVisitor
    {
        /// <summary>
        /// TAC container
        /// For details look at the class definition at SimpleLang.TACode.ThreeAddressCode.cs
        /// </summary>
        public ThreeAddressCode ThreeAddressCodeContainer { get; }

        public ThreeAddressCodeVisitor()
        {
            ThreeAddressCodeContainer = new ThreeAddressCode();
        }

        public override void VisitAssignNode(AssignNode a)
        {
            string rightPartExpression = null;
#if SINGLE_TAC_ASSIGN_COMMANDS_REQUIRED
            // Switching expression to see if it is of any primitive kind
            // And if so, skip TAC generation to avoid additional tmp variables creation
            switch (a.Expr)
            {
                case IdNode idNode:
                {
                    rightPartExpression = idNode.ToString();
                    break;
                }
                case IntNumNode intNumNode:
                {
                    rightPartExpression = intNumNode.ToString();
                    break;
                }
                case BoolNode boolNode:
                {
                    rightPartExpression = boolNode.ToString();
                    break;
                }
                default:
                {
                    // if the default case is hit, Expr is complex, and TAC simplification can't be done
                    rightPartExpression = GenerateThreeAddressLine(a.Expr);
                    break;
                }
            }
#else
            rightPartExpression = GenerateThreeAddressLine(a.Expr);
#endif
            ThreeAddressCodeContainer.PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
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
            switch (expression)
            {
                // Trivial cases
                case IdNode idNode:
                {
                    return ThreeAddressCodeContainer.CreateAndPushIdNode(idNode);
                }
                case IntNumNode intNumNode:
                {
                    return ThreeAddressCodeContainer.CreateAndPushIntNumNode(intNumNode);
                }
                case BoolNode boolNode:
                {
                    return ThreeAddressCodeContainer.CreateAndPushBoolNode(boolNode);
                }
                // Complex case, when a part of an expr is a binary operation
                case BinOpNode binOpNode:
                {
                    // Recursive traversing left & right parts of BinOp
                    var leftPart = GenerateThreeAddressLine(binOpNode.Left);
                    var rightPart = GenerateThreeAddressLine(binOpNode.Right);
                    
                    // Creating and pushing the resulting binOp between 
                    // already generated above TAC variables
                    var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
                    ThreeAddressCodeContainer.PushNode(new TacAssignmentNode()
                    {
                        Label = TmpNameManager.Instance.GenerateLabel(),
                        LeftPartIdentifier = tmpName,
                        FirstOperand = leftPart,
                        Operation = binOpNode.Op,
                        SecondOperand = rightPart
                    });
                    return tmpName;
                }
            }
            // Defaulting return. If no known code constructions are encountered
            return default(string);
        }

        public override void VisitIfNode(IfNode c)
        {
            // Separate conditional expression from the rest of the generation
            // As we will need the resulting last tmp ID for conditional goto jump later
            var conditionalExpression = GenerateThreeAddressLine(c.Expr);
            
            // Generating 'else' clause and exiting labels
            var mainIfBlockStartLabel = TmpNameManager.Instance.GenerateLabel();
            var exitingLabel = TmpNameManager.Instance.GenerateLabel();

            ThreeAddressCodeContainer.PushNode(new TacIfGotoNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                Condition = conditionalExpression,
                TargetLabel = mainIfBlockStartLabel
            });
            
            // Checking if 'else' block exists and traversing it to generate TAC
            c.Stat2?.Visit(this);
            
            // Creating goto jump towards an exit of the loop. That's the case, when we hit 'else'
            ThreeAddressCodeContainer.PushNode(new TacGotoNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                TargetLabel = exitingLabel
            });
            
            // Pushing main if block starting label to denote entry point for conditional jump
            ThreeAddressCodeContainer.PushNode(new TacEmptyNode()
            {
                Label = mainIfBlockStartLabel
            });
            
            // Traversing main if block and generating TAC
            c.Stat1.Visit(this);
            
            // Placing the exit label at the end of if-else section
            ThreeAddressCodeContainer.PushNode(new TacEmptyNode()
            {
                Label = exitingLabel
            });
        }

        public override void VisitWhileNode(WhileNode c)
        {
            // Separate conditional expression from the rest of the generation
            // As we will need the resulting last tmp ID for conditional goto jump later
            var conditionalExpression = GenerateThreeAddressLine(c.Expr);
            
            // Label to the initial conditional jump check (bool expression under while())
            var conditionalCheckLabel = TmpNameManager.Instance.GenerateLabel();
            
            // Creating starting and ending labels
            var endOfWhileStatementLabel = TmpNameManager.Instance.GenerateLabel();
            var startOfWhileBodyLabel = TmpNameManager.Instance.GenerateLabel();

            // Create conditional jump statement at the starting position of while
            ThreeAddressCodeContainer.PushNode(new TacIfGotoNode()
            {
                Label = conditionalCheckLabel,
                Condition = conditionalExpression,
                TargetLabel = startOfWhileBodyLabel
            });
            
            // Exiting goto jump. If condition is false -- then this line will execute
            // And jump out of while body
            ThreeAddressCodeContainer.PushNode(new TacGotoNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                TargetLabel = endOfWhileStatementLabel
            });
            
            // Main body entry point
            ThreeAddressCodeContainer.PushNode(new TacEmptyNode()
            {
                Label = startOfWhileBodyLabel
            });
            
            // Traversing while block contents and generating TAC
            c.Stat.Visit(this);
           
            // Placing upward jump to the entry point of the while statement
            ThreeAddressCodeContainer.PushNode(new TacGotoNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                TargetLabel = conditionalCheckLabel
            });
           
            // Placing exiting label at the end of while
            ThreeAddressCodeContainer.PushNode(new TacEmptyNode()
            {
                Label = endOfWhileStatementLabel
            });
        }

        public override void VisitForNode(ForNode c)
        {
            // Traversing initial counter assignment and generating TAC
            c.Assign.Visit(this);
            string conditionalExpression;
            
            // Setting entry point label
            var startOfForStatementLabel = TmpNameManager.Instance.GenerateLabel();
            ThreeAddressCodeContainer.PushNode(new TacEmptyNode()
            {
                Label = startOfForStatementLabel
            });
            
            // Traversing main for block and generating TAC
            c.Stat.Visit(this);
            ThreeAddressCodeContainer.PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
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
            var conditionalExpressionID = TmpNameManager.Instance.GenerateTmpVariableName();
            
            ThreeAddressCodeContainer.PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPartIdentifier = conditionalExpressionID,
                FirstOperand = c.Assign.Id.Name,
                Operation = "<",
                SecondOperand = conditionalExpression
            });
            // Creating conditional jump towards a label of loop entry point
            ThreeAddressCodeContainer.PushNode(new TacIfGotoNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                Condition = conditionalExpressionID,
                TargetLabel = startOfForStatementLabel
            });
        }
    }
}