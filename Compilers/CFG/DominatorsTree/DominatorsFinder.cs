using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using QuickGraph;
using SimpleLang.GenKill.Implementations;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.InOut;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.IterationAlgorithms;
using SimpleLang.IterationAlgorithms.CollectionOperators;
using SimpleLang.IterationAlgorithms.Interfaces;
using SimpleLang.TacBasicBlocks;
using SimpleLang.Utility;
using System.Text;

namespace SimpleLang.CFG.DominatorsTree
{
//    public class DominatorsFinder : IterationAlgorithm<TacNode>
//    {
//        public Dictionary<ThreeAddressCode, HashSet<TacNode>> Dominators;
//
//        public DominatorsFinder(ControlFlowGraph cfg, Dictionary<ThreeAddressCode, IExpressionSetsContainer> lines)
//            : base(cfg, new TFByComposition(lines), new IntersectCollectionOperator<TacNode>(), true)
//        {
//            Execute();
//            Dominators = InOut.Out;
//        }
//    }
    public class DominatorsFinder : IIterationAlgorithm<ThreeAddressCode>
    {
        public InOutContainer<ThreeAddressCode> InOut { get; set; } = new InOutContainer<ThreeAddressCode>();

        public Dictionary<ThreeAddressCode, HashSet<ThreeAddressCode>> Dominators =
            new Dictionary<ThreeAddressCode, HashSet<ThreeAddressCode>>();

        private ICollectionOperator<ThreeAddressCode> _collectionOperator =
            new IntersectCollectionOperator<ThreeAddressCode>();

        public DominatorsFinder(ControlFlowGraph cfg)
        {
            var entryPoint = cfg.Vertices.FirstOrDefault(e => cfg.InDegree(e) == 0);
            if (entryPoint == null) return;

            var vertices = cfg.Vertices.ToList();
            var union = new UnionCollectionOperator<ThreeAddressCode>();
            //var threeAddressCode = new List<ThreeAddressCode>();
            var threeAddressCodeHashSet = new HashSet<ThreeAddressCode>();
            foreach (var vertex in vertices.Where(vertex => vertex != entryPoint)) {
                //threeAddressCode.Add(vertex);
                threeAddressCodeHashSet.Add(vertex);
            }
            
            InOut.Out.Add(entryPoint, new HashSet<ThreeAddressCode>() {entryPoint});
            InOut.In[entryPoint] = new HashSet<ThreeAddressCode>();
            //int k = 0;
            foreach (var basicBlock in cfg.SourceBasicBlocks)
            {
                if (basicBlock == entryPoint) 
                {
                    continue;
                }
                InOut.Out[basicBlock] = threeAddressCodeHashSet; //new HashSet<ThreeAddressCode>() { threeAddressCode[k++] }; 
            }

            var outWasChanged = true;

            while (outWasChanged)
            {
                outWasChanged = false;
                for (var i = 1; i < vertices.Count; ++i)
                {
                    var curBlock = vertices[i];

                    var prevBlock = vertices[i - 1];
                    InOut.In[curBlock] = new HashSet<ThreeAddressCode>();

                    if (prevBlock == entryPoint) {
                        InOut.In[curBlock] = InOut.Out[prevBlock];
                    } else {
                        InOut.In[curBlock] = _collectionOperator.Collect(InOut.In[prevBlock], InOut.Out[prevBlock]);
                    }

                    var builder1 = new StringBuilder();
                    foreach (var entry in InOut.Out[curBlock]) {
                        builder1.Append(entry);
                    }

                    InOut.Out[curBlock] = new HashSet<ThreeAddressCode>(InOut.In[curBlock].Union(new HashSet<ThreeAddressCode>(){curBlock}));

                    var builder2 = new StringBuilder();
                    foreach (var entry in InOut.Out[curBlock]) {
                        builder2.Append(entry);
                    }

                    if (!builder1.Equals(builder2)) {
                        outWasChanged = true;
                    }
                }
            }
            Dominators = InOut.Out;
        }
    }
}