using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.IterationAlgorithms;
using SimpleLang.TacBasicBlocks;
using SimpleLang.Optimizations;
using System.Text.RegularExpressions;

namespace SimpleLang.Optimizations
{
    public class DeadCodeOptimizationWithITA : IIterativeAlgorithmOptimizer<TacNode>
    {
        public bool IsVariable(string val)
        {
            if (val != null)
            {
                Regex numb = new Regex(@"^[0-9](\w*)");
                if ((val != "true") && (val != "false") && numb.Matches(val).Count == 0 && val[0] != 't' && val[0] != '\"')
                    return true;
            }
            return false;
        }
        public bool IsTempVariable(string val)
        {
            if (val != null)
            {
                Regex numb = new Regex(@"^[0-9](\w*)");
                if ((val != "true") && (val != "false") && numb.Matches(val).Count == 0 && val[0] == 't' && val[0] != '\"')
                    return true;
            }
            return false;
        }
        public Dictionary<string, bool> InitializeVariables(ThreeAddressCode block, HashSet<TacNode> outData)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            foreach (var line in block)
            {
                if (line.GetType() == typeof(TacAssignmentNode))
                {
                    string leftIdent = ((TacAssignmentNode)line).LeftPartIdentifier;
                    if (IsVariable(leftIdent) && !result.ContainsKey(leftIdent))
                    {
                        result.Add(leftIdent, false);
                    }
                    else
                        if (IsTempVariable(leftIdent) && !result.ContainsKey(leftIdent))
                        result.Add(leftIdent, false);

                }
            }
            foreach (var item in outData)
            {
                if (result.ContainsKey(item.ToString()))
                    result[item.ToString()] = true;
            }
            return result;
        }

        public bool Optimize(IterationAlgorithm<TacNode> ita)
        {
            var initialTAC = ita.controlFlowGraph.SourceBasicBlocks.ToString();
            foreach (var basicBlock in ita.controlFlowGraph.SourceBasicBlocks)
            {
                var inData = ita.InOut.In[basicBlock];
                var outData = ita.InOut.Out[basicBlock];
                DeadCodeOptimization dcOpt = new DeadCodeOptimization(InitializeVariables(basicBlock, outData));
                var wasApplied = dcOpt.Optimize(basicBlock);
                while(wasApplied)
                {
                    wasApplied = dcOpt.Optimize(basicBlock);
                }
            }
            return !string.Equals(initialTAC, ita.controlFlowGraph.SourceBasicBlocks.ToString());
        }
    }
}
