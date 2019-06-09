using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.CFG;
using SimpleLang.IterationAlgorithms;
using SimpleLang.IterationAlgorithms.Interfaces;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TacBasicBlocks;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Utility;

namespace SimpleLang.ConstDistrib
{
    public class ConstDistributionOptimization : IIterativeAlgorithmOptimizer<SemilatticeStreamValue>
    {
        public ConstDistributionOptimization(){}

        public bool Optimize(IterationAlgorithm<SemilatticeStreamValue> ita)
        {
            var bblocks = ita.controlFlowGraph.SourceBasicBlocks.BasicBlockItems;
            var isOptimized = false;

            for (int i = 0; i < bblocks.Count; i++)
            {
                var m = ita.InOut.Out[bblocks[i]].ToDictionary(e => e.VarName);

                for(var it = bblocks[i].First; true; it = it.Next)
                {
                    var instuction = it.Value;

                    if (instuction is TacAssignmentNode tacInsruction)
                    {
                        if(Utility.Utility.IsVariable(tacInsruction.FirstOperand) &&
                            m.ContainsKey(tacInsruction.FirstOperand) &&
                            m[tacInsruction.FirstOperand].Value.TypeValue == SemilatticeValueEnum.CONST &&
                            tacInsruction.FirstOperand != m[tacInsruction.FirstOperand].Value.ConstValue
                        )
                        {
                            tacInsruction.FirstOperand = m[tacInsruction.FirstOperand].Value.ConstValue;
                            isOptimized = true;
                        }

                        if (tacInsruction.SecondOperand != null &&
                            Utility.Utility.IsVariable(tacInsruction.SecondOperand) &&
                            m.ContainsKey(tacInsruction.SecondOperand) &&
                            m[tacInsruction.SecondOperand].Value.TypeValue == SemilatticeValueEnum.CONST &&
                            tacInsruction.SecondOperand != m[tacInsruction.SecondOperand].Value.ConstValue
                        )
                        {
                            tacInsruction.SecondOperand = m[tacInsruction.SecondOperand].Value.ConstValue;
                            isOptimized = true;
                        }
                    }

                    if (it == bblocks[i].Last)
                        break;
                }
            }

            return isOptimized;
        }
    }
}