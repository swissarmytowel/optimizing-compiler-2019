using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Optimizations;
using System.Text.RegularExpressions;
using SimpleLang.Optimizations.Interfaces;

namespace SimpleLang.Optimizations
{
    public class DeadCodeOptimization : IOptimizer
    {
        public Dictionary<string, bool> variables; // Tkey = varName, Tvalue = isAlive
        public DeadCodeOptimization()
        {
            variables = new Dictionary<string, bool>();
        }
        public DeadCodeOptimization(Dictionary<string, bool> variablesInfo)
        {
            variables = new Dictionary<string, bool>(variablesInfo);
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
            foreach (var line in block)
            {
                if (line.GetType() == typeof(TacAssignmentNode))
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
        public void ClearVariables()
        {
            variables = new Dictionary<string, bool>();
        }
        public LinkedList<TacNode> GetDeadCode(ThreeAddressCode block)
        {
            InitializeVariables(block);
            LinkedList<TacNode> toDeleteList = new LinkedList<TacNode>();

            for (int i = block.TACodeLines.Count - 1; i >= 0; --i)
            {
                if (block.TACodeLines.ElementAt(i).GetType() == typeof(TacAssignmentNode))
                {
                    string leftIdent = ((TacAssignmentNode)block.TACodeLines.ElementAt(i)).LeftPartIdentifier;


                    if (!this.variables[leftIdent])
                    {
                        toDeleteList.AddLast(block.TACodeLines.ElementAt(i));
                    }
                    this.variables[leftIdent] = false;

                    string firstOp = ((TacAssignmentNode)block.TACodeLines.ElementAt(i)).FirstOperand;
                    if (IsVariable(firstOp) || IsTempVariable(firstOp))
                        this.variables[firstOp] = true;

                    string secondOp = ((TacAssignmentNode)block.TACodeLines.ElementAt(i)).SecondOperand;
                    if (IsVariable(secondOp) || IsTempVariable(secondOp))
                        this.variables[secondOp] = true;
                }
                if (block.TACodeLines.ElementAt(i).GetType() == typeof(TacIfGotoNode))
                {
                    string cond = ((TacIfGotoNode)block.TACodeLines.ElementAt(i)).Condition;
                    this.variables[cond] = true;
                }
            }
            ClearVariables();
            return toDeleteList;
        }

        public bool Optimize(ThreeAddressCode block)
        {
            LinkedList<TacNode> deadCodeList = GetDeadCode(block);
            if (deadCodeList.Count == 0)
                return false;
            else
            {
                block.RemoveNodes(deadCodeList);
                return true;
            }
        }
    }
}
