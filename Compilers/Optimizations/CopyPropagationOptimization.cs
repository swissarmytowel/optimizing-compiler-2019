using System.Collections.Generic;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TacBasicBlocks;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{
    public class CopyPropagationOptimization : IOptimizer
    {
        public bool Optimize(ThreeAddressCode tac)
        {
            var isOptimized = false;
            var directAssignments = new Dictionary<string, string>();

            var blocks = new BasicBlocks();
            blocks.SplitTACode(tac);

            foreach (var block in blocks)
            {
                var currentNode = block.First;
                while (currentNode != null)
                {
                    if (currentNode.Value is TacAssignmentNode assignmentNode)
                    {
                        if (directAssignments.ContainsKey(assignmentNode.LeftPartIdentifier))
                            directAssignments.Remove(assignmentNode.LeftPartIdentifier);

                        if (assignmentNode.Operation == null)
                        {
                            if (!int.TryParse(assignmentNode.FirstOperand, out int firstOpValue))
                            {
                                var id = assignmentNode.LeftPartIdentifier;
                                var firstOp = assignmentNode.FirstOperand;
                                directAssignments.Add(id, firstOp);
                            }
                        }

                        if (directAssignments.ContainsKey(assignmentNode.FirstOperand))
                        {
                            assignmentNode.FirstOperand = directAssignments[assignmentNode.FirstOperand];
                            isOptimized = true;
                        }

                        if (assignmentNode.SecondOperand != null && directAssignments.ContainsKey(assignmentNode.SecondOperand))
                        {
                            assignmentNode.SecondOperand = directAssignments[assignmentNode.SecondOperand];
                            isOptimized = true;
                        }
                        
                    }

                    currentNode = currentNode.Next;
                }
            }

            return isOptimized;
        }
    }
}
