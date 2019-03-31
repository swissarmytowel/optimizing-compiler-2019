using System;
using System.Linq;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{

    public class BasicBlocks
    {
        public List<List<TacNode>> BasicBlockItems { get; set; }
        private ThreeAddressCode RawTACode { get; }

        public BasicBlocks(ThreeAddressCode TACode)
        {
            BasicBlockItems = new List<List<TacNode>>();
            RawTACode = TACode;
        }

        private List<int> FindLeaders(List<TacNode> tacNodes)
        {
            var position = 0;
            var previousGoto = false;
            var res = new List<int>();

            foreach (var tacItem in tacNodes)
            {
                if (position == 0 || previousGoto)
                    res.Add(position);
                else if (tacItem is TacGotoNode gotoNode)
                {
                    var targetPostiion = RawTACode.GetNodePositonByLabel(gotoNode.TargetLabel);
                    res.Add(targetPostiion);
                    previousGoto = true;
                }
                else previousGoto = false;
                position++;
            }
            res.Sort();
            return res;
        }

        public void SplitTACode()
        {
            var tacNodes = RawTACode.TACodeLines.ToList();
            var leaderNodes = FindLeaders(tacNodes);

            for(int i = 0;i < leaderNodes.Count - 1;i++)
            {
                var currentIndex = leaderNodes[i];
                var nextIndex = leaderNodes[i + 1];
                var block = tacNodes.GetRange(currentIndex, nextIndex - currentIndex);
                BasicBlockItems.Add(block);
            }
        }

    }
}
