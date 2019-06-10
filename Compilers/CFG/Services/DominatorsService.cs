using SimpleLang.TACode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.CFG.DominatorsTree
{
    public class DominatorsService
    {
        private ControlFlowGraph _cfg;
        private DominatorsFinder dominatorsFinder;

        public DominatorsService(ControlFlowGraph cfg)
        {
            _cfg = cfg;
            dominatorsFinder = new DominatorsFinder(cfg);
        }

        public bool IsVertexDominator(ThreeAddressCode v1, ThreeAddressCode v2)
        {
            var dominators = dominatorsFinder.Dominators;
            if (dominators.Keys.Contains(v2))
            {
                HashSet<ThreeAddressCode> v1Dominators = dominators[v2];
                return v1Dominators.Contains(v1);
            }
            else return false;
        }

        public bool IsVertexImmediateDominator(ThreeAddressCode v1, ThreeAddressCode v2)
        {
            var dominators = dominatorsFinder.ImmediateDominators;
            if (dominators.Keys.Contains(v2))
                return dominators[v2] == v1;
            else return false;
        }

        private int FindIndexByBlock(ThreeAddressCode block)
        {
            return _cfg.SourceBasicBlocks.BasicBlockItems.FindIndex(e => e == block);
        }

        private string DominatorsToString(ThreeAddressCode block)
        {
            string text = "";
            var dominators = dominatorsFinder.Dominators;
            if (dominators.Keys.Contains(block))
            {
                HashSet<ThreeAddressCode> dominatorsBlocks = new HashSet<ThreeAddressCode>(dominators[block]);

                if (dominatorsBlocks.Count == 0) {
                    text += "no dominators\n";
                    return text;
                }

                foreach (var code in dominatorsBlocks)
                {
                    text += FindIndexByBlock(code) + " ";
                }
            }
            return text;
        }

        private string ImmediateDominatorsToString(ThreeAddressCode block)
        {
            string text = "";
            var dominators = dominatorsFinder.ImmediateDominators;
            if (dominators.Keys.Contains(block))
                text += FindIndexByBlock(dominators[block]);
            return text;
        }

        public override string ToString()
        {
            string text = "";
            foreach (var block in _cfg.SourceBasicBlocks.BasicBlockItems)
            {
                int blockInd = FindIndexByBlock(block);
                text += "\nblockInd" + blockInd + ":\n";
                text += "Dominator: " + DominatorsToString(block) + " ";
                text += "ImmediateDominator: " + ImmediateDominatorsToString(block);
            }
            return text;
        }
    }
}
