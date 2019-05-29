using System.Collections.Generic;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations
{
    public class CommonSubexprOptimization : IOptimizer
    {
        public bool Optimize(ThreeAddressCode tac)
        {
            var blocks = new BasicBlocks();
            blocks.SplitTACode(tac);

            var nodesByExpression = new Dictionary<string, LinkedListNode<TacNode>>();
            var isOptimized = false;

            foreach (var block in blocks)
            {
                nodesByExpression.Clear();
                var currentNode = block.First;
                while (currentNode != null)
                {
                    if (currentNode.Value is TacAssignmentNode assignment
                        && assignment.Operation != null
                        && !int.TryParse(assignment.FirstOperand, out _)
                        && !int.TryParse(assignment.SecondOperand, out _))
                    {
                        var expression = RightPartToString(assignment);
                        if (!nodesByExpression.ContainsKey(expression))
                            nodesByExpression.Add(expression, currentNode);
                        else
                        {
                            var node = nodesByExpression[expression];
                            var assignmentNode = node.Value as TacAssignmentNode;
                            if (IsPossibleToOptimize(node, currentNode, 
                                assignmentNode.FirstOperand, assignmentNode.SecondOperand))
                            {
                                assignment.FirstOperand = assignmentNode.LeftPartIdentifier;
                                assignment.Operation = null;
                                assignment.SecondOperand = null;
                                isOptimized = true;
                            }
                            else
                                nodesByExpression[expression] = currentNode;
                        }
                    }

                    currentNode = currentNode.Next;
                }
            }

            return isOptimized;
        }

        private static bool IsPossibleToOptimize(
            LinkedListNode<TacNode> firstNode, LinkedListNode<TacNode> lastNode, 
            string firstOpName, string secondOpName)
        {
            var currentNode = firstNode;
            while (currentNode != lastNode)
            {
                if (currentNode.Value is TacAssignmentNode assignment &&
                    (assignment.LeftPartIdentifier == firstOpName ||
                     assignment.LeftPartIdentifier == secondOpName))
                    return false;

                currentNode = currentNode.Next;
            }

            return true;
        }

        private static string RightPartToString(TacAssignmentNode node) =>
            $"{node.FirstOperand}{node.Operation}{node.SecondOperand}";
    }
}