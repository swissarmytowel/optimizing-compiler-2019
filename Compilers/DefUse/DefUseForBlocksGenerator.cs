using System.Collections.Generic;
using System.Text.RegularExpressions;

using SimpleLang.Optimizations;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TacBasicBlocks;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.DefUse
{
    static class DefUseForBlocksGenerator
    {
        static private bool IsVariable(string val)
        {
            if (val != null)
            {
                Regex numb = new Regex(@"^[0-9](\w*)");
                if ((val != "true") && (val != "false") && numb.Matches(val).Count == 0 && val[0] != 't' && val[0] != '\"')
                    return true;
            }
            return false;
        }

        static private bool IsTempVariable(string val)
        {
            if (val != null)
            {
                Regex numb = new Regex(@"^[0-9](\w*)");
                if ((val != "true") && (val != "false") && numb.Matches(val).Count == 0 && val[0] == 't' && val[0] != '\"')
                    return true;
            }
            return false;
        }

        static private bool IsValidVariable(string val)
        {
            return IsVariable(val) && !IsTempVariable(val);
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
                        if (IsValidVariable(leftPartIdentifier) && !defUseContainer.use.Contains(tacNode1))
                            defUseContainer.def.Add(tacNode1);
                        string firstOperand = assignmentNode.FirstOperand;
                        TacNodeVarDecorator tacNode2 = CreateVarTacNode(firstOperand);
                        if (IsValidVariable(firstOperand) && !defUseContainer.def.Contains(tacNode2))
                            defUseContainer.use.Add(tacNode2);
                        string secondOperand = assignmentNode.SecondOperand;
                        TacNodeVarDecorator tacNode3 = CreateVarTacNode(secondOperand);
                        if (IsValidVariable(secondOperand) && !defUseContainer.def.Contains(tacNode3))
                            defUseContainer.use.Add(tacNode3);
                    }
                }
                resultBlocksDefUse.Add(bblock, defUseContainer);
            }

            return resultBlocksDefUse;
        }
    }
}
