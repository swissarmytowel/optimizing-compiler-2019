using System;
using System.Linq;
using System.Collections.Generic;

using SimpleLang.CFG;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.IterationAlgorithms.Interfaces;
using SimpleLang.GenKill.Interfaces;

namespace SimpleLang.MOP
{
    public class MeetOverPaths : IIterationAlgorithm<TacNode>
    {
        public ControlFlowGraph ControlFlowGraph { get; }
        public Dictionary<ThreeAddressCode, HashSet<TacNode>> In { get; set; }
        public Dictionary<ThreeAddressCode, HashSet<TacNode>> Out { get; set; }
        public ITransmissionFunction TransmissionFunction { get; }

        public MeetOverPaths(ControlFlowGraph controlFlowGraph, ITransmissionFunction transmissionFunction)
        {
            TransmissionFunction = transmissionFunction;
            ControlFlowGraph = controlFlowGraph;
            In = new Dictionary<ThreeAddressCode, HashSet<TacNode>>();
            Out = new Dictionary<ThreeAddressCode, HashSet<TacNode>>();
        }

        public void Compute()
        {
            var visited = new Dictionary<ThreeAddressCode, bool>();
            foreach (var bblock in ControlFlowGraph.SourseBasicBlocks)
            {
                visited.Add(bblock, false);
                var tfResult = TransmissionFunction.Calculate(new HashSet<TacNode>(), ControlFlowGraph.EntryBlock);
                Out.Add(bblock, tfResult);
            }
                

            var predecessors = new Stack<ThreeAddressCode>();
            var isChanged = true;

            while (isChanged)
                DepthFirstSearch(ControlFlowGraph.EntryBlock, visited, predecessors, ref isChanged);

        }

        private void DepthFirstSearch(
            ThreeAddressCode currentBlock,
            Dictionary<ThreeAddressCode, bool> visited,
            Stack<ThreeAddressCode> predecessors,
            ref bool isChanged
            )
        {
            if (ControlFlowGraph.IsOutEdgesEmpty(currentBlock)) return;
            visited[currentBlock] = true;

            foreach (var outEdge in ControlFlowGraph.OutEdges(currentBlock))
            {
                var outVertex = outEdge.Target;
                var sourceVertex = outEdge.Source;

                //In[sourceVertex] = операторСбора(predecessors);
                //var outBefore = Out[sourceVertex];
                //Out[sourceVertex] = TransmissionFunction.Calculate(In[sourceVertex], sourceVertex);
                //isChanged = !outBefore.SequenceEqual(Out[sourceVertex]);

                predecessors.Push(sourceVertex);
                if (!visited[outVertex]) DepthFirstSearch(outVertex, visited, predecessors, ref isChanged);
                predecessors.Pop();
            }

            visited[currentBlock] = false;
        }
    }
}
