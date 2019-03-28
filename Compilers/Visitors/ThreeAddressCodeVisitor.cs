using System;
using System.Linq;
using System.Text;
using ProgramTree;
using System.Collections.Generic;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Visitors
{
    class ThreeAddressCodeVisitor : AutoVisitor
    {
        public ThreeAddressCode Tac { get; }

        public ThreeAddressCodeVisitor()
        {
            Tac = new ThreeAddressCode();
        }
        
        public override void VisitBinOpNode(BinOpNode binop)
        {
            base.VisitBinOpNode(binop);
        }
        
        public override void VisitAssignNode(AssignNode a)
        {
            var rightPartExpression = GenerateThreeAddressLine(a.Expr);
            Tac.PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPart = a.Id.Name,
                FirstOperand = rightPartExpression
            });
        }

        private string GenerateThreeAddressLine(ExprNode expression)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            switch (expression)
            {
                case IdNode tmp:
                {
                    Tac.CreateAndPushIdNode(tmp, tmpName);
                    break;
                }
                case IntNumNode tmp:
                {
                    Tac.CreateAndPushIntNumNode(tmp, tmpName);
                    break;
                }
                case BoolNode tmp:
                {
                    Tac.CreateAndPushBoolNode(tmp, tmpName);
                    break;
                }
                case BinOpNode tmp:
                {
                    var leftPart = GenerateThreeAddressLine(tmp.Left);
                    var rightPart = GenerateThreeAddressLine(tmp.Right);
                    Tac.PushNode(new TacAssignmentNode()
                    {
                        Label = TmpNameManager.Instance.GenerateLabel(),
                        LeftPart = tmpName,
                        FirstOperand = leftPart,
                        Operation = tmp.Op,
                        SecondOperand = rightPart
                    });
                    break;
                }
            }

            return tmpName;
        }
        
        public override void VisitIfNode(IfNode c)
        {
            base.VisitIfNode(c);
        }
    }
}
