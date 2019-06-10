using System;
using System.Collections.Generic;

using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations
{
    public class UnreachableCodeOpt : IOptimizer
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
            var previuosNodes = new HashSet<TacNode>();

            while (currentNode != null)
            {
                var line = currentNode.Value;
                if (line is TacAssignmentNode assignmentNode)
                {
                    var rightPart = $"{assignmentNode.FirstOperand} {assignmentNode.Operation} {assignmentNode.SecondOperand}";
                    if (!variablesValue.ContainsKey(assignmentNode.LeftPartIdentifier))
                        variablesValue.Add(assignmentNode.LeftPartIdentifier, rightPart);
                    else variablesValue[assignmentNode.LeftPartIdentifier] = rightPart;
                }

                if (line is TacIfGotoNode ifGotoNode && Equals(variablesValue[ifGotoNode.Condition], "True") || line.GetType() == typeof(TacGotoNode))
                {
                    var gotoNode = line as TacGotoNode;
                    if (!previuosNodes.Contains(tac[gotoNode.TargetLabel]))
                        if (CheckLabels(labels, currentNode.Next, gotoNode.TargetLabel, linesToDelete))
                        {
                            currentNode.Value = new TacGotoNode { Label = gotoNode.Label, TargetLabel = gotoNode.TargetLabel };
                            isApplied = true;
                        }
                }
                previuosNodes.Add(line);
                currentNode = currentNode.Next;
            }
            if (linesToDelete.Count == 0)
                return false;

            tac.RemoveNodes(linesToDelete);
            
            return isApplied;
        }
    }
}
