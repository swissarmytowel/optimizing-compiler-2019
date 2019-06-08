using System.Collections;
using System.Collections.Generic;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.TacBasicBlocks
{
    public class BasicBlocks : IEnumerable
    {
        public List<ThreeAddressCode> BasicBlockItems { get; set; }

        public BasicBlocks() => BasicBlockItems = new List<ThreeAddressCode>();

        private HashSet<TacNode> FindLeaders(ThreeAddressCode RawTACode)
        {
            var previousGoto = false;
            var firstTAC = RawTACode.TACodeLines.First.Value;
            var leaderLabels = new HashSet<TacNode>{ firstTAC };

            foreach(var currentElement in RawTACode)
            {
                if (previousGoto)
                    leaderLabels.Add(currentElement);

                if (currentElement is TacGotoNode gotoNode)
                    leaderLabels.Add(RawTACode[gotoNode.TargetLabel]);

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
                if (leaderLabels.Contains(currentElement))
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
