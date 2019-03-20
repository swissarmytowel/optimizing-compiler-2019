using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class MaxOpExprVisitor: AutoVisitor
    {
        public int MaxCount = 0;

        public string Result
        {
            get { return $"Максимальное количество операций: = {MaxCount}"; }
        }

        private int GetOpCountBinOpNode(BinOpNode binop)
        {
            int res = 1;
            if (binop.Left is BinOpNode)
                res += GetOpCountBinOpNode(binop.Left as BinOpNode);
            if (binop.Right is BinOpNode)
                res += GetOpCountBinOpNode(binop.Right as BinOpNode);
            return res;
        }

        public override void VisitBinOpNode(BinOpNode binop)
        {
            int count = GetOpCountBinOpNode(binop);
            if (count > MaxCount)
                MaxCount = count;
        }
    }
}
