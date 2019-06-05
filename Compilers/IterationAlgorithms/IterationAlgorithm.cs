using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.IterationAlgorithms.Interfaces;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.GenKill.Implementations;


namespace SimpleLang.IterationAlgorithms
{
    abstract class IterationAlgorithm :IIterationAlgorithm<TacNode>
    {
        public Dictionary<ThreeAddressCode, HashSet<TacNode>> In { get; set; } = new Dictionary<ThreeAddressCode, HashSet<TacNode>>();
        public Dictionary<ThreeAddressCode, HashSet<TacNode>> Out { get; set; } = new Dictionary<ThreeAddressCode, HashSet<TacNode>>();

        private ControlFlowGraph controlFlowGraph;
        private Func<ThreeAddressCode, IEnumerable<ThreeAddressCode>> GetPredVertices;
        private Func<HashSet<TacNode>, ThreeAddressCode, HashSet<TacNode>> TransmissionFunc;

        protected HashSet<TacNode> InitilizationSet { get; set; } = new HashSet<TacNode>();
        protected bool isForwardDirection = true;
        protected abstract HashSet<TacNode> CollectionOperator(HashSet<TacNode> x, HashSet<TacNode> y);

        protected IterationAlgorithm(
            ControlFlowGraph cfg,
            ITransmissionFunction func,
            bool forwardDirection = true)
        {
            controlFlowGraph = cfg;
            TransmissionFunc = func.Calculate;
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

            Out[entryPoint] = new HashSet<TacNode>();
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
                    Out[vertex] = TransmissionFunc(In[vertex], vertex);
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

        public override string ToString()
        {
            var builder = new StringBuilder();
            var numBlock = 0;

            foreach (var inItem in In)
            {
                builder.Append($"--- IN {numBlock} :\n");
                if (inItem.Value.Count == 0)
                {
                    builder.Append("null");
                }
                else
                {
                    var tmp = 0;
                    foreach (var value in inItem.Value)
                    {
                        builder.Append($"{tmp++})");
                        builder.Append(value.ToString());
                        builder.Append("\n");
                    }
                }

                builder.Append($"\n--- OUT {numBlock}:\n");
                if (!Out.TryGetValue(inItem.Key, out _) || Out[inItem.Key].Count == 0)
                {
                    builder.Append("null");
                }
                else
                {
                    var tmp = 0;
                    foreach (var value in Out[inItem.Key])
                    {
                        builder.Append($"{tmp++})");
                        builder.Append(value.ToString());
                        builder.Append("\n");
                    }
                }

                builder.Append("\n");
                numBlock++;
            }

            return builder.ToString();
        }

    }
}
