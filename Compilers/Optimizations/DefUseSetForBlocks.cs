using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{
    using SetsType = Dictionary<ThreeAddressCode, List<string>>;

    class DefUseSetForBlocks 
    {
        public SetsType DefSets { get; private set; }
        public SetsType UseSets { get; private set; }

        public DefUseSetForBlocks(BasicBlocks bb)
        {
            DefSets = new SetsType();
            UseSets = new SetsType();
            foreach (var block in bb.BasicBlockItems)
            {
                List<string> defSet = new List<string>();
                List<string> useSet = new List<string>();
                var deadCodeOpt = new DeadCodeOptimization();
                deadCodeOpt.AnalyzeVariables(block);
                var vars = deadCodeOpt
                                .variables
                                .Where(obj => obj.Value)
                                .Select(obj => obj.Key)
                                .ToList();
                if (vars.Count > 0)
                {
                    foreach (var elem in block.TACodeLines)
                        if (elem is TacAssignmentNode assignNode)
                        {
                            var Id = assignNode.LeftPartIdentifier;
                            if (vars.Contains(Id) && !defSet.Contains(Id))
                                defSet.Add(Id);
                            var firstOp = assignNode.FirstOperand;
                            if (vars.Contains(firstOp) && !useSet.Contains(firstOp))
                                useSet.Add(firstOp);
                            var secondOp = assignNode.SecondOperand;
                            if (vars.Contains(secondOp) && !useSet.Contains(secondOp))
                                useSet.Add(secondOp);
                        }
                }
                DefSets.Add(block, defSet);
                UseSets.Add(block, useSet);
            }
        }

        public override string ToString()
        {
            var res = "";
            res += "DefSets:\n";
            foreach (var defSet in DefSets)
                res += "(" + defSet.Key + ") => [" + string.Join(",", defSet.Value) + "]\n";
            res += "UseSets:\n";
            foreach (var useSet in UseSets)
                res += "(" + useSet.Key + ") => [" + string.Join(",", useSet.Value) + "]\n";
            return res;
        }


    }
}
