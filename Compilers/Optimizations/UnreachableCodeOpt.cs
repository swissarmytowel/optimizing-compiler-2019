using System;
using System.Collections.Generic;

using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations
{
    class UnreachableCodeOpt : IOptimizer
    {
        private HashSet<string> FindAllLabels(ThreeAddressCode tac)
        {
            var setOfLables = new HashSet<string>();

            foreach (var line in tac)
                if (line.Label != null) setOfLables.Add(line.Label);

            return setOfLables;
        }

        private bool CheckLabels(HashSet<string> lables, 
            LinkedListNode<TacNode> ifNode, string targetLabel, List<TacNode> tacNodesToRemove)
        {
            var currentNode = ifNode;
            var line = currentNode.Value;

            while (!Equals(line.Label, targetLabel))
            {
                if (lables.Contains(line.Label))
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
            var labels = FindAllLabels(tac);
            var currentNode = tac.TACodeLines.First;
            var linesToDelete = new List<TacNode>();
            var variablesValue = new Dictionary<string, string>();

            while (currentNode != null)
            {
                var line = currentNode.Value;
                if (line is TacAssignmentNode assignmentNode)
                {
                    if (!variablesValue.ContainsKey(assignmentNode.LeftPartIdentifier))
                        variablesValue.Add(assignmentNode.LeftPartIdentifier, assignmentNode.FirstOperand);
                    variablesValue[assignmentNode.LeftPartIdentifier] = assignmentNode.FirstOperand;
                }

                if (line is TacIfGotoNode ifGotoNode && Equals(variablesValue[ifGotoNode.Condition], "True") || line.GetType() == typeof(TacGotoNode))
                {
                    var gotoNode = line as TacGotoNode;
                    if (CheckLabels(labels, currentNode.Next, gotoNode.TargetLabel, linesToDelete))
                    {
                        currentNode.Value = new TacGotoNode { Label = gotoNode.Label, TargetLabel = gotoNode.TargetLabel };
                        tac.RemoveNodes(linesToDelete);
                        linesToDelete.Clear();
                        isApplied = true;
                    }
                }
                currentNode = currentNode.Next;
            }

            return isApplied;
        }
    }
}
