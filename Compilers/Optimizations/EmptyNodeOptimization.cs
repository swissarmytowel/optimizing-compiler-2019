using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{
    class EmptyNodeOptimization : IOptimizer
    {
        public bool Optimize(ThreeAddressCode tac)
        {
            bool isUsed = false;
            var node = tac.TACodeLines.First;

            while (node != null)
            {
                var next = node.Next;
                var val = node.Value;
                var label = val.Label;
                if (val is TacEmptyNode)
                {
                    isUsed = true;
                    if (next != null)
                    {
                        next.Value.Label = label;
                    }
                    tac.TACodeLines.Remove(node);
                }
                node = next;
            }
            return isUsed;
        }
    }
}
