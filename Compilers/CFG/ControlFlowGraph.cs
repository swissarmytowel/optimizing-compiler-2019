using System.IO;
using System.Linq;
using System.Text;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using QuickGraph;

namespace SimpleLang.CFG
{
    public class ControlFlowGraph : BidirectionalGraph<ThreeAddressCode, Edge<ThreeAddressCode>>
    {
        public BasicBlocks Blocks { get; private set; }

        public ControlFlowGraph() : base(false) { }

        public ThreeAddressCode EntryBlock => Blocks.BasicBlockItems.First();

        public ThreeAddressCode ExitBlock => Blocks.BasicBlockItems.Last();

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
            if (IsVerticesEmpty)
                return "Empty CFG.";

            var currentIndex = 0;
            var indices = Vertices.ToDictionary(vertex => vertex, _ => currentIndex++);
            
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("VERTICES");
            stringBuilder.Append(Vertices
                .Select((tac, idx) => $"#{idx}:\n{tac}\n")
                .Aggregate("", (acc, cur) => acc + cur));

            stringBuilder.AppendLine("EDGES");
            foreach (var vertex in Vertices)
            {
                var targetVertices = OutEdges(vertex).Select(x => indices[x.Target]);
                stringBuilder.AppendLine(
                    $"{indices[vertex]} -> [{targetVertices.Aggregate(" ", (acc, cur) => " " + cur + acc)}]");
            }

            return stringBuilder.ToString();
        }
    }
}
