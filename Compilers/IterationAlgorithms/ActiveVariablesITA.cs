using System;
using System.Collections.Generic;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.GenKill.Interfaces;
using System.Linq;
using SimpleLang.GenKill.Implementations;

namespace SimpleLang.IterationAlgorithms
{
    class ActiveVariablesITA : IterationAlgorithm
    {  
        protected override HashSet<TacNode> CollectionOperator(HashSet<TacNode> x, HashSet<TacNode> y)
        {
            return new HashSet<TacNode>(x.Union(y));
        }

        public ActiveVariablesITA(
            ControlFlowGraph cfg,
            Dictionary<ThreeAddressCode, IExpressionSetsContainer> lines
            ) : base(cfg, new TFByComposition(lines), false)
        {
            Execute();
        }
    }
}
