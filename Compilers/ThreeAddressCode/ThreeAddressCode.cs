using System.Collections.Generic;
using System.Text;
using SimpleLang.ThreeAddressCode.TacNodes;

namespace SimpleLang.ThreeAddressCode
{
    public class ThreeAddressCode
    {
        public LinkedList<TacNode> CodeList { get; set; }

        public ThreeAddressCode()
        {
            CodeList = new LinkedList<TacNode>();
        }

        public void PushNode(TacNode node)
        {
            CodeList.AddLast(node);
        }

        public void RemoveNode(TacNode node)
        {
            CodeList.Remove(node);
        }

        public void RemoveNodes(IEnumerable<TacNode> nodes)
        {
            foreach (var tacNode in nodes)
            {
                CodeList.Remove(tacNode);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var tacNode in CodeList)
            {
                builder.Append(tacNode?.ToString());
            }

            return builder.ToString();
        }
    }
}