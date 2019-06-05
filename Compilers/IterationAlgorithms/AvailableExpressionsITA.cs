using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.GenKill.Implementations;

namespace SimpleLang.IterationAlgorithms
{
    class AvailableExpressionsITA: IterationAlgorithm
    {
        protected override HashSet<TacNode> CollectionOperator(HashSet<TacNode> x, HashSet<TacNode> y)
        {
            return new HashSet<TacNode>(x.Intersect(y));
        }

        public AvailableExpressionsITA(
            ControlFlowGraph cfg,
            Dictionary<ThreeAddressCode, IExpressionSetsContainer> lines) : base(cfg, new TFByComposition(lines))
        {
            InitilizationSet = lines
                .Values
                .Aggregate(new HashSet<TacNode>(), (a, b) => new HashSet<TacNode>(a.Union(b.GetFirstSet().Union(b.GetSecondSet()))));

            Execute();
        }
    }
}
