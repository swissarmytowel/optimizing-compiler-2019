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
using SimpleLang.Utility;

namespace SimpleLang.Optimizations
{
    public class ReachingDefinitionsConstPropagation : IIterativeAlgorithmOptimizer<TacNode>
    {
        private static TacAssignmentNode Routine(HashSet<TacNode> inData, HashSet<TacNode> outData, string operand)
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
            
            foreach (var basicBlock in ita.controlFlowGraph.SourceBasicBlocks)
            {
                var isFirstBlock = false;

                var traversedNodesInBlock = new HashSet<TacAssignmentNode>();
                var inData = ita.InOut.In[basicBlock];
                var outData = ita.InOut.Out[basicBlock];

                if (inData.Count == 0)
                {
                    inData = outData;
                    isFirstBlock = true;
                }

                foreach (var line in basicBlock)
                {
                    if (!(line is TacAssignmentNode assignmentNode)) continue;
                    traversedNodesInBlock.Add(assignmentNode);

                    var firstOperand = assignmentNode.FirstOperand;
                    var secondOperand = assignmentNode.SecondOperand;

                    if (firstOperand != null)
                    {
                        var tmpValue = Routine(inData, outData, firstOperand);
                        if (tmpValue != null)
                        {
                            var encounteredRedefinition = traversedNodesInBlock.FirstOrDefault(entry =>
                                                              string.Equals(tmpValue.LeftPartIdentifier,
                                                                  entry.LeftPartIdentifier)) != null;
                            if (isFirstBlock)
                            {
                                if (!encounteredRedefinition) continue;
                            }
                            else
                            {
                                if (encounteredRedefinition) continue;
                            }
                            assignmentNode.FirstOperand = tmpValue.FirstOperand;
                            wasApplied = true;
                        }
                    }

                    if (secondOperand != null)
                    {
                        var tmpValue = Routine(inData, outData, secondOperand);
                        if (tmpValue != null)
                        {
                            var encounteredRedefinition = traversedNodesInBlock.FirstOrDefault(entry =>
                                                              string.Equals(tmpValue.LeftPartIdentifier,
                                                                  entry.LeftPartIdentifier)) != null;
                            if (isFirstBlock)
                            {
                                if (!encounteredRedefinition) continue;
                            }
                            else
                            {
                                if (encounteredRedefinition) continue;
                            }
                            assignmentNode.SecondOperand = tmpValue.FirstOperand;
                            wasApplied = true;
                        }
                    }
                }
            }
            
            return wasApplied;
        }
    }
}