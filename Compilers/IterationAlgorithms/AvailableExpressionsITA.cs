using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.GenKill.Implementations;
using SimpleLang.IterationAlgorithms.CollectionOperators;

namespace SimpleLang.IterationAlgorithms
{
    public class AvailableExpressionsITA: IterationAlgorithm<TacNode>
    {
        public AvailableExpressionsITA(
            ControlFlowGraph cfg,
            Dictionary<ThreeAddressCode, IExpressionSetsContainer> lines
            ) : base(cfg, new TFByComposition(lines), new IntersectCollectionOperator<TacNode>())
        {
            InitilizationSet = lines
                .Values
                .Aggregate(new HashSet<TacNode>(), 
                (a, b) => new HashSet<TacNode>(a.Union(b.GetFirstSet().Union(b.GetSecondSet()))));

            Execute();
        }
    }
}
