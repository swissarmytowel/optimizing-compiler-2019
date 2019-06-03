using System;
using System.Collections.Generic;
using QuickGraph;

using SimpleLang.CFG;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.IterationAlgorithms.Interfaces;

namespace SimpleLang.MOP
{
    public class MeetOverPaths<T> : IIterationAlgorithm<T>
    {
        public ControlFlowGraph ControlFlowGraph { get; }
        public Dictionary<ThreeAddressCode, HashSet<T>> In { get; set; }
        public Dictionary<ThreeAddressCode, HashSet<T>> Out { get; set; }

        public MeetOverPaths(ControlFlowGraph controlFlowGraph)
        {
            ControlFlowGraph = controlFlowGraph;
            In = new Dictionary<ThreeAddressCode, HashSet<T>>();
            Out = new Dictionary<ThreeAddressCode, HashSet<T>>();
        }

        public void Compute()
        {
            Dictionary<ThreeAddressCode, bool> visited = new Dictionary<ThreeAddressCode, bool>();
            foreach (var bblock in ControlFlowGraph.SourseBasicBlocks)
                visited.Add(bblock, false);

            List<ThreeAddressCode> predecessors = new List<ThreeAddressCode>();
            var isChanged = true;

            while (isChanged)
                DepthFirstSearch(ControlFlowGraph.EntryBlock, visited, predecessors, ref isChanged);

        }

        private void DepthFirstSearch(
            ThreeAddressCode currentBlock,
            Dictionary<ThreeAddressCode, bool> visited,
            List<ThreeAddressCode> predecessors,
            ref bool isChanged
            )
        {
            if (ControlFlowGraph.IsOutEdgesEmpty(currentBlock)) return;
            visited[currentBlock] = true;

            foreach(var outEdge in ControlFlowGraph.OutEdges(currentBlock))
            {
                var outVertex = outEdge.Target;
                Console.WriteLine(outVertex);
                if (!visited[outVertex]) DepthFirstSearch(outVertex, visited, predecessors, ref isChanged);
            }

            visited[currentBlock] = false;
            isChanged = false;
        }
    }
}
