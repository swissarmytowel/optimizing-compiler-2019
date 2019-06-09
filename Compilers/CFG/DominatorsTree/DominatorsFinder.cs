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
        /// Ключ - блок
        /// Значение - Все доминаторы текущего блока
        /// </summary>
        public Dictionary<ThreeAddressCode, HashSet<ThreeAddressCode>> Dominators =
            new Dictionary<ThreeAddressCode, HashSet<ThreeAddressCode>>();

        /// <summary>
        /// Непосредственные доминаторы
        /// Ключ - блок
        /// Значение - непосредственный доминатор
        /// </summary>
        public Dictionary<ThreeAddressCode, ThreeAddressCode> ImmediateDominators =
            new Dictionary<ThreeAddressCode, ThreeAddressCode>();

        public DominatorsFinder(ControlFlowGraph cfg)
        {
            // Входной узел программы
            var entryPoint = cfg.Vertices.FirstOrDefault(e => cfg.InDegree(e) == 0);
            if (entryPoint == null) return;

            var vertices = cfg.Vertices.ToList();
            var threeAddressCodeHashSet = new HashSet<ThreeAddressCode>();

            // Добавляем все базовые блоки, кроме входного узла
            foreach (var vertex in vertices.Where(vertex => vertex != entryPoint)) {
                threeAddressCodeHashSet.Add(vertex);
            }

            // Для входного узла In - пустой, Out - сам базовый блок
            InOut.In[entryPoint] = new HashSet<ThreeAddressCode>();
            InOut.Out.Add(entryPoint, new HashSet<ThreeAddressCode>() {entryPoint});
            ImmediateDominators[entryPoint] = entryPoint;

            // Для всех ББл, кроме входного, Out явл. множеством всех ББл, кроме входного
            foreach (var basicBlock in cfg.SourceBasicBlocks)
            {
                if (basicBlock == entryPoint) 
                {
                    continue;
                }
                InOut.Out[basicBlock] = threeAddressCodeHashSet; 
            }

            var outWasChanged = true;

            // Цикл: пока вносятся изменения в Out хотя бы одного базового блока
            while (outWasChanged)
            {
                outWasChanged = false;
                for (var i = 1; i < vertices.Count; ++i)
                {
                    var curBlock = vertices[i];

                    // Предки текущего узла
                    var ancestors = cfg.Edges.Where(edge => edge.Target == curBlock).Select(e => e.Source).ToList();

#region All Dominators 
                    InOut.In[curBlock] = new HashSet<ThreeAddressCode>();
                    InOut.In[curBlock] = InOut.Out[ancestors[0]];

                    // Если несколько непосредственных предков
                    if (ancestors.Count > 1) { 
                        for (int ind = 1; ind < ancestors.Count; ++ind) {
                            if (curBlock == ancestors[ind]) continue;
                            var tmpIntersection = new HashSet<ThreeAddressCode>(InOut.In[curBlock]);
                            tmpIntersection.IntersectWith(InOut.Out[ancestors[ind]]);
                            InOut.In[curBlock] = tmpIntersection;
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
                        // Находим непосредственных доминаоров, т.е. доминаторов не являющихся:
                        // 1) рассматриваемым узлом;
                        // 2) доминатором над каким-либо из доминаторов данного узла
                        var immediateDomsCurBlock = InOut.Out[curBlock].Where(block => block != curBlock).Select(p => p).ToList();
                        var needToDeleteElems = new List<ThreeAddressCode>();
                        foreach (var dom in immediateDomsCurBlock) {
                            if (immediateDomsCurBlock.Contains(ImmediateDominators[dom]) && ImmediateDominators[dom] != dom) {
                                needToDeleteElems.Add(ImmediateDominators[dom]);
                            }
                        }
                        needToDeleteElems.Distinct();
                        ImmediateDominators[curBlock] = immediateDomsCurBlock
                            .Where(d => !needToDeleteElems.Contains(d))
                            .FirstOrDefault();
#endregion
                    }
                }
            }
            ImmediateDominators[entryPoint] = null;
            Dominators = InOut.Out;
        }
    }
}