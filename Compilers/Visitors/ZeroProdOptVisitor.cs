using System;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class ZeroProdOptVisitor : ChangeVisitor
    {
        public override void VisitBinOpNode(BinOpNode binop)
        {
            var isZeroLeft = binop.Left is IntNumNode && (binop.Left as IntNumNode).Num == 0;
            var isZeroRight = binop.Right is IntNumNode && (binop.Right as IntNumNode).Num == 0;

            if (string.Equals(binop.Op, "*") && (isZeroRight || isZeroLeft))
                ReplaceExpr(binop, new IntNumNode(0));
            else base.VisitBinOpNode(binop);
        }
    }
}
