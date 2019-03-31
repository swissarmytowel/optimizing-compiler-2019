using System;
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

        private List<string> FindLeaders()
        {
            var previousGoto = false;
            var leaderLabels = new List<string>{ "L1" };

            var currentNode = RawTACode.TACodeLines.First;
            var next = currentNode.Next;

            while(next != null)
            {
                var currentElement = currentNode.Value;
                if (currentElement is TacEmptyNode || previousGoto)
                {
                    leaderLabels.Add(currentNode.Previous.Value.Label);
                    leaderLabels.Add(currentElement.Label);
                }
                previousGoto = currentElement is TacGotoNode;
                currentNode = next;
                next = currentNode.Next;
            }
            leaderLabels.Add(RawTACode.TACodeLines.Last.Value.Label);
            return leaderLabels;
        }

        public void SplitTACode()
        {
            var leaderLabels = FindLeaders();
            var leaderPos = 0;
            var basicBlock = new List<TacNode>();

            foreach(var currentElement in RawTACode)
            {
                if (string.Equals(currentElement.Label, leaderLabels[leaderPos]))
                {
                    if (leaderPos < leaderLabels.Count - 1 && string.Equals(currentElement.Label, leaderLabels[leaderPos + 1]))
                    {
                        BasicBlockItems.Add(new List<TacNode> { currentElement });
                        leaderPos++;
                    }
                    else if (basicBlock.Count > 0)
                    {
                        basicBlock.Add(currentElement);
                        BasicBlockItems.Add(basicBlock);
                        basicBlock = new List<TacNode>();
                    }
                    else basicBlock.Add(currentElement);
                    leaderPos++;

                } else basicBlock.Add(currentElement);
            }
        }

    }
}
