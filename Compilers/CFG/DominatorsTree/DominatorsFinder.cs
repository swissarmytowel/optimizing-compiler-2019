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
    public class DominatorsFinder : IIterationAlgorithm<ThreeAddressCode>
    {
        public InOutContainer<ThreeAddressCode> InOut { get; set; } = new InOutContainer<ThreeAddressCode>();

        /// <summary>
        /// Все доминаторы
        /// </summary>
        public Dictionary<ThreeAddressCode, HashSet<ThreeAddressCode>> Dominators =
            new Dictionary<ThreeAddressCode, HashSet<ThreeAddressCode>>();

        /// <summary>
        /// Непосредственные доминаторы
        /// </summary>
        public Dictionary<ThreeAddressCode, List<ThreeAddressCode>> ImmediateDominators =
            new Dictionary<ThreeAddressCode, List<ThreeAddressCode>>();

        private ICollectionOperator<ThreeAddressCode> _collectionOperator =
            new IntersectCollectionOperator<ThreeAddressCode>();

        public DominatorsFinder(ControlFlowGraph cfg)
        {
            var entryPoint = cfg.Vertices.FirstOrDefault(e => cfg.InDegree(e) == 0);
            if (entryPoint == null) return;

            var vertices = cfg.Vertices.ToList();
            var union = new UnionCollectionOperator<ThreeAddressCode>();
            var threeAddressCodeHashSet = new HashSet<ThreeAddressCode>();
            foreach (var vertex in vertices.Where(vertex => vertex != entryPoint)) {
                threeAddressCodeHashSet.Add(vertex);
            }
            
            InOut.Out.Add(entryPoint, new HashSet<ThreeAddressCode>() {entryPoint});
            ImmediateDominators[entryPoint] = new List<ThreeAddressCode>(){ entryPoint };
            InOut.In[entryPoint] = new HashSet<ThreeAddressCode>();
            foreach (var basicBlock in cfg.SourceBasicBlocks)
            {
                if (basicBlock == entryPoint) 
                {
                    continue;
                }
                InOut.Out[basicBlock] = threeAddressCodeHashSet; 
            }

            var outWasChanged = true;

            while (outWasChanged)
            {
                outWasChanged = false;
                for (var i = 1; i < vertices.Count; ++i)
                {
                    var curBlock = vertices[i];
                    var ancestors = cfg.Edges.Where(edge => edge.Target == curBlock).Select(e => e.Source).ToList();

#region All Dominators 
                    InOut.In[curBlock] = new HashSet<ThreeAddressCode>();
                    InOut.In[curBlock] = InOut.Out[ancestors[0]];

                    // Если несколько непосредственных предков
                    if (ancestors.Count > 1) { 
                        for (int ind = 1; ind < ancestors.Count; ++ind) {
                            if (curBlock == ancestors[ind]) continue;
                            InOut.In[curBlock] = _collectionOperator.Collect(InOut.In[curBlock], InOut.Out[ancestors[ind]]);
                        }
                    }

                    var builder1 = new StringBuilder();
                    foreach (var entry in InOut.Out[curBlock]) {
                        builder1.Append(entry);
                    }

                    InOut.Out[curBlock] = new HashSet<ThreeAddressCode>(InOut.In[curBlock].Union(new HashSet<ThreeAddressCode>(){curBlock}));
#endregion
                    var builder2 = new StringBuilder();
                    foreach (var entry in InOut.Out[curBlock]) {
                        builder2.Append(entry);
                    }

                    if (!builder1.Equals(builder2)) {
                        outWasChanged = true;

#region Immediate Dominators
                        ImmediateDominators[curBlock] = InOut.Out[curBlock].Where(block => block != curBlock).Select(p => p).ToList();

                        var needToDeleteElems = new List<ThreeAddressCode>();
                        foreach (var dom in ImmediateDominators[curBlock]) {
                            var itsDoms = ImmediateDominators[dom];
                            var needToDelete = itsDoms.Where(d => ImmediateDominators[curBlock].Contains(d) && d != dom).Select(p => p);
                            needToDeleteElems.AddRange(needToDelete);
                        }
                        needToDeleteElems.Distinct();
                        foreach(var elem in needToDeleteElems) {
                            ImmediateDominators[curBlock].Remove(elem);
                        }
#endregion
                    }
                }
            }
            Dominators = InOut.Out;
        }
    }
}