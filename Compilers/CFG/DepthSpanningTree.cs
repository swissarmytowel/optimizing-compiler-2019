using System.Collections.Generic;
using QuickGraph;
using SimpleLang.TacBasicBlocks;
using SimpleLang.TACode;

namespace SimpleLang.CFG
{
    public class DepthSpanningTree : BidirectionalGraph
    {
        public BasicBlocks SortedBasicBlocks { get; }

        private HashSet<ThreeAddressCode> _visitedBlocks;
        private ControlFlowGraph _cfg;

        public DepthSpanningTree(ControlFlowGraph cfg)
        {
            SortedBasicBlocks = new BasicBlocks();
            _cfg = cfg;
            _visitedBlocks = new HashSet<ThreeAddressCode>();

            if (_cfg == null ||_cfg.IsVerticesEmpty)
                return;

            Build(_cfg.EntryBlock);
            FillBasicBlocks();
        }

        public void Rebuild(ControlFlowGraph cfg)
        {
            SortedBasicBlocks.BasicBlockItems.Clear();
            Graph.Clear();
            _cfg = cfg;
            _visitedBlocks.Clear();

            if (_cfg == null || _cfg.IsVerticesEmpty)
                return;
            
            Build(_cfg.EntryBlock);
            FillBasicBlocks();
        }

        private void FillBasicBlocks()
        {
            foreach (var vertex in Vertices)
                SortedBasicBlocks.BasicBlockItems.Add(vertex);
        }

        private void Build(ThreeAddressCode block)
        {
            if (block == null)
                return;

            _visitedBlocks.Add(block);
            var outEdges = _cfg.OutEdges(block);
            foreach (var edge in outEdges)
            {
                var targetBlock = edge.Target;

                if (_visitedBlocks.Contains(targetBlock))
                    continue;

                if (!ContainsVertex(block))
                    Graph.AddVertex(block);

                if (!ContainsVertex(targetBlock))
                    Graph.AddVertex(targetBlock);

                Graph.AddEdge(new Edge<ThreeAddressCode>(block, targetBlock));
                Build(targetBlock);
            }
        }
    }
}
