using System;
using System.Collections.Generic;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.GenKill.Interfaces;
using System.Linq;
using SimpleLang.GenKill.Implementations;
using SimpleLang.IterationAlgorithms.CollectionOperators;

namespace SimpleLang.IterationAlgorithms
{
    public class ActiveVariablesITA : IterationAlgorithm<TacNode>
    {  
        public ActiveVariablesITA(
            ControlFlowGraph cfg,
            Dictionary<ThreeAddressCode, IExpressionSetsContainer> lines
            ) : base(cfg, new TFByComposition(lines), new UnionCollectionOperator<TacNode>(), false)
        {
            Execute();
        }
    }
}
