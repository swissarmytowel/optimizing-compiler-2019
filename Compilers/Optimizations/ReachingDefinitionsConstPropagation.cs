using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.IterationAlgorithms;
using SimpleLang.TacBasicBlocks;
using SimpleLang.Utility;

namespace SimpleLang.Optimizations
{
    public class ReachingDefinitionsConstPropagation : IAlgorithmOptimizer<TacNode>
    {
        private static string Routine(HashSet<TacNode> inData, HashSet<TacNode> outData, string operand)
        {
            var reachedDefinitions = new HashSet<TacNode>();
            foreach (var entry in inData)
            {
                if (!(entry is TacAssignmentNode assignmentEntry)) continue;
                
                if (assignmentEntry.LeftPartIdentifier != operand || 
                    assignmentEntry.SecondOperand != null ||
                    !Utility.Utility.IsNum(assignmentEntry.FirstOperand)) continue;
                
                if (outData.Contains(assignmentEntry))
                    reachedDefinitions.Add(assignmentEntry);
            }

            if (reachedDefinitions.Count == 0) return null;

            var tmpValue = (reachedDefinitions.First() as TacAssignmentNode)?.FirstOperand;

            if (reachedDefinitions.Count == 1 || reachedDefinitions.All(entry =>
                    (entry as TacAssignmentNode)?.FirstOperand == tmpValue))
            {
                return tmpValue;
            }

            return null;
        }

        public bool Optimize(BasicBlocks bb, IterationAlgorithm<TacNode> ita)
        {
            var wasApplied = false;
            foreach (var basicBlock in ita.controlFlowGraph.SourceBasicBlocks)
            {
                var inData = ita.InOut.In[basicBlock];
                var outData = ita.InOut.Out[basicBlock];

                if (inData.Count == 0) continue;
                foreach (var line in basicBlock)
                {
                    if (!(line is TacAssignmentNode assignmentNode)) continue;

                    var firstOperand = assignmentNode.FirstOperand;
                    var secondOperand = assignmentNode.SecondOperand;

                    if (firstOperand != null)
                    {
                        var tmpValue = Routine(inData, outData, firstOperand);
                        if (tmpValue != null)
                            assignmentNode.FirstOperand = tmpValue;
                    }

                    if (secondOperand != null)
                    {
                        var tmpValue = Routine(inData, outData, secondOperand);
                        if (tmpValue != null)
                            assignmentNode.SecondOperand = tmpValue;
                    }
                }
            }

            return true;
        }
    }
}