using System;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{

    public class BasicBlocks
    {
        public List<ThreeAddressCode> BasicBlockItems { get; set; }

        public BasicBlocks()
        {
            BasicBlockItems = new List<ThreeAddressCode>();
        }

        private List<string> FindLeaders(ThreeAddressCode RawTACode)
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

        public void SplitTACode(ThreeAddressCode RawTACode)
        {
            var leaderLabels = FindLeaders(RawTACode);
            var leaderPos = 0;
            var basicBlock = new ThreeAddressCode();

            foreach(var currentElement in RawTACode)
            {
                if (string.Equals(currentElement.Label, leaderLabels[leaderPos]))
                {
                    if (leaderPos < leaderLabels.Count - 1 && string.Equals(currentElement.Label, leaderLabels[leaderPos + 1]))
                    {
                        basicBlock.PushNode(currentElement);
                        BasicBlockItems.Add(basicBlock);
                        basicBlock = new ThreeAddressCode();
                        leaderPos++;
                    }
                    else if (basicBlock.TACodeLines.Count > 0)
                    {
                        basicBlock.PushNode(currentElement);
                        BasicBlockItems.Add(basicBlock);
                        basicBlock = new ThreeAddressCode();
                    }
                    else basicBlock.PushNode(currentElement);
                    leaderPos++;

                } else basicBlock.PushNode(currentElement);
            }
        }

    }
}
