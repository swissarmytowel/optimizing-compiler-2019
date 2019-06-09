using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.ConstDistrib;
using SimpleLang.CFG;

namespace SimpleLang.IterationAlgorithms
{
    public class ConstDistributionITA: IterationAlgorithm<SemilatticeStreamValue>
    {
        
        public ConstDistributionITA(ControlFlowGraph cfg)
            :base(cfg, new ConstDistribFunction(), new ConstDistribOperator())
        {
            InitilizationSet = new HashSet<SemilatticeStreamValue>();

            Execute();
        }

    }
}
