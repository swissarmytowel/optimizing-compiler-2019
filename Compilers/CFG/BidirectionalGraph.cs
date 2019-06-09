using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QuickGraph;
using SimpleLang.TacBasicBlocks;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.CFG
{
    public abstract class BidirectionalGraph
    {
        public IEnumerable<ThreeAddressCode> Vertices => Graph.Vertices;
        public IEnumerable<Edge<ThreeAddressCode>> Edges => Graph.Edges;
        public BasicBlocks SourceBasicBlocks { get; protected set; }
        public int VertexCount => Graph.VertexCount;
        public int EdgeCount => Graph.EdgeCount;
        public bool IsVerticesEmpty => Graph.IsVerticesEmpty;
        public bool IsEdgesEmpty => Graph.IsEdgesEmpty;

        /// <summary>
        /// Returns a vertex at index position
        /// </summary>
        public ThreeAddressCode this[int index] => GetVertexAt(index);

        /// <summary>
        /// Returns a node at nodeIdx position from a vertex at nodeIdx position
        /// </summary>
        public TacNode this[int vertexIdx, int nodeIdx] => GetVertexAt(vertexIdx).ElementAt(nodeIdx);

        protected BidirectionalGraph<ThreeAddressCode, Edge<ThreeAddressCode>> Graph;

        protected BidirectionalGraph()
        {
            SourceBasicBlocks = new BasicBlocks();
            Graph = new BidirectionalGraph<ThreeAddressCode, Edge<ThreeAddressCode>>(false);

        }

        public void SaveToFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var directoryName = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            File.WriteAllText(fileName, ToString());
        }

        public override string ToString()
        {
            if (IsVerticesEmpty)
                return "Empty graph.";

            var currentIndex = 0;
            var indices = Vertices.ToDictionary(vertex => vertex, _ => currentIndex++);

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("VERTICES");
            stringBuilder.Append(Vertices
                .Select((tac, idx) => $"#{idx}:\n{tac}\n")
                .Aggregate("", (acc, cur) => acc + cur));

            stringBuilder.AppendLine("EDGES");
            foreach (var vertex in Vertices)
            {
                var targetVertices = OutEdges(vertex).Select(x => indices[x.Target]);
                stringBuilder.AppendLine(
                    $"{indices[vertex]} -> [{targetVertices.Aggregate(" ", (acc, cur) => " " + cur + acc)}]");
            }

            return stringBuilder.ToString();
        }

        private ThreeAddressCode GetVertexAt(int index)
        {
            if (index < 0 || index > VertexCount - 1)
                return null;

            return SourceBasicBlocks.BasicBlockItems[index];
        }

        #region Methods from QuickGraph

        public IEnumerable<Edge<ThreeAddressCode>> OutEdges(ThreeAddressCode vertex)
            => Graph.OutEdges(vertex);

        public IEnumerable<Edge<ThreeAddressCode>> InEdges(ThreeAddressCode vertex)
            => Graph.InEdges(vertex);

        public bool IsOutEdgesEmpty(ThreeAddressCode vertex)
            => Graph.IsOutEdgesEmpty(vertex);

        public bool IsInEdgesEmpty(ThreeAddressCode vertex)
            => Graph.IsInEdgesEmpty(vertex);

        public int OutDegree(ThreeAddressCode vertex)
            => Graph.OutDegree(vertex);

        public int InDegree(ThreeAddressCode vertex)
            => Graph.InDegree(vertex);

        public bool ContainsVertex(ThreeAddressCode vertex)
            => Graph.ContainsVertex(vertex);

        public bool ContainsEdge(Edge<ThreeAddressCode> edge)
            => Graph.ContainsEdge(edge);

        #endregion
    }
}
