using System.Collections.Generic;
using QuickGraph;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using System.Linq;

namespace SimpleLang.CFG
{
    public enum EdgeType
    {
        Coming = 1,
        Retreating = 2,
        Cross = 3
    }
    
    public class DstEdgeClassifier
    {
        public Dictionary<Edge<ThreeAddressCode>, EdgeType> EdgeTypes { get; set; }
        public DepthSpanningTree DSTree { get; set; }

        public DstEdgeClassifier(ControlFlowGraph cfg)
        {
            DSTree = new DepthSpanningTree(cfg);
            EdgeTypes = new Dictionary<Edge<ThreeAddressCode>, EdgeType>();
        }
        
        public void ClassificateEdges(ControlFlowGraph cfg)
        {
            DSTree = new DepthSpanningTree(cfg);
            
            foreach (var edge in cfg.Edges)
            {
                if (DSTree.Edges.Any(e => e.Target.Equals(edge.Target) && e.Source.Equals(edge.Source)))
                {
                    EdgeTypes.Add(edge, EdgeType.Coming);
                }
                else if (FindBackwardPath( edge.Source, edge.Target))
                {
                    EdgeTypes.Add(edge, EdgeType.Retreating);
                }
                else
                {
                    EdgeTypes.Add(edge, EdgeType.Cross);
                }
            }
        }

        private bool FindBackwardPath(ThreeAddressCode source, ThreeAddressCode target)
        {
            var result = false;
            var containVertex = DSTree.ContainsVertex(source);
            var incomingEdges = containVertex ? DSTree.InEdges(source) : new List<Edge<ThreeAddressCode>>();
            while (incomingEdges.Any())
            {
                var edge = incomingEdges.First();
                if (edge.Source.Equals(target))
                {
                    result = true;
                    break;
                }

                incomingEdges = DSTree.InEdges(edge.Source);
            }

            return result;
        }
        
        public override string ToString()
        {
            return string.Join("\n", EdgeTypes.Select(ed => $"[{ed.Key.Source.ToString()} -> {ed.Key.Target.ToString()}]: {ed.Value}"));
        }
    }
}