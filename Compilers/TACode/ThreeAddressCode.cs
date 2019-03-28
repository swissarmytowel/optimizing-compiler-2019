using System.Collections.Generic;
using System.Text;
using ProgramTree;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.TACode
{
    public class ThreeAddressCode
    {
        public LinkedList<TacNode> CodeList { get; }

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

        #region Convenience methods

        public void CreateAndPushBoolNode(BoolNode node, string tmpName)
        {
            PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPart = tmpName,
                FirstOperand = node.Value.ToString()
            });
        }
        
        public void CreateAndPushIdNode(IdNode node, string tmpName)
        {
            PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPart = tmpName,
                FirstOperand = node.Name.ToString()
            });
        }
        
        public void CreateAndPushIntNumNode(IntNumNode node, string tmpName)
        {
            PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPart = tmpName,
                FirstOperand = node.Num.ToString()
            });
        }
        
        #endregion

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var tacNode in CodeList)
            {
                builder.Append(tacNode?.ToString() + "\n");
            }

            return builder.ToString();
        }
    }
}