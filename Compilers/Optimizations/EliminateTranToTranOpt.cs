using System;
using System.Linq;
using System.Collections.Generic;

using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{
    public class EliminateTranToTranOpt : IOptimizer
    {
        private List<TacGotoNode> FindGotoNodes(ThreeAddressCode tac)
        {
            var res = new List<TacGotoNode>();
            var targetLabels = new Dictionary<object, int>();
            var prohibitedTargets = new HashSet<object>();

            foreach (var line in tac)
            {
                if (line.GetType() == typeof(TacGotoNode))
                {
                    var gotoNode = line as TacGotoNode;

                    if (!prohibitedTargets.Contains(gotoNode.TargetLabel))
                    {
                        prohibitedTargets.Add(gotoNode.TargetLabel);
                        res.Add(gotoNode);
                    }
                    else res = res.Where(x => x.TargetLabel != gotoNode.TargetLabel).ToList();
                }
            }

            return res;
        }

        public bool Optimize(ThreeAddressCode tac)
        {
            var currentNode = tac.TACodeLines.First;
            var targets = FindGotoNodes(tac);

            while(currentNode != null)
            {
                var line = currentNode.Value;

                if (line is TacGotoNode gotoNode && tac[gotoNode.TargetLabel] is TacGotoNode tacGoto)
                {
                    gotoNode.TargetLabel = tacGoto.TargetLabel;
                }
                currentNode = currentNode.Next;
            }
            return true;
        }
    }
}
