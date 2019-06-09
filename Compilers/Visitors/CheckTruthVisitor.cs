using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class CheckTruthVisitor: ChangeVisitor
    {
        public override void VisitBinOpNode(BinOpNode binop)
        {
            base.VisitBinOpNode(binop);
            if (binop.Op == "<")
            {
                ExprNode expr1 = binop.Left;
                ExprNode expr2 = binop.Right;
                
                if (expr1 is IntNumNode ex && expr2 is IntNumNode ex2)
                {
                    if (ex.Num < ex2.Num)
                    {
                        ReplaceExpr(expr1.Parent as ExprNode, new BoolNode(true));
                    }
                }
            }
        }

    }
}
