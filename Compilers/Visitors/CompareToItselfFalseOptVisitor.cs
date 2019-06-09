using System;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class CompareToItselfFalseOptVisitor : ChangeVisitor
    {
        public override void VisitBinOpNode(BinOpNode binop)
        {
            if (binop.Left is IdNode left && binop.Right is IdNode right)
            {
                var isNamesEqual = string.Equals(left.Name, right.Name);
                if (string.Equals(binop.Op, ">") || string.Equals(binop.Op, "!=") && isNamesEqual)
                    ReplaceExpr(binop, new BoolNode(false));
                else base.VisitBinOpNode(binop);
            }
        }
    }
}
