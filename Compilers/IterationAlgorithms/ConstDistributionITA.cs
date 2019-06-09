using System.Collections.Generic;
using SimpleLang.ConstDistrib;
using SimpleLang.CFG;

namespace SimpleLang.IterationAlgorithms
{
    public class ConstDistributionITA: IterationAlgorithm<SemilatticeStreamValue>
    {
        public ConstDistributionITA(ControlFlowGraph cfg)
            : base(cfg, new ConstDistribFunction(), new ConstDistribOperator())
        {
            InitilizationSet = new HashSet<SemilatticeStreamValue>();

            Execute();
        }

    }
}
