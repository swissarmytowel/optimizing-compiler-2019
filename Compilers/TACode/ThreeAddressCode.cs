using System.Collections.Generic;
using System.Text;
using ProgramTree;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.TACode
{
    public class ThreeAddressCode
    {
        public LinkedList<TacNode> TACodeLines { get; }

        public ThreeAddressCode()
        {
            TACodeLines = new LinkedList<TacNode>();
        }

        public void PushNode(TacNode node)
        {
            TACodeLines.AddLast(node);
        }

        public void RemoveNode(TacNode node)
        {
            TACodeLines.Remove(node);
        }

        public void RemoveNodes(IEnumerable<TacNode> nodes)
        {
            foreach (var tacNode in nodes)
            {
                TACodeLines.Remove(tacNode);
            }
        }
        
        public void PushNodes(IEnumerable<TacNode> nodes)
        {
            foreach (var tacNode in nodes)
            {
                TACodeLines.AddLast(tacNode);
            }
        }
        #region Convenience methods

        public string CreateAndPushBoolNode(BoolNode node)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPart = tmpName,
                FirstOperand = node.Value.ToString()
            });
            return tmpName;
        }

        public string CreateAndPushIdNode(IdNode node)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPart = tmpName,
                FirstOperand = node.Name.ToString()
            });
            return tmpName;
        }

        public string CreateAndPushIntNumNode(IntNumNode node)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPart = tmpName,
                FirstOperand = node.Num.ToString()
            });
            return tmpName;
        }

        public void CreateAndPushEmptyNode(EmptyNode node)
        {
            PushNode(new TacEmptyNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel()
            });
        }
        
        #endregion

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var tacNode in TACodeLines)
            {
                builder.Append(tacNode?.ToString() + "\n");
            }

            return builder.ToString();
        }
    }
}