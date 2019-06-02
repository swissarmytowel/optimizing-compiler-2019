using System;
using System.Collections.Generic;
using QuickGraph;
using QuickGraph.Algorithms.Search;

using SimpleLang.CFG;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.MOP
{
    public class MeetOverPaths
    {
        public ControlFlowGraph ControlFlowGraph { get; }
        public DepthFirstSearchAlgorithm<ThreeAddressCode, Edge<ThreeAddressCode>> DepthFirstSearch { get; }

        public MeetOverPaths(ControlFlowGraph controlFlow)
        {
            ControlFlowGraph = controlFlow;
            DepthFirstSearch = new DepthFirstSearchAlgorithm<ThreeAddressCode, Edge<ThreeAddressCode>>(controlFlow.AdjancyMatrix);
        }

        public HashSet<TacNode> ComputeIn(ThreeAddressCode bbloc)
        {
            throw new Exception();
        }

        private void treeEdge(Object sender, EdgeEventArgs<ThreeAddressCode, Edge<ThreeAddressCode>> e)
        {

        }

        //private void DepthFirstSearch(
        //    ControlFlowGraph controlFlow,
        //    ThreeAddressCode currentBlock,
        //    ThreeAddressCode stopBlock,
        //    ref Dictionary<ThreeAddressCode, bool> visited,
        //    ref HashSet<string> _in
        //    )
        //{
        //    if (currentBlock == stopBlock) return;
        //    visited[currentBlock] = true;

        //    throw new Exception();
        //}

    }
}
