using System.IO;
using System.Linq;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using QuickGraph;
using QuickGraph.Graphviz;
using SimpleLang.CFG.Visualization;

namespace SimpleLang.CFG
{
    public class ControlFlowGraph : BidirectionalGraph<ThreeAddressCode, Edge<ThreeAddressCode>>
    {
        public BasicBlocks Blocks { get; private set; }

        public ControlFlowGraph() : base(false) { }

        public void Construct(ThreeAddressCode tac)
        {
            Blocks = new BasicBlocks();
            Blocks.SplitTACode(tac);
            Construct();
        }

        public void Construct(BasicBlocks blocks)
        {
            Blocks = blocks;
            Construct();
        }

        private void Construct()
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

        public void SaveToFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var directoryName = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            
            File.WriteAllText(fileName, ToString());
        }

        public override string ToString()
        {
            var graphviz = new GraphvizAlgorithm<ThreeAddressCode, Edge<ThreeAddressCode>>(this);
            graphviz.FormatVertex += VizFormatVertex;
            return graphviz.Generate(new DotPrinter(), "");
        }

        private static void VizFormatVertex<TVertex>(object sender, FormatVertexEventArgs<TVertex> e)
        {
            e.VertexFormatter.Label = $"Basic block:\n{e.Vertex}";
        }
    }
}
