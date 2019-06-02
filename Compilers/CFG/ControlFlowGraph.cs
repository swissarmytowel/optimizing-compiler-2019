using System.Linq;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using QuickGraph;

namespace SimpleLang.CFG
{
    public class ControlFlowGraph : BidirectionalGraph
    {
        public ThreeAddressCode EntryBlock => IsVerticesEmpty ? null : Vertices.First();
        public ThreeAddressCode ExitBlock => IsVerticesEmpty ? null : Vertices.Last();
        public ThreeAddressCode SourceCode { get; private set; }
        

        public ControlFlowGraph(ThreeAddressCode tac)
        {
            SourceCode = tac;

            if (SourceCode == null || SourceCode.TACodeLines.Count == 0)
                return;

            Build();
        }

        public DepthSpanningTree GetDepthSpanningTree()
            => new DepthSpanningTree(this);

        private void Rebuild(ThreeAddressCode tac)
        {
            SourceCode = tac;

            if (SourceCode == null || SourceCode.TACodeLines.Count == 0)
                return;

            Build();
        }

        private void Build()
        {
            var blocks = new BasicBlocks();
            blocks.SplitTACode(SourceCode);

            Graph.AddVertexRange(blocks.BasicBlockItems);

            var blocksCount = blocks.BasicBlockItems.Count;
            for (var i = 0; i < blocksCount; ++i)
            {
                var currentBlock = blocks.BasicBlockItems[i];

                if (currentBlock.Last() is TacGotoNode gotoNode)
                {
                    var targetBlock = blocks.BasicBlockItems.Find(
                        x => x.First().Label == gotoNode.TargetLabel);
                    Graph.AddEdge(new Edge<ThreeAddressCode>(currentBlock, targetBlock));

                    if (!(currentBlock.Last() is TacIfGotoNode))
                        continue;
                }

                if (i < blocksCount - 1)
                {
                    var nextBlock = blocks.BasicBlockItems[i + 1];
                    Graph.AddEdge(new Edge<ThreeAddressCode>(currentBlock, nextBlock));
                }
            }
        }
    }
}
