using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using SimpleLang.Optimizations;
using SimpleLang.DefUse.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.DefUse.Implementations
{
    class DefUseVisitor : IDefUseVisitor
    {

        private bool IsVariable(string val)
        {
            if (val != null)
            {
                Regex numb = new Regex(@"^[0-9](\w*)");
                if ((val != "true") && (val != "false") && numb.Matches(val).Count == 0 && val[0] != 't' && val[0] != '\"')
                    return true;
            }
            return false;
        }

        private bool IsTempVariable(string val)
        {
            if (val != null)
            {
                Regex numb = new Regex(@"^[0-9](\w*)");
                if ((val != "true") && (val != "false") && numb.Matches(val).Count == 0 && val[0] == 't' && val[0] != '\"')
                    return true;
            }
            return false;
        }

        private bool IsValidVariable(string val)
        {
            return IsVariable(val) && !IsTempVariable(val);
        }

        private TacNode CreateVarTacNode(string varName)
        {
            return new TacAssignmentNode
            {
                LeftPartIdentifier = varName,
                FirstOperand = null
            };
        }

        public Dictionary<ThreeAddressCode, IExpressionSetsContainer> GenerateDefUseForBlocks(BasicBlocks bblocks)
        {
            var resultBlocksDefUse = new Dictionary<ThreeAddressCode, IExpressionSetsContainer>();
            DefUseContainer defUseContainer = new DefUseContainer();

            foreach (var bblock in bblocks)
            {
                foreach (var line in bblock)
                {
                    if (line is TacAssignmentNode assignmentNode)
                    {
                        string leftPartIdentifier = assignmentNode.LeftPartIdentifier;
                        TacNode tacNode1 = CreateVarTacNode(leftPartIdentifier);
                        if (IsValidVariable(leftPartIdentifier) && !defUseContainer.use.Contains(tacNode1))
                            defUseContainer.def.Add(tacNode1);
                        string firstOperand = assignmentNode.FirstOperand;
                        TacNode tacNode2 = CreateVarTacNode(firstOperand);
                        if (IsValidVariable(firstOperand) && !defUseContainer.def.Contains(tacNode2))
                            defUseContainer.use.Add(tacNode2);
                        string secondOperand = assignmentNode.SecondOperand;
                        TacNode tacNode3 = CreateVarTacNode(secondOperand);
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
