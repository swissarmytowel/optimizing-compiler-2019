using QuickGraph;
using SimpleLang.CFG.DominatorsTree;
using SimpleLang.TACode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.CFG
{
    public class EdgeClassifierService
    {
        private ControlFlowGraph _cfg;
        private DstEdgeClassifier edgeClassifier;
        private DominatorsService dominatorService;
        public HashSet<Edge<ThreeAddressCode>> BackEdges;
        public HashSet<Edge<ThreeAddressCode>> ComingEdges;
        public HashSet<Edge<ThreeAddressCode>> RetreatingEdges;
        public HashSet<Edge<ThreeAddressCode>> CrossEdges;

        public EdgeClassifierService(ControlFlowGraph cfg)
        {
            _cfg = cfg;
            edgeClassifier = new DstEdgeClassifier(cfg);
            edgeClassifier.ClassificateEdges(cfg);
            dominatorService = new DominatorsService(cfg);
            GenerateClassifyEdges();
        }

        private void GenerateClassifyEdges()
        {
            BackEdges = new HashSet<Edge<ThreeAddressCode>>();
            ComingEdges = new HashSet<Edge<ThreeAddressCode>>();
            RetreatingEdges = new HashSet<Edge<ThreeAddressCode>>();
            CrossEdges = new HashSet<Edge<ThreeAddressCode>>();
            foreach (var edgeType in edgeClassifier.EdgeTypes)
            {
                Edge<ThreeAddressCode> edge = edgeType.Key;
                if (edgeType.Value == EdgeType.Coming)
                {
                    ComingEdges.Add(edge);
                }
                if (edgeType.Value == EdgeType.Retreating)
                {
                    RetreatingEdges.Add(edge);
                    if (dominatorService.IsVertexDominator(edge.Target, edge.Source))
                    {
                        BackEdges.Add(edge);
                    }
                }
                if (edgeType.Value == EdgeType.Cross)
                {
                    CrossEdges.Add(edge);
                }
            }
        }

        private string BlocksToString()
        {
            string text = "";
            int blockI = 0;
            foreach (var block in _cfg.SourceBasicBlocks.BasicBlockItems)
                text += "\nBLOCK" + blockI++ + ":\n" + block;
            return text;
        }

        private string EdgesToString(HashSet<Edge<ThreeAddressCode>> edges)
        {
            string text = "";
            foreach (var edge in edges)
            {
                int from = _cfg.SourceBasicBlocks.BasicBlockItems.FindIndex(e => e == edge.Source);
                int to = _cfg.SourceBasicBlocks.BasicBlockItems.FindIndex(e => e == edge.Target);
                text += "\nEDGE: " + $"block{from} -> block{to}";
            }
            return text;
        }

        public override string ToString()
        {
            string text = "\nBasicBlocks: \n" + BlocksToString();
            //text += "\nDominatorService:" + dominatorService + "\n";
            text += "\nBackEdges:" + EdgesToString(BackEdges) + "\n";
            text += "\nComingEdges:" + EdgesToString(ComingEdges) + "\n";
            text += "\nRetreatingEdges:" + EdgesToString(RetreatingEdges) + "\n";
            text += "\nCrossEdges:" + EdgesToString(CrossEdges) + "\n";
            return text;
        } 

    }
}
