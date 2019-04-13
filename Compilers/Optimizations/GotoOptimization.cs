using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{
    class GotoOptimization : IOptimizer
    {
        public bool Optimize(ThreeAddressCode tac)
        {
            bool isUsed = false;
            var node = tac.TACodeLines.First;
            
            while (node != null)
            {
                var val = node.Value;
                var label = val.Label;
                if (val is TacEmptyNode)
                {
                    isUsed = true;
                    if (node.Next != null) {
                        node.Next.Value.Label = label;
                    }
                    tac.TACodeLines.Remove(node);
                }
                node = node.Next;
            }
            return isUsed;
        }
    }
}
