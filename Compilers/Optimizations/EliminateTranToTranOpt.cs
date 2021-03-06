﻿using System;
using System.Linq;
using System.Collections.Generic;

using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{
    public class EliminateTranToTranOpt : IOptimizer
    {
        private List<LinkedListNode<TacNode>> FindGotoNodes(ThreeAddressCode tac)
        {
            var currentNode = tac.TACodeLines.First;
            var gotoNodes = new List<LinkedListNode<TacNode>>();
            var res = new List<LinkedListNode<TacNode>>();
            var targetLabels = new Dictionary<string, int>();
            var previousNodes = new HashSet<TacNode>();

            while (currentNode != null)
            {
                var currentValue = currentNode.Value;

                if (currentValue.GetType() == typeof(TacGotoNode))
                {
                    var tacGoto = currentValue as TacGotoNode;

                    if (tac[tacGoto.TargetLabel].GetType() == typeof(TacIfGotoNode) && !previousNodes.Contains(tac[tacGoto.TargetLabel]))
                    {
                        if (targetLabels.ContainsKey(tacGoto.TargetLabel))
                            targetLabels[tacGoto.TargetLabel]++;
                        else targetLabels.Add(tacGoto.TargetLabel, 1);

                        gotoNodes.Add(currentNode);
                    }
                }
                previousNodes.Add(currentValue);
                currentNode = currentNode.Next;
            }

            var usingTargets = new HashSet<string>(targetLabels
                .Where(x => x.Value == 1)
                .Select(x => x.Key));

            foreach(var node in gotoNodes)
            {
                var gotoNode = node.Value as TacGotoNode;
                if (usingTargets.Contains(gotoNode.TargetLabel))
                    res.Add(node);
            }

            return res;
        }

        private int FindNumberNextLabel(ThreeAddressCode tac)
        {
            var maxNumber = int.MinValue;

            foreach(var t in tac)
            {
                if (t.Label != null)
                {
                    var currentNumber = int.Parse(t.Label.Remove(0, 1));
                    if (currentNumber > maxNumber) maxNumber = currentNumber;
                }
            }

            return ++maxNumber;
        }

        public bool Optimize(ThreeAddressCode tac)
        {
            var goToNodes = FindGotoNodes(tac);
            var isApplied = false;
            var nextLabel = FindNumberNextLabel(tac);

            foreach(var gotoNode in goToNodes)
            {
                var label = $"L{nextLabel}";
                var gotoValue = gotoNode.Value as TacGotoNode;
                var targetNode = tac.TACodeLines.Find(tac[gotoValue.TargetLabel]);
                var nextNode = targetNode.Next;

                if (nextNode.Value.Label != null)
                    label = nextNode.Value.Label;

                nextNode.Value.Label = label;
                targetNode.Value.Label = null;

                tac.TACodeLines.Remove(targetNode);
                tac.TACodeLines.AddAfter(gotoNode, new TacGotoNode { TargetLabel = label });
                tac.TACodeLines.AddAfter(gotoNode, targetNode);
                tac.TACodeLines.Remove(gotoNode);
                nextLabel++;
            }

            var previousNodes = new HashSet<TacNode>();

            foreach (var line in tac.TACodeLines)
            {
                if (line is TacGotoNode gotoNode 
                    && tac[gotoNode.TargetLabel].GetType() == typeof(TacGotoNode) 
                    && !previousNodes.Contains(tac[gotoNode.TargetLabel]))
                {
                    var tacGoto = tac[gotoNode.TargetLabel] as TacGotoNode;
                    gotoNode.TargetLabel = tacGoto.TargetLabel;
                    isApplied = true;
                }
                previousNodes.Add(line);
            }

            return isApplied;
        }
    }
}
