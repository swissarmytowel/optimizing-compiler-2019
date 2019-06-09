using System;
using System.Collections.Generic;

using SimpleLang.Optimizations;
using SimpleLang.E_GenKill.Interfaces;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using System.Text.RegularExpressions;
using SimpleLang.TacBasicBlocks;

namespace SimpleLang.E_GenKill.Implementations
{
    public class E_GenKillVisitor : IEGenKillVisitor
    {
        public string CombineRightPart(TacAssignmentNode node)
        {
            string result = node.FirstOperand;
            if (node.Operation != null)
            {
                result += node.Operation + node.SecondOperand;
            }
            return result;
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
        public HashSet<TacNode> FillUniversalSet(BasicBlocks bblocks)
        {
            var uSet = new HashSet<TacNode>();
            foreach (var bblock in bblocks)
            {
                foreach (var line in bblock)
                {
                    if (line is TacAssignmentNode assignmentNode)
                    {
                        uSet.Add(line);
                    }
                }
            }
            return uSet;
        }
        public void AddToKill(HashSet<TacNode> kill, HashSet<TacNode> uSet, string variable)
        {
            foreach (var item in uSet)
            {
                if (((TacAssignmentNode)item).FirstOperand == variable || ((TacAssignmentNode)item).SecondOperand == variable)
                {
                    kill.Add(item);
                }
            }
        }
        public void DeleteFromGen(ref HashSet<TacNode> gen, string variable)
        {
            var tmp_gen = new HashSet<TacNode>(gen);
            foreach (var item in gen)
            {
                if (((TacAssignmentNode)item).FirstOperand == variable || ((TacAssignmentNode)item).SecondOperand == variable)
                {
                    tmp_gen.Remove(item);
                }
            }
            gen = tmp_gen;
        }

        public Dictionary<ThreeAddressCode, IExpressionSetsContainer> GenerateAvailableExpressionForBlocks(BasicBlocks bblocks)
        {
            var resultSet = new Dictionary<ThreeAddressCode, IExpressionSetsContainer>();
            var uSet = FillUniversalSet(bblocks);

            foreach (var bblock in bblocks)
            {
                var killSet1 = new HashSet<TacNode>();
                var genSet1 = new HashSet<TacNode>();
                foreach (var line in bblock)
                {
                    if (line is TacAssignmentNode assignmentNode)
                    {
                        if (IsTempVariable(assignmentNode.LeftPartIdentifier))
                        {
                            genSet1.Add(line);
                            killSet1.Remove(line);
                        }
                        else
                        {
                            AddToKill(killSet1, uSet, assignmentNode.LeftPartIdentifier);
                            DeleteFromGen(ref genSet1, assignmentNode.LeftPartIdentifier);
                        }
                    }
                }
                resultSet.Add(bblock, GetGenKillContainer());
                foreach (var item in genSet1)
                {
                    resultSet[bblock].AddToFirstSet(item);
                }
                foreach (var item in killSet1)
                {
                    resultSet[bblock].AddToSecondSet(item);
                }
            }
            return resultSet;
        }

        public IExpressionSetsContainer GetGenKillContainer() => new E_GenKillConatainer();
    }
}
