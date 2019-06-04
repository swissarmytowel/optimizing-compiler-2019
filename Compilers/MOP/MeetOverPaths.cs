using System;
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
                // Здесь нужно применить оператор сбора ко всей последовательности
                // ну и посчитать пердаточную функцию от полученного выше множества
                //var _out = Out[sourceVertex];
                //var tmp2 = TransmissionFunction.Calculate(_out, outVertex);

                predecessors.Push(sourceVertex);
                if (!visited[outVertex]) DepthFirstSearch(outVertex, visited, predecessors, ref isChanged);
                predecessors.Pop();
            }

            if (currentBlock == ControlFlowGraph.EntryBlock)
            {
                // Здесь проверить из менялся ли Out
            }

            visited[currentBlock] = false;
            isChanged = false;
        }
    }
}
