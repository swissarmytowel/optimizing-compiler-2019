using SimpleLang.Optimizations;
using SimpleLang.TACode;
using QuickGraph;
using QuickGraph.Graphviz;

namespace SimpleLang.CFG
{
    public class ControlFlowGraph
    {
        private BasicBlocks _blocks;
        private AdjacencyGraph<ThreeAddressCode, Edge<ThreeAddressCode>> _graph;
        public BasicBlocks Blocks => _blocks;
        public AdjacencyGraph<ThreeAddressCode, Edge<ThreeAddressCode>> Graph => _graph;

        public ControlFlowGraph()
        {
            _graph = new AdjacencyGraph<ThreeAddressCode, Edge<ThreeAddressCode>>();
        }

        public void BuildFrom(ThreeAddressCode tac)
        {
            _graph.Clear();
            _blocks = new BasicBlocks();
            _blocks.SplitTACode(tac);

            Build();
        }

        public void BuildFrom(BasicBlocks blocks)
        {
            _graph.Clear();
            _blocks = blocks;

            Build();
        }

        private void Build()
        {
            
        }

        public override string ToString()
        {
            var viz = new GraphvizAlgorithm<ThreeAddressCode, Edge<ThreeAddressCode>>(_graph);
            viz.FormatVertex += VizFormatVertex;
            return viz.Generate(new DotPrinter(), "");
        }

        private static void VizFormatVertex<TVertex>(object sender, FormatVertexEventArgs<TVertex> e)
        {
            e.VertexFormatter.Label = e.Vertex.ToString();
        }
    }
}
