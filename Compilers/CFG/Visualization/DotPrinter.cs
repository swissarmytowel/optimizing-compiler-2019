using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace SimpleLang.CFG.Visualization
{
    public sealed class DotPrinter : IDotEngine
    {
        public string Run(GraphvizImageType imageType, string dot, string outputFileName) 
            => dot;
    }
}
