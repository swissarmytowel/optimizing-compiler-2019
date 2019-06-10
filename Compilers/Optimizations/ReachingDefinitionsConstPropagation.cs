using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using QuickGraph;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.IterationAlgorithms;
using SimpleLang.TacBasicBlocks;
using SimpleLang.TacBasicBlocks.DefUse;

using SimpleLang.Utility;

namespace SimpleLang.Optimizations
{
    public class ReachingDefinitionsConstPropagation : IIterativeAlgorithmOptimizer<TacNode>
    {
        //private static TacAssignmentNode Routine(HashSet<TacNode> inData, string operand)
        private static TacAssignmentNode Routine(HashSet<TacNode> inData,  string operand)
        {
            var reachedDefinitions = new HashSet<TacNode>();
            foreach (var entry in inData)
            {
                if (!(entry is TacAssignmentNode assignmentEntry)) continue;

                if (assignmentEntry.LeftPartIdentifier == operand &&
                    assignmentEntry.SecondOperand == null &&
                    Utility.Utility.IsNum(assignmentEntry.FirstOperand))
                {
                    reachedDefinitions.Add(assignmentEntry);
                }
            }

            if (reachedDefinitions.Count == 0) return null;

            var tmpValue = (reachedDefinitions.First() as TacAssignmentNode);

            if (reachedDefinitions.Count == 1 || reachedDefinitions.All(entry =>
                    tmpValue != null && (entry as TacAssignmentNode)?.FirstOperand == tmpValue.FirstOperand))
            {
                return tmpValue;
            }

            return null;
        }

        public bool Optimize(IterationAlgorithm<TacNode> ita)
        {
            var wasApplied = false;
            var defUsePropagated = false;

            var optimizer = new DefUseConstPropagation();

            foreach (var basicBlock in ita.controlFlowGraph.SourceBasicBlocks)
            {
                var traversedNodesInBlock = new HashSet<TacAssignmentNode>();
                var inData = ita.InOut.In[basicBlock];
                var outData = ita.InOut.Out[basicBlock];

                if (inData.Count == 0) 
                {
                    defUsePropagated = optimizer.Optimize(basicBlock);
                    while (defUsePropagated)
                    {
                        defUsePropagated = optimizer.Optimize(basicBlock);
                    }
                    continue;
                }

                foreach (var line in basicBlock)
                {
                    if (!(line is TacAssignmentNode assignmentNode)) continue;
                    traversedNodesInBlock.Add(assignmentNode);

                    var firstOperand = assignmentNode.FirstOperand;
                    var secondOperand = assignmentNode.SecondOperand;

                    if (firstOperand != null && Utility.Utility.IsVariable(firstOperand))
                    {
                        var tmpValue = Routine(inData, firstOperand);
                        if (tmpValue != null)
                        {
                            var encounteredRedefinition = traversedNodesInBlock.FirstOrDefault(entry =>
                                                              string.Equals(tmpValue.LeftPartIdentifier,
                                                                  entry.LeftPartIdentifier)
                                                              && !string.Equals(tmpValue.FirstOperand,
                                                                  entry.FirstOperand)) != null;
   
                            if (!encounteredRedefinition)
                            {
                                assignmentNode.FirstOperand = tmpValue.FirstOperand;
                                wasApplied = true;
                            }
                        }
                    }

                    if (secondOperand != null && Utility.Utility.IsVariable(secondOperand))
                    {
                        var tmpValue = Routine(inData, secondOperand);
                        if (tmpValue != null)
                        {
                            var encounteredRedefinition = traversedNodesInBlock.FirstOrDefault(entry =>
                                                              string.Equals(tmpValue.LeftPartIdentifier,
                                                                  entry.LeftPartIdentifier) && !string.Equals(tmpValue.FirstOperand,
                                                                  entry.FirstOperand)) != null;
                            if (!encounteredRedefinition)
                            {
                                assignmentNode.SecondOperand = tmpValue.FirstOperand;
                                wasApplied = true;
                            }
                        }
                    }
                }

                if (!wasApplied)
                {
                    defUsePropagated = optimizer.Optimize(basicBlock);
                    while (defUsePropagated)
                    {
                        defUsePropagated = optimizer.Optimize(basicBlock);
                    }
                }
            }
            
            return wasApplied || defUsePropagated;
        }
    }
}