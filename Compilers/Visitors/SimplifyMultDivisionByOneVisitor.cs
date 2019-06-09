using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class SimplifyMultDivisionByOneVisitor : ChangeVisitor
    {
        public override void VisitBinOpNode(BinOpNode binop)
        {
            base.VisitBinOpNode(binop);
            if (binop.Op == "*")
            {
                ExprNode expr1 = binop.Left;
                ExprNode expr2 = binop.Right;
                if (expr1 is IntNumNode expr && expr.Num == 1)
                {
                    ReplaceExpr(expr1.Parent as ExprNode, expr2);
                }
                else if (expr2 is IntNumNode exp && exp.Num == 1)
                {
                    ReplaceExpr(expr2.Parent as ExprNode, expr1);
                }
            }
            if (binop.Op == "/")
            {
                ExprNode expr1 = binop.Left;
                ExprNode expr2 = binop.Right;
                if (expr2 is IntNumNode exp && exp.Num == 1)
                {
                    ReplaceExpr(expr2.Parent as ExprNode, expr1);
                }
            }
        }
    }
}
