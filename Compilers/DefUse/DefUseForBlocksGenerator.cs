using System.Collections.Generic;
using System.Text.RegularExpressions;

using SimpleLang.Optimizations;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TacBasicBlocks;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.DefUse
{
    public static class DefUseForBlocksGenerator
    {
  
        static private bool IsValidVariable(string val)
        {
            return val != null && Utility.Utility.IsVariable(val);
        }

        static private TacNodeVarDecorator CreateVarTacNode(string varName)
        {
            return new TacNodeVarDecorator { VarName = varName };
        }

        static public Dictionary<ThreeAddressCode, IExpressionSetsContainer> Execute(BasicBlocks bblocks)
        {
            var resultBlocksDefUse = new Dictionary<ThreeAddressCode, IExpressionSetsContainer>();
            
            foreach (var bblock in bblocks)
            {
                DefUseContainer defUseContainer = new DefUseContainer();
                foreach (var line in bblock)
                {
                    if (line is TacAssignmentNode assignmentNode)
                    {
                        string leftPartIdentifier = assignmentNode.LeftPartIdentifier;
                        TacNodeVarDecorator tacNode1 = CreateVarTacNode(leftPartIdentifier);
                        bool defAdd = false;
                        if (IsValidVariable(leftPartIdentifier) && !defUseContainer.def.Contains(tacNode1) 
                            && !defUseContainer.use.Contains(tacNode1)) {
                            defAdd = true;
                            defUseContainer.def.Add(tacNode1);
                        }
                        string firstOperand = assignmentNode.FirstOperand;
                        TacNodeVarDecorator tacNode2 = CreateVarTacNode(firstOperand);
                        if (IsValidVariable(firstOperand) && (!defUseContainer.def.Contains(tacNode2) ||
                            (leftPartIdentifier == firstOperand && defAdd)))
                            defUseContainer.use.Add(tacNode2);
                        string secondOperand = assignmentNode.SecondOperand;
                        TacNodeVarDecorator tacNode3 = CreateVarTacNode(secondOperand);
                        if (IsValidVariable(secondOperand) && (!defUseContainer.def.Contains(tacNode3) ||
                            (leftPartIdentifier == secondOperand && defAdd)))
                            defUseContainer.use.Add(tacNode3);
                    }
                }
                resultBlocksDefUse.Add(bblock, defUseContainer);
            }

            return resultBlocksDefUse;
        }
    }
}
