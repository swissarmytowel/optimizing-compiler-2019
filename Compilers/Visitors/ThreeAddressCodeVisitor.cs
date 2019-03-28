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
            var rightPartExpression = GenerateThreeAddressLine(a.Expr);
            ThreeAddressCodeContainer.PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPart = a.Id.Name,
                FirstOperand = rightPartExpression
            });
        }

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
                        LeftPart = tmpName,
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
    }
}
