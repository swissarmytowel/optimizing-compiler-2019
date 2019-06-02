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
    class AvailableExpressionsITA<T> : IterationAlgorithm<T>
    {
        protected override HashSet<T> CollectionOperator(HashSet<T> x, HashSet<T> y)
        {
            return new HashSet<T>(x.Intersect(y));
        }

        protected override HashSet<T> TransmissionFunc(ThreeAddressCode tac, HashSet<T> x)
        {
            throw new NotImplementedException();
        }

        public AvailableExpressionsITA(ControlFlowGraph cfg) : base(cfg)
        {
            InitilizationSet = new HashSet<T>();
            Execute();
        }
    }
}
