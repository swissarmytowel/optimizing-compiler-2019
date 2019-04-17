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
                var next = node.Next;
                var val = node.Value;
                var label = val.Label;
                if (next != null)
                {
                    var nextVal = next.Value;
                    if (val is TacIfGotoNode && nextVal is TacGotoNode)
                    {
                        isUsed = true;
                        var ifVal = val as TacIfGotoNode;
                        ifVal.Condition = "!(" + ifVal.Condition + ")";
                        var oldTarget = ifVal.TargetLabel;
                        ifVal.TargetLabel = (nextVal as TacGotoNode).TargetLabel;
                        var remove = next;
                        next = next.Next;
                        tac.TACodeLines.Remove(remove);
                    }
                }
                node = next;
                
            }
            return isUsed;
        }
    }
}
