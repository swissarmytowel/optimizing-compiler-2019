using System;
using System.Collections.Generic;
using System.Linq;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.IterationAlgorithms.Interfaces;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.InOut;
using SimpleLang.IterationAlgorithms.CollectionOperators;


namespace SimpleLang.IterationAlgorithms
{
    public abstract class IterationAlgorithm<T> :IIterationAlgorithm<T>
    {
        public InOutContainer<T> InOut { get; set; } = new InOutContainer<T>();

        private ControlFlowGraph controlFlowGraph;
        private Func<ThreeAddressCode, IEnumerable<ThreeAddressCode>> GetPredVertices;
        private Func<HashSet<T>, ThreeAddressCode, HashSet<T>> TransmissionFunc;
        private Func<HashSet<T>, HashSet<T>, HashSet<T>> CollectionOperator;

        protected HashSet<T> InitilizationSet { get; set; } = new HashSet<T>();
        protected bool isForwardDirection = true;
        //protected abstract HashSet<TacNode> CollectionOperator(HashSet<TacNode> x, HashSet<TacNode> y);

        protected IterationAlgorithm(
            ControlFlowGraph cfg,
            ITransmissionFunction<T> func,
            ICollectionOperator<T> collectionOperator,
            bool forwardDirection = true)
        {
            controlFlowGraph = cfg;
            TransmissionFunc = func.Calculate;
            CollectionOperator = collectionOperator.Collect;
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
            var entryPoints = (isForwardDirection) ? 
                controlFlowGraph.Vertices.Where(e => controlFlowGraph.InDegree(e) == 0):
                controlFlowGraph.Vertices.Where(e => controlFlowGraph.OutDegree(e) == 0);

            var vertices = (isForwardDirection) ? 
                controlFlowGraph.Vertices.ToList() :
                controlFlowGraph.Vertices.Reverse().ToList();

            foreach(var vertex in vertices)
            {
                InOut.Out[vertex] = InitilizationSet;
            }

            var isChanged = true;

            while (isChanged)
            {
                isChanged = false;
                foreach(var vertex in vertices)
                {
                    var pred = (entryPoints.Contains(vertex))?
                        new HashSet<T>() :
                        GetPredVertices(vertex)
                        .Select(e => InOut.Out[e])
                        .Aggregate((a,b) => CollectionOperator(a,b));

                    InOut.In[vertex] = pred;
                    var tmp = InOut.Out[vertex];
                    InOut.Out[vertex] = TransmissionFunc(InOut.In[vertex], vertex);
                    if (!tmp.SequenceEqual(InOut.Out[vertex]))
                    {
                        isChanged = true;
                    }
                }
            }

            if (!isForwardDirection)
            {
                var tmp = InOut.Out;
                InOut.Out = InOut.In;
                InOut.In = tmp;
            }
        }
    }
}
