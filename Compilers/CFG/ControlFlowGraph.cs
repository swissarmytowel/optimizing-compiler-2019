using System.Linq;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using QuickGraph;
using QuickGraph.Graphviz;

namespace SimpleLang.CFG
{
    public class ControlFlowGraph : BidirectionalGraph<ThreeAddressCode, Edge<ThreeAddressCode>>
    {
        public BasicBlocks Blocks { get; private set; }

        public ControlFlowGraph() : base(false) { }

        public void BuildFrom(ThreeAddressCode tac)
        {
            Blocks = new BasicBlocks();
            Blocks.SplitTACode(tac);
            Build();
        }

        public void BuildFrom(BasicBlocks blocks)
        {
            Blocks = blocks;
            Build();
        }

        private void Build()
        {
            Clear();

            AddVertexRange(Blocks.BasicBlockItems);

            var blocksCount = Blocks.BasicBlockItems.Count;
            for (var i = 0; i < blocksCount; ++i)
            {
                var currentBlock = Blocks.BasicBlockItems[i];

                if (currentBlock.Last() is TacGotoNode gotoNode)
                {
                    var targetBlock = Blocks.BasicBlockItems.Find(
                        x => x.First().Label == gotoNode.TargetLabel);
                    AddEdge(new Edge<ThreeAddressCode>(currentBlock, targetBlock));

                    if (!(currentBlock.Last() is TacIfGotoNode))
                        continue;
                }

                if (i < blocksCount - 1)
                {
                    var nextBlock = Blocks.BasicBlockItems[i + 1];
                    AddEdge(new Edge<ThreeAddressCode>(currentBlock, nextBlock));
                }
            }
        }

        public override string ToString()
        {
            var viz = new GraphvizAlgorithm<ThreeAddressCode, Edge<ThreeAddressCode>>(this);
            viz.FormatVertex += VizFormatVertex;
            return viz.Generate(new DotPrinter(), "");
        }

        private static void VizFormatVertex<TVertex>(object sender, FormatVertexEventArgs<TVertex> e)
        {
            e.VertexFormatter.Label = e.Vertex.ToString();
        }
    }
}
