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
            var threeAddressCode = new ThreeAddressCode();
            foreach (var vertex in vertices.Where(vertex => vertex != entryPoint))
            {
                threeAddressCode.TACodeLines.Concat(vertex.TACodeLines);
            }
            
            var initializationSet = new HashSet<ThreeAddressCode>()
                {threeAddressCode};

            InOut.Out.Add(entryPoint, new HashSet<ThreeAddressCode>() {entryPoint});
            InOut.In[entryPoint] = new HashSet<ThreeAddressCode>();
            foreach (var basicBlock in cfg.SourceBasicBlocks)
            {
                if (basicBlock == entryPoint) continue;
                InOut.Out[basicBlock] = initializationSet;
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
                    InOut.In[curBlock] = _collectionOperator.Collect(InOut.In[prevBlock], InOut.Out[prevBlock]);
                    

                    var tmp = InOut.Out[curBlock];

                    InOut.Out[curBlock] = new HashSet<ThreeAddressCode>(InOut.Out[curBlock].Union(new HashSet<ThreeAddressCode>(){curBlock}));
                    if (!tmp.SequenceEqual(InOut.Out[curBlock]))
                    {
                        outWasChanged = true;
                    }
                }
            }

            Dominators = InOut.Out;
        }
    }
}