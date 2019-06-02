using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.Optimizations;

namespace SimpleLang.IterationAlgorithms
{
    class ActiveVariablesITA : IterationAlgorithm<string>
    {  
        protected override HashSet<string> TransmissionFunc(ThreeAddressCode tac, HashSet<string> x)
        {
            return new HashSet<string>(defUseSetForBlocks
                .UseSets[tac]
                .Union(x.Except(defUseSetForBlocks.DefSets[tac])));
        }

        protected override HashSet<string> CollectionOperator(HashSet<string> x, HashSet<string> y)
        {
            return new HashSet<string>(x.Union(y));
        }

        DefUseSetForBlocks defUseSetForBlocks;

        public ActiveVariablesITA(ControlFlowGraph cfg) : base(cfg, false)
        {
            InitilizationSet = new HashSet<string>();
            defUseSetForBlocks = new DefUseSetForBlocks(cfg.SourseBasicBlocks);

            Execute();
        }
    }
}
