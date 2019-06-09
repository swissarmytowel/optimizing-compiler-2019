using QuickGraph;
using SimpleLang.TACode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.CFG.NaturalCycles
{
    public class CFGNaturalCycle: BidirectionalGraph
    {
        public ThreeAddressCode EntryBlock;
        public HashSet<ThreeAddressCode> ExitBlocks = new HashSet<ThreeAddressCode>();

        public bool IsContainsCycle(CFGNaturalCycle cycle)
        {
            if (IsVerticesEmpty || cycle.IsVerticesEmpty)
                return false;
            if (Vertices.Contains(cycle.EntryBlock))
            {
                bool res = true;
                foreach (var exit in cycle.ExitBlocks)
                    if (!Vertices.Contains(exit))
                        res = false; 
                return res;
            } else return false;
        }

        // Генерация естественного цикла слиянием нескольких циклов
        public CFGNaturalCycle(IEnumerable<CFGNaturalCycle> cycles)
        {
            var sumVertices = new HashSet<ThreeAddressCode>();
            var sumEdges = new HashSet<Edge<ThreeAddressCode>>();
            foreach (var cycle in cycles)
            {
                foreach(var v in cycle.Vertices)
                    sumVertices.Add(v);
                foreach (var e in cycle.Edges)
                    sumEdges.Add(e);
                foreach (var exit in cycle.ExitBlocks)
                    ExitBlocks.Add(exit);
                EntryBlock = cycle.EntryBlock;
            }
            Graph.AddVertexRange(sumVertices);
            Graph.AddEdgeRange(sumEdges);
        }

        //Алгоритм построения естественного цикла обратного ребра n → d.
        public CFGNaturalCycle(ControlFlowGraph cfg, Edge<ThreeAddressCode> loopEdge)
        {
            //loopSet ≔ {n, d}
            var visitedVertex = new HashSet<ThreeAddressCode>();
            // Пометим d как посещённый
            visitedVertex.Add(loopEdge.Target);
            EntryBlock = loopEdge.Target;
            Graph.AddVertex(EntryBlock);
            var targetVertex = new Queue<ThreeAddressCode>();
            targetVertex.Enqueue(loopEdge.Source);
            ExitBlocks.Add(loopEdge.Source);
            Graph.AddVertex(loopEdge.Source);
            Graph.AddEdge(new Edge<ThreeAddressCode>(loopEdge.Source, EntryBlock));
            //Выполним поиск в глубину на обратном графе потока, начиная с n. Внесём все
            //посещённые узлы в loopSet.
            while (targetVertex.Count() > 0)
            {
                var curVertex = targetVertex.Dequeue();
                var containVertex = cfg.ContainsVertex(curVertex);
                // Рассмотрим обратный граф потока управления (все стрелочки – в обратную сторону)
                var incomingEdges = containVertex ? cfg.InEdges(curVertex) : new List<Edge<ThreeAddressCode>>();
                foreach (var edge in incomingEdges)
                {
                    if (!visitedVertex.Contains(edge.Source))
                        targetVertex.Enqueue(edge.Source);
                    if (!Graph.Vertices.Contains(edge.Source))
                        Graph.AddVertex(edge.Source);
                    Graph.AddEdge(new Edge<ThreeAddressCode>(edge.Source, edge.Target));
                }
                visitedVertex.Add(curVertex);
            }
        }
    }
}
