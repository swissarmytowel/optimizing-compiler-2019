using SimpleLang.TACode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.CFG.NaturalCycles
{
    public class CFGNaturalCycles
    {
        public HashSet<CFGNaturalCycle> Cycles;
        public Dictionary<CFGNaturalCycle, HashSet<CFGNaturalCycle>> NestedLoops;

        // Естественные циклы
        public CFGNaturalCycles (ControlFlowGraph cfg)
        {
            var edgeClassifierService = new EdgeClassifierService(cfg);
            // Для данного обратного ребра n → d определим естественный цикл ребра как d плюс
            // множество узлов, которые могут достичь n не проходя через d.
            var retreatingEdges = edgeClassifierService.RetreatingEdges;
            Cycles = new HashSet<CFGNaturalCycle>();
            foreach (var edge in retreatingEdges)
            {
                // Запуск алгоритма построения естественного цикла
                var cycle = new CFGNaturalCycle(cfg, edge);
                Cycles.Add(cycle);
            }
            //Console.WriteLine(ToString());
            //Console.WriteLine("MergeLoopsWithCommonEntryBlock");
            // Объединение циклов с общим заголовком в один
            MergeLoopsWithCommonEntryBlock();
            //Console.WriteLine(ToString());
            // Заполнение информации о вложенности циклов
            DefinitionNestingForCycles();
        }

        //Естественные циклы либо не пересекаются либо один из них вложен в другой
        private void DefinitionNestingForCycles()
        {
            NestedLoops = new Dictionary<CFGNaturalCycle, HashSet<CFGNaturalCycle>>();
            foreach (var cycleParent in Cycles) {
                NestedLoops.Add(cycleParent, new HashSet<CFGNaturalCycle>());
                foreach (var cycleChildren in Cycles)
                {
                    if (cycleChildren != cycleParent && cycleParent.IsContainsCycle(cycleChildren))
                        NestedLoops[cycleParent].Add(cycleChildren);
                }
            }
        }

        // Естественные циклы с общим заголовком Если два естественных цикла имеют общий заголовок,
        // то будем считать, что они образуют один естественный цикл
        private void MergeLoopsWithCommonEntryBlock()
        {
            var commonStart = new Dictionary<ThreeAddressCode, HashSet<CFGNaturalCycle>>();
            foreach (var cycle in Cycles)
            {
                var entry = cycle.EntryBlock;
                if (!commonStart.Keys.Contains(entry))
                    commonStart.Add(entry, new HashSet<CFGNaturalCycle>());
                commonStart[entry].Add(cycle);
            }
            foreach (var pair in commonStart)
            {
                var oldCycles = pair.Value;
                if (oldCycles.Count() > 1)
                {
                    var newCycle = new CFGNaturalCycle(oldCycles);
                    foreach (var oldCycle in oldCycles)
                        Cycles.Remove(oldCycle);
                    Cycles.Add(newCycle);
                }
            }
        }

        public string NestedLoopsText()
        {
            string text = "\n";
            foreach (var cycle in Cycles)
            {
                text += "NestedLoops for\n";
                text += cycle.ToString();
                text += "-----------------\n";
                foreach (var nested in NestedLoops[cycle])
                    text += nested.ToString();
                text += "-----------------\n\n";
            }
            return text;
        }

        public override string ToString()
        {
            string text = "Loops Count = " + Cycles.Count() + "\n";
            foreach (var cycle in Cycles)
                text += cycle.ToString();
            return text;
        }
    }
}
