using ProgramTree;

namespace SimpleLang.Visitors
{
    class ThirdOptimizationVisitor : ChangeVisitor
    {
        public override void VisitBinOpNode(BinOpNode binop)
        {
            base.VisitBinOpNode(binop);
            if (binop.Left is IntNumNode left && binop.Right is IntNumNode right)
            {
                var num = 0;

                switch (binop.Op)
                {
                    case "+":
                        num = left.Num + right.Num;
                        break;
                    case "-":
                        num = left.Num - right.Num;
                        break;
                    case "*":
                        num = left.Num * right.Num;
                        break;
                    case "/":
                        num = left.Num / right.Num;
                        break;
                }

                var newInt = new IntNumNode(num);
                ReplaceExpr(binop, newInt);
            }
        }
    }
}
