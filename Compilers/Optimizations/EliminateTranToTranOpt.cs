using System;
using System.Collections.Generic;

using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{
    public class RemoveTranToTranOpt : IOptimizer
    {
        private Dictionary<object, int> FindTargets(ThreeAddressCode tac)
        {
            var res = new Dictionary<object, int>();
            for (var line in tac)
            {
                if (line is TacGotoNode gotoNode)
                {
                    if (res.ContainsKey(gotoNode.TargetLabel))
                        res[gotoNode.TargetLabel]++;
                    else res.Add() 
                }
            }
        }

        public bool Optimize(ThreeAddressCode tac)
        {
            var currentNode = tac.TACodeLines.First;

            while(currentNode != null)
            {
                var line = currentNode.Value;
                if (line is TacGotoNode gotoNode && tac[gotoNode.TargetLabel] is TacGotoNode tacGoto)
                {
                    gotoNode.TargetLabel = tacGoto.TargetLabel
                }
                currentNode = currentNode.Next;
            }
        }
    }
}
