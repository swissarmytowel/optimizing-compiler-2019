using System;
using System.Linq;
using System.Collections.Generic;
using QuickGraph;

using SimpleLang.CFG;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.IterationAlgorithms.Interfaces;
using SimpleLang.IterationAlgorithms.CollectionOperators;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.InOut;

namespace SimpleLang.MOP
{
    public class MeetOverPaths : IIterationAlgorithm
    {
        public ControlFlowGraph ControlFlowGraph { get; }
        public ITransmissionFunction TransmissionFunction { get; }
        public ICollectionOperator CollectionOperator { get; }
        public bool IsForwardDirection { get; }
        public InOutContainer InOut { get; set; }
        public HashSet<TacNode> InitSet { get; set; }

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
            InitSet = initSet;
            InOut = new InOutContainer();
        }

        public void Compute()
        {
            var visited = new Dictionary<ThreeAddressCode, bool>();
            foreach (var bblock in ControlFlowGraph.SourceBasicBlocks)
            {
                visited.Add(bblock, false);
                InOut.Out.Add(bblock, InitSet);
                InOut.In.Add(bblock, new HashSet<TacNode>());
            }
                

            var isChanged = true;
            var entryBlock = IsForwardDirection ? ControlFlowGraph.EntryBlock : ControlFlowGraph.ExitBlock;


            while (isChanged)
            {
                var predecessors = new Stack<ThreeAddressCode>();
                var outBefore = new Dictionary<ThreeAddressCode, HashSet<TacNode>>(InOut.Out);
                if (IsForwardDirection)
                    DepthFirstSearch(entryBlock, visited, predecessors, ControlFlowGraph.IsOutEdgesEmpty, ControlFlowGraph.OutEdges);
                else DepthFirstSearch(entryBlock, visited, predecessors, ControlFlowGraph.IsInEdgesEmpty, ControlFlowGraph.InEdges);

                isChanged = false;
                foreach (var _out in InOut.Out)
                {
                    var key = _out.Key;
                    isChanged = isChanged || !_out.Value.SequenceEqual(outBefore[key]);
                }
            }

            if (!IsForwardDirection)
            {
                var tmp = InOut.Out;
                InOut.Out = InOut.In;
                InOut.In = tmp;
            }
        }

        private void DepthFirstSearch(
            ThreeAddressCode currentBlock,
            Dictionary<ThreeAddressCode, bool> visited,
            Stack<ThreeAddressCode> predecessors,
            Func<ThreeAddressCode, bool> checkForNext,
            Func<ThreeAddressCode ,IEnumerable<Edge<ThreeAddressCode>>> getNextEdges
            )
        {
            visited[currentBlock] = true;

            var collectionOperatorResult = new HashSet<TacNode>();
            if (predecessors.Count > 0)
                collectionOperatorResult = predecessors
                .Select(e => InOut.Out[e])
                .Aggregate((a, b) => CollectionOperator.Collect(a, b));

            InOut.In[currentBlock] = CollectionOperator.Collect(collectionOperatorResult, InOut.In[currentBlock]);
            var outBefore = InOut.Out[currentBlock];
            InOut.Out[currentBlock] = TransmissionFunction.Calculate(InOut.In[currentBlock], currentBlock);

            if (checkForNext(currentBlock))
            {
                visited[currentBlock] = false;
                return;
            }

            predecessors.Push(currentBlock);
            foreach (var outEdge in getNextEdges(currentBlock))
            {
                var outVertex = IsForwardDirection ? outEdge.Target : outEdge.Source;
                if (!visited[outVertex]) DepthFirstSearch(outVertex, visited, predecessors, checkForNext, getNextEdges);
            }

            predecessors.Pop();
            visited[currentBlock] = false;
        }
    }
}
