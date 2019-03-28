using System.Collections;
using System.Collections.Generic;
using System.Text;
using ProgramTree;
using SimpleLang.TACode.TacNodes;

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
        /// Remove a node (a representation of TAC code line) from a container 
        /// </summary>
        /// <param name="node">Node to be removed</param>
        public void RemoveNode(TacNode node)
        {
            TACodeLines.Remove(node);
        }

        /// <summary>
        /// Remove multiple nodes from a container 
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

        #region Convenience methods

        /// <summary>
        /// Create TAC representation of an AST BoolNode and push it to the end of the container
        /// </summary>
        /// <param name="node">AST Boolean Node</param>
        /// <returns>Identifier (left side of an assignment operation) of the current TAC line</returns>
        public string CreateAndPushBoolNode(BoolNode node)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPartIdentifier = tmpName,
                FirstOperand = node.Value.ToString()
            });
            return tmpName;
        }

        /// <summary>
        /// Create TAC representation of an AST IdNode and push it to the end of the container
        /// </summary>
        /// <param name="node">AST Identifier Node</param>
        /// <returns>Identifier (left side of an assignment operation) of the current TAC line</returns>
        public string CreateAndPushIdNode(IdNode node)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPartIdentifier = tmpName,
                FirstOperand = node.Name.ToString()
            });
            return tmpName;
        }

        /// <summary>
        /// Create TAC representation of an AST BoolNode and push it to the end of the container
        /// </summary>
        /// <param name="node">AST Boolean Node</param>
        /// <returns>Identifier (left side of an assignment operation) of the current TAC line</returns>
        public string CreateAndPushIntNumNode(IntNumNode node)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel(),
                LeftPartIdentifier = tmpName,
                FirstOperand = node.Num.ToString()
            });
            return tmpName;
        }

        /// <summary>
        /// Create TAC representation of an AST EmptyNode and push it to the end of the container
        /// </summary>
        /// <param name="node">AST Empty Node</param>
        /// <returns>Identifier (left side of an assignment operation) of the current TAC line</returns>
        public void CreateAndPushEmptyNode(EmptyNode node)
        {
            PushNode(new TacEmptyNode()
            {
                Label = TmpNameManager.Instance.GenerateLabel()
            });
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