using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Optimizations;
using System.Text.RegularExpressions;


namespace SimpleLang.Optimizations
{
    public class DeadCodeOptimization
    {
        public Dictionary<string, bool> variables; // Tkey = varName, Tvalue = isAlive
        public DeadCodeOptimization()
        {
            variables = new Dictionary<string, bool>();
        }
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
        public void InitializeVariables(ThreeAddressCode block)
        {
            foreach(var line in block)
            {
                if(line.GetType() == typeof(TacAssignmentNode))
                {
                    string leftIdent = ((TacAssignmentNode)line).LeftPartIdentifier;
                    if (IsVariable(leftIdent) && !this.variables.ContainsKey(leftIdent))
                        this.variables.Add(leftIdent, true);
                    else
                        if (IsTempVariable(leftIdent) && !this.variables.ContainsKey(leftIdent))
                            this.variables.Add(leftIdent, false);

                }
            }
        }
        public void AnalyzeVariables(ThreeAddressCode block)
        {
            InitializeVariables(block);
            string tempRes = "";
            for (int i = block.TACodeLines.Count - 1; i >= 0; --i)
            {
                if (block.TACodeLines.ElementAt(i).GetType() == typeof(TacAssignmentNode))
                {
                    string leftIdent = ((TacAssignmentNode)block.TACodeLines.ElementAt(i)).LeftPartIdentifier;
                    if (IsVariable(leftIdent) || IsTempVariable(leftIdent))
                    {
                        this.variables[leftIdent] = false;
                    }

                    string firstOp = ((TacAssignmentNode)block.TACodeLines.ElementAt(i)).FirstOperand;
                    if (IsVariable(firstOp) || IsTempVariable(firstOp))
                        this.variables[firstOp] = true;

                    string secondOp = ((TacAssignmentNode)block.TACodeLines.ElementAt(i)).SecondOperand;
                    if (IsVariable(secondOp) || IsTempVariable(secondOp))
                        this.variables[secondOp] = true;
                }
            }
        }
    }
}
