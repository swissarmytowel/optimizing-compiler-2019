using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.IterationAlgorithms.Interfaces;

namespace SimpleLang.IterationAlgorithms
{
    
    abstract class IterationAlgorithm<T> : IIterationAlgorithm<T>
    {
        public Dictionary<ThreeAddressCode, HashSet<T>> In { get; set; } = new Dictionary<ThreeAddressCode, HashSet<T>>();
        public Dictionary<ThreeAddressCode, HashSet<T>> Out { get; set; } = new Dictionary<ThreeAddressCode, HashSet<T>>();

        private ControlFlowGraph controlFlowGraph;
        private Func<ThreeAddressCode, IEnumerable<ThreeAddressCode>> GetPredVertices;

        protected HashSet<T> InitilizationSet { get; set; }
        protected bool isForwardDirection = true;
        protected abstract HashSet<T> CollectionOperator(HashSet<T> x, HashSet<T> y);
        protected abstract HashSet<T> TransmissionFunc(ThreeAddressCode tac, HashSet<T> x);

        protected IterationAlgorithm(ControlFlowGraph cfg, bool forwardDirection = true)
        {
            controlFlowGraph = cfg;
            isForwardDirection = forwardDirection;
            if (forwardDirection)
            {
                GetPredVertices = x => controlFlowGraph.InEdges(x).Select(e => e.Source);
            }
            else
            {
                GetPredVertices = x => controlFlowGraph.OutEdges(x).Select(e => e.Target);
            }
        }

        protected void Execute()
        {
            var entryPoint = (isForwardDirection) ? controlFlowGraph.EntryBlock : controlFlowGraph.ExitBlock;
            var vertices = (isForwardDirection) ? 
                controlFlowGraph.Vertices.ToList() :
                controlFlowGraph.Vertices.Reverse().ToList();

            Out[entryPoint] = new HashSet<T>();
            foreach(var vertex in vertices.Skip(1))
            {
                Out[vertex] = InitilizationSet;
            }

            var isChanged = true;

            while (isChanged)
            {
                isChanged = false;
                foreach(var vertex in vertices.Skip(1))
                {
                    var pred = GetPredVertices(vertex)
                        .Select(e => Out[e])
                        .Aggregate((a,b) => CollectionOperator(a,b));

                    In[vertex] = pred;
                    var tmp = Out[vertex];
                    Out[vertex] = TransmissionFunc(vertex, In[vertex]);
                    if (!tmp.SequenceEqual(Out[vertex]))
                    {
                        isChanged = true;
                    }
                }
            }

            if (!isForwardDirection)
            {
                var tmp = Out;
                Out = In;
                In = tmp;
            }
        }

    }
}
