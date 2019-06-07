using System.Collections.Generic;
using System.Linq;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.GenKill.Implementations;

namespace SimpleLang.IterationAlgorithms
{
    class ReachingDefinitionsITA : IterationAlgorithm
    {
        protected override HashSet<TacNode> CollectionOperator(HashSet<TacNode> x, HashSet<TacNode> y)
        {
            return new HashSet<TacNode>(x.Union(y));
        }

        public ReachingDefinitionsITA(
            ControlFlowGraph cfg,
            Dictionary<ThreeAddressCode, IExpressionSetsContainer> lines) : base(cfg, new TFByComposition(lines))
        {
            Execute();
        }
    }
}
