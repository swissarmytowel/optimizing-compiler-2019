using System;
using System.Collections;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;


namespace SimpleLang.Optimizations
{

    public class BasicBlocks : IEnumerable
    {
        public List<ThreeAddressCode> BasicBlockItems { get; set; }

        public BasicBlocks() => BasicBlockItems = new List<ThreeAddressCode>();

        private HashSet<object> FindLeaders(ThreeAddressCode RawTACode)
        {
            var previousGoto = false;
            var firstLabel = RawTACode.TACodeLines.First.Value.Label;
            var leaderLabels = new HashSet<object>{ firstLabel };

            foreach(var currentElement in RawTACode)
            {
                if (previousGoto)
                    leaderLabels.Add(currentElement.Label);

                if (currentElement is TacGotoNode gotoNode)
                    leaderLabels.Add(gotoNode.TargetLabel);

                previousGoto = currentElement is TacGotoNode;
            }
            return leaderLabels;
        }

        public void SplitTACode(ThreeAddressCode RawTACode)
        {
            var leaderLabels = FindLeaders(RawTACode);
            var isFirstLeader = true;
            var basicBlock = new ThreeAddressCode();

            foreach(var currentElement in RawTACode)
            {
                if (leaderLabels.Contains(currentElement.Label))
                {
                    if (isFirstLeader) isFirstLeader = false;
                    else
                    {
                        BasicBlockItems.Add(basicBlock);
                        basicBlock = new ThreeAddressCode();
                    }
                }
                basicBlock.PushNode(currentElement);
            }
            if (basicBlock.TACodeLines.Count > 0) BasicBlockItems.Add(basicBlock);
        }

        public IEnumerator<ThreeAddressCode> GetEnumerator()
        {
            return BasicBlockItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return BasicBlockItems.GetEnumerator();
        }

    }
}
