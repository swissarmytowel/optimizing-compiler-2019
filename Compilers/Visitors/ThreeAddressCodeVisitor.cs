using System;
using System.Linq;
using System.Text;
using ProgramTree;
using System.Collections.Generic;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Visitors
{
    internal class ThreeAddressCodeVisitor : AutoVisitor
    {
        public ThreeAddressCode Tac { get; }

        public ThreeAddressCodeVisitor()
        {
            Tac = new ThreeAddressCode();
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
            switch (expression)
            {
                case IdNode tmp:
                {
                    return Tac.CreateAndPushIdNode(tmp);
                }
                case IntNumNode tmp:
                {
                    return Tac.CreateAndPushIntNumNode(tmp);
                }
                case BoolNode tmp:
                {
                    return Tac.CreateAndPushBoolNode(tmp);
                }
                case BinOpNode tmp:
                {
                    var leftPart = GenerateThreeAddressLine(tmp.Left);
                    var rightPart = GenerateThreeAddressLine(tmp.Right);
                    var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
                    Tac.PushNode(new TacAssignmentNode()
                    {
                        Label = TmpNameManager.Instance.GenerateLabel(),
                        LeftPart = tmpName,
                        FirstOperand = leftPart,
                        Operation = tmp.Op,
                        SecondOperand = rightPart
                    });
                    return tmpName;
                }
            }
            return default(string);
        }
        
        public override void VisitIfNode(IfNode c)
        {
            base.VisitIfNode(c);
        }
    }
}
