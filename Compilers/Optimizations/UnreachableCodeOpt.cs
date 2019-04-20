using System;
using System.Collections.Generic;

using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations
{
    class UnreachableCodeOpt : IOptimizer
    {
        private HashSet<object> FindAllTargetLabels(ThreeAddressCode tac)
        {
            var setOfLables = new HashSet<object>();

            foreach (var line in tac)
                if (line is TacGotoNode gotoNode)
                    setOfLables.Add(gotoNode.TargetLabel);
            return setOfLables;
        }

        private bool CheckLabels(HashSet<object> targetLabels, 
            LinkedListNode<TacNode> ifNode, object targetLabel, List<TacNode> tacNodesToRemove)
        {
            var currentNode = ifNode;
            var line = currentNode.Value;

            while (!Equals(line.Label, targetLabel))
            {
                if (targetLabels.Contains(line.Label))
                {
                    tacNodesToRemove.Clear();
                    return false;
                }
                tacNodesToRemove.Add(line);
                currentNode = currentNode.Next;
                line = currentNode.Value;
            }

            return true;
        }

        public bool Optimize(ThreeAddressCode tac)
        {
            var isApplied = false;
            var targetLabels = FindAllTargetLabels(tac);

            var currentNode = tac.TACodeLines.First;
            var linesToDelete = new List<TacNode>();

            while (currentNode != null)
            {
                var line = currentNode.Value;

                if (line is TacIfGotoNode ifGotoNode && Equals(ifGotoNode.Condition, "true"))
                {
                    if (CheckLabels(targetLabels, currentNode.Next, ifGotoNode.TargetLabel, linesToDelete))
                    {
                        tac[line.Label] = new TacGotoNode { Label = ifGotoNode.Label, TargetLabel = ifGotoNode.TargetLabel };
                        tac.RemoveNodes(linesToDelete);
                        linesToDelete.Clear();
                    }
                }
                currentNode = currentNode.Next;
            }

            return isApplied;
        }
    }
}
