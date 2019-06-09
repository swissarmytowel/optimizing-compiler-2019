using System.Collections.Generic;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.GenKill.Implementations;
using SimpleLang.IterationAlgorithms.CollectionOperators;

namespace SimpleLang.IterationAlgorithms
{
    public class ReachingDefinitionsITA : IterationAlgorithm<TacNode>
    {
        public ReachingDefinitionsITA(
            ControlFlowGraph cfg,
            Dictionary<ThreeAddressCode, IExpressionSetsContainer> lines
            ) : base(cfg, new TFByComposition(lines), new UnionCollectionOperator<TacNode>())
        {
            Execute();
        }
    }
}
