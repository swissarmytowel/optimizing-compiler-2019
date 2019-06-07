using System;
using System.Linq;
using System.Collections.Generic;

using SimpleLang.CFG;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.IterationAlgorithms.Interfaces;
using SimpleLang.IterationAlgorithms.CollectionOperators;
using SimpleLang.GenKill.Interfaces;

namespace SimpleLang.MOP
{
    public class MeetOverPaths : IIterationAlgorithm<TacNode>
    {
        public ControlFlowGraph ControlFlowGraph { get; }
        public Dictionary<ThreeAddressCode, HashSet<TacNode>> In { get; set; }
        public Dictionary<ThreeAddressCode, HashSet<TacNode>> Out { get; set; }
        public ITransmissionFunction TransmissionFunction { get; }
        public ICollectionOperator CollectionOperator { get; }
        public bool IsForwardDirection { get; }

        public MeetOverPaths(ControlFlowGraph controlFlowGraph,
            ITransmissionFunction transmissionFunction,
            ICollectionOperator collectionOperator,
            HashSet<TacNode> initSet,
            bool isForwardDirection=true
            )
        {
            TransmissionFunction = transmissionFunction;
            ControlFlowGraph = controlFlowGraph;
            CollectionOperator = collectionOperator;
            IsForwardDirection = isForwardDirection;
            In = new Dictionary<ThreeAddressCode, HashSet<TacNode>>();
            Out = new Dictionary<ThreeAddressCode, HashSet<TacNode>>();
        }

        public void Compute()
        {
            var visited = new Dictionary<ThreeAddressCode, bool>();
            foreach (var bblock in ControlFlowGraph.SourseBasicBlocks)
            {
                visited.Add(bblock, false);
                //var tfResult = TransmissionFunction.Calculate(new HashSet<TacNode>(), ControlFlowGraph.EntryBlock);
                Out.Add(bblock, new HashSet<TacNode>());
                In.Add(bblock, new HashSet<TacNode>());
            }
                

            var isChanged = true;

            while (isChanged)
            {
                var predecessors = new Stack<ThreeAddressCode>();
                var outBefore = new Dictionary<ThreeAddressCode, HashSet<TacNode>>(Out);
                DepthFirstSearch(ControlFlowGraph.EntryBlock, visited, predecessors);

                isChanged = false;
                foreach (var _out in Out)
                {
                    var key = _out.Key;
                    isChanged = isChanged || !_out.Value.SequenceEqual(outBefore[key]);
                }
            }

        }

        private void DepthFirstSearch(
            ThreeAddressCode currentBlock,
            Dictionary<ThreeAddressCode, bool> visited,
            Stack<ThreeAddressCode> predecessors
            )
        {
            visited[currentBlock] = true;

            var collectionOperatorResult = new HashSet<TacNode>();
            if (predecessors.Count > 0)
                collectionOperatorResult = predecessors
                .Select(e => Out[e])
                .Aggregate((a, b) => CollectionOperator.Collect(a, b));

            In[currentBlock] = CollectionOperator.Collect(collectionOperatorResult, In[currentBlock]);
            var outBefore = Out[currentBlock];
            Out[currentBlock] = TransmissionFunction.Calculate(In[currentBlock], currentBlock);

            if (ControlFlowGraph.IsOutEdgesEmpty(currentBlock))
            {
                visited[currentBlock] = false;
                return;
            }

            predecessors.Push(currentBlock);
            foreach (var outEdge in ControlFlowGraph.OutEdges(currentBlock))
            {
                var outVertex = outEdge.Target;
                if (!visited[outVertex]) DepthFirstSearch(outVertex, visited, predecessors);
            }

            predecessors.Pop();
            visited[currentBlock] = false;
        }
    }
}
