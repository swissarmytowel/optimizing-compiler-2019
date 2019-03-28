using System.Collections.Generic;
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
    }
}