using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using SimpleLang.TACode.TacNodes;
using ProgramTree;

namespace SimpleLang.TACode
{
    /// <summary>
    /// Three-address code container wrapper
    /// </summary>
    public class ThreeAddressCode: IEnumerable<TacNode>
    {
        /// <summary>
        /// Linked list representation of the tac code lines
        /// </summary>
        public LinkedList<TacNode> TACodeLines { get; }
        
        
        public TacNode this[string label]
        {
            get => GetNodeByLabel(label);
            set => SetNodeByLabel(label, value);
        }
        
        public LinkedListNode<TacNode> First => TACodeLines.First;
        public LinkedListNode<TacNode> Last => TACodeLines.Last;

        public ThreeAddressCode()
        {
            TACodeLines = new LinkedList<TacNode>();
        }

        /// <summary>
        /// Push node (a representation of TAC code line) to the end of the container
        /// </summary>
        /// <param name="node">Current node to be pushed</param>
        public void PushNode(TacNode node)
        {
            TACodeLines.AddLast(node);
        }

        /// <summary>
        /// Push multiple nodes to the end of the container
        /// </summary>
        /// <param name="nodes">Enumerable, containing nodes to be pushed</param>
        public void PushNodes(IEnumerable<TacNode> nodes)
        {
            foreach (var tacNode in nodes)
            {
                TACodeLines.AddLast(tacNode);
            }
        }
        
        /// <summary>
        /// Remove a node (a representation of TAC code line) from a container
        /// <exception>InvalidOperationException</exception>
        /// <exception>ArgumentNullException</exception>
        /// </summary>
        /// <param name="node">Node to be removed</param>
        public void RemoveNode(TacNode node)
        {
            TACodeLines.Remove(node);
        }

        /// <summary>
        /// Remove a node by label
        /// <exception>InvalidOperationException</exception>
        /// <exception>ArgumentNullException</exception>
        /// </summary>
        /// <param name="label">Label of a node to be removed</param>
        public void RemoveNodeByLabel(string label)
        {
            var labeledNode = TACodeLines.FirstOrDefault(node => string.Equals(node.Label, label));
            if (labeledNode != null)
            {
                TACodeLines.Remove(labeledNode);
            }
        }
        
        /// <summary>
        /// Remove multiple nodes from a container
        /// <exception>InvalidOperationException</exception>
        /// <exception>ArgumentNullException</exception>
        /// </summary>
        /// <param name="nodes">Enumerable, containing nodes to be removed</param>
        public void RemoveNodes(IEnumerable<TacNode> nodes)
        {
            foreach (var tacNode in nodes)
            {
                TACodeLines.Remove(tacNode);
            }
        }
        
        /// <summary>
        /// Acquire node from TAC code list by label
        /// </summary>
        /// <param name="label">Desired label</param>
        /// <returns>Node found, if not => null</returns>
        public TacNode GetNodeByLabel(string label)
        {
            return TACodeLines.FirstOrDefault(node => string.Equals(node.Label, label));
        }

        /// <summary>
        /// Set node from TAC code list by label. if not in TAC code list => push new node of @param:value
        /// <remarks>Very inefficient.</remarks>
        /// TODO: Tune efficiency
        /// </summary>
        /// <param name="label">Desired label</param>
        /// <param name="value">Node to be set at found cell</param>
        public void SetNodeByLabel(string label, TacNode value)
        {
            var current = TACodeLines.First;
            while (current.Next != null)
            {
                if (!string.Equals(current.Value.Label, label))
                {
                    current = current.Next;
                    continue;
                }
                current.Value = value;
                return;
            }
            PushNode(value);
        }

        #region Convenience methods

        /// <summary>
        /// Create TAC representation of an AST BoolNode and push it to the end of the container
        /// </summary>
        /// <param name="node">AST Boolean Node</param>
        /// <param name="label">Custom specified label for a code line</param>
        /// <returns>Identifier (left side of an assignment operation) of the current TAC line</returns>
        public string CreateAndPushBoolNode(BoolNode node, string label=null)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = label,
                LeftPartIdentifier = tmpName,
                FirstOperand = node.Value.ToString()
            });
            return tmpName;
        }

        /// <summary>
        /// Create TAC representation of an AST IdNode and push it to the end of the container
        /// </summary>
        /// <param name="node">AST Identifier Node</param>
        /// <param name="label">Custom specified label for a code line</param>
        /// <returns>Identifier (left side of an assignment operation) of the current TAC line</returns>
        public string CreateAndPushIdNode(IdNode node, string label=null)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = label,
                LeftPartIdentifier = tmpName,
                FirstOperand = node.Name.ToString()
            });
            return tmpName;
        }

        /// <summary>
        /// Create TAC representation of an AST BoolNode and push it to the end of the container
        /// </summary>
        /// <param name="node">AST Boolean Node</param>
        /// <param name="label">Custom specified label for a code line</param>
        /// <returns>Identifier (left side of an assignment operation) of the current TAC line</returns>
        public string CreateAndPushIntNumNode(IntNumNode node, string label=null)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = label,
                LeftPartIdentifier = tmpName,
                FirstOperand = node.Num.ToString()
            });
            return tmpName;
        }

        /// <summary>
        /// Create TAC representation of an AST EmptyNode and push it to the end of the container
        /// </summary>
        /// <param name="node">AST Empty Node</param>
        /// <param name="label">Custom specified label for a code line</param>
        /// <returns>Identifier (left side of an assignment operation) of the current TAC line</returns>
        public void CreateAndPushEmptyNode(EmptyNode node, string label=null)
        {
            PushNode(new TacEmptyNode()
            {
                Label = label
            });
        }

        #endregion

        #region utility methods

        public static bool IsFunction(string operand)
        {
            return operand.StartsWith("func");
        }

        #endregion
        public IEnumerator<TacNode> GetEnumerator()
        {
            return TACodeLines.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return TACodeLines.GetEnumerator();
        }
        
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
