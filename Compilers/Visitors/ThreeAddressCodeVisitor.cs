// Undefine, if the following behavior is desired:
// a = 1 =>
// L1: t1 = 1
// L2: a = t1

#define SINGLE_TAC_ASSIGN_COMMANDS_REQUIRED

using System;
using System.Linq;
using System.Text;
using ProgramTree;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Visitors
{
    internal class ThreeAddressCodeVisitor : AutoVisitor
    {
        public ThreeAddressCode ThreeAddressCodeContainer { get; }

        public ThreeAddressCodeVisitor()
        {
            ThreeAddressCodeContainer = new ThreeAddressCode();
        }

        public override void VisitAssignNode(AssignNode a)
        {
            string rightPartExpression = null;
#if SINGLE_TAC_ASSIGN_COMMANDS_REQUIRED
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

        private int depth = 0;

        private string GenerateThreeAddressLine(ExprNode expression)
        {
            switch (expression)
            {
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
                case BinOpNode binOpNode:
                {
                    var leftPart = GenerateThreeAddressLine(binOpNode.Left);
                    var rightPart = GenerateThreeAddressLine(binOpNode.Right);
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

            return default(string);
        }

        public override void VisitIfNode(IfNode c)
        {
            var conditionalExpression = GenerateThreeAddressLine(c.Expr);

            var firstLabel = TmpNameManager.Instance.GenerateLabel();
            var secondLabel = TmpNameManager.Instance.GenerateLabel();

            ThreeAddressCodeContainer.PushNode(new TacIfGotoNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                Condition = conditionalExpression,
                TargetLabel = firstLabel
            });

            c.Stat2?.Visit(this);
            ThreeAddressCodeContainer.PushNode(new TacGotoNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                TargetLabel = secondLabel
            });

            ThreeAddressCodeContainer.PushNode(new TacEmptyNode()
            {
                Label = firstLabel
            });
            c.Stat1.Visit(this);

            ThreeAddressCodeContainer.PushNode(new TacEmptyNode()
            {
                Label = secondLabel
            });
        }

        public override void VisitWhileNode(WhileNode c)
        {
            var conditionalExpression = GenerateThreeAddressLine(c.Expr);
            var startOfWhileStatementLabel = TmpNameManager.Instance.GenerateLabel();
            var endOfWhileStatementLabel = TmpNameManager.Instance.GenerateLabel();

            ThreeAddressCodeContainer.PushNode(new TacIfGotoNode()
            {
                Label = startOfWhileStatementLabel,
                Condition = conditionalExpression,
                TargetLabel = endOfWhileStatementLabel
            });
            c.Stat.Visit(this);
            ThreeAddressCodeContainer.PushNode(new TacGotoNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                TargetLabel = startOfWhileStatementLabel
            });
            ThreeAddressCodeContainer.PushNode(new TacEmptyNode()
            {
                Label = endOfWhileStatementLabel
            });
        }

        public override void VisitForNode(ForNode c)
        {
            c.Assign.Visit(this);
            string conditionalExpression;

            var startOfForStatementLabel = TmpNameManager.Instance.GenerateLabel();
            ThreeAddressCodeContainer.PushNode(new TacEmptyNode()
            {
                Label = startOfForStatementLabel
            });

            c.Stat.Visit(this);
            ThreeAddressCodeContainer.PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPartIdentifier = c.Assign.Id.Name,
                FirstOperand = c.Assign.Id.Name,
                Operation = "+",
                SecondOperand = "1"
            });

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

            var conditionalExpressionID = TmpNameManager.Instance.GenerateTmpVariableName();

            ThreeAddressCodeContainer.PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPartIdentifier = conditionalExpressionID,
                FirstOperand = c.Assign.Id.Name,
                Operation = "<",
                SecondOperand = conditionalExpression
            });

            ThreeAddressCodeContainer.PushNode(new TacIfGotoNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                Condition = conditionalExpressionID,
                TargetLabel = startOfForStatementLabel
            });
        }
    }
}