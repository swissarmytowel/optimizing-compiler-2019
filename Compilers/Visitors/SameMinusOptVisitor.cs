using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class SameMinusOptVisitor : ChangeVisitor
    {
        public override void VisitBinOpNode(BinOpNode binop)
        {
            base.VisitBinOpNode(binop);
            if (binop.Op == "-")
            {
                ExprNode expr1 = binop.Left;
                ExprNode expr2 = binop.Right;
                if (expr1 is IdNode && expr2 is IdNode)
                {
                    if ((expr1 as IdNode).Name == (expr2 as IdNode).Name)
                    {
                        ReplaceExpr(expr1.Parent as ExprNode, new IntNumNode(0));
                    }
                }
            }
        }
    }
}
