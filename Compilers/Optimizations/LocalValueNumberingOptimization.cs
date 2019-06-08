using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations
{
    public class LocalValueNumberingOptimization : IOptimizer
    {
        public bool Optimize(ThreeAddressCode tac)
        {
            bool isUsed = false;
            var node = tac.TACodeLines.First;

            var currentNumber = 0;
            var valueToNumber = new Dictionary<string, int>();
            var valueDict = new Dictionary<int, LinkedListNode<TacNode>>();
            var numberToT = new Dictionary<int, string>();
            var parameters = tac.TACodeLines.OfType<TacAssignmentNode>().Select(e => e.LeftPartIdentifier);

            while (node != null)
            {
                var val = node.Value;
                var label = val.Label;
                if (val is TacAssignmentNode assigned)
                {

                    var Ti = assigned.LeftPartIdentifier;
                    var Li = assigned.FirstOperand;
                    var Ri = assigned.SecondOperand;
                    var Opi = assigned.Operation ?? String.Empty;

                    if (Ri != null)
                    {
                        int valL;
                        int valR;
                        if (!valueToNumber.TryGetValue(Li, out valL))
                        {
                            valL = currentNumber++;
                            valueToNumber.Add(Li, valL);
                        }

                        if (!valueToNumber.TryGetValue(Ri, out valR))
                        {
                            valR = currentNumber++;
                            valueToNumber.Add(Ri, valR);
                        }
                        var hash = $"{valL} {Opi} {valR}".Trim();
                        var hashReversed = $"{valR} {Opi} {valL}".Trim();
                        int tmp;
                        if (valueToNumber.TryGetValue(hash, out tmp) || valueToNumber.TryGetValue(hashReversed, out tmp))
                        {
                            isUsed = true;
                            valueToNumber[Ti] = tmp;
                            var paramToNumber = valueToNumber.Where(e => parameters.Contains(e.Key) && e.Key != Ti);
                            var findable = paramToNumber.FirstOrDefault(e => e.Value == tmp);
                            if (findable.Key != null)
                            {
                                assigned.FirstOperand = findable.Key;
                                assigned.Operation = null;
                                assigned.SecondOperand = null;
                            }
                            else
                            {

                                var tmpTacNode = new TacAssignmentNode()
                                {
                                    LeftPartIdentifier = TmpNameManager.Instance.GenerateTmpVariableName(),
                                    FirstOperand = numberToT[tmp]
                                };
                                tac.TACodeLines.AddAfter(valueDict[tmp], tmpTacNode);
                                assigned.FirstOperand = tmpTacNode.LeftPartIdentifier;
                                assigned.Operation = null;
                                assigned.SecondOperand = null;
                            }
  
                            valueDict[tmp] = node;
                        }
                        else
                        {
                            var valN = currentNumber++;
                            valueToNumber.Add(hash, valN);              
                            valueDict[valN] = node;
                            valueToNumber[Ti] = valN;
                            numberToT[valN] = Ti;
                        }
                    }
                    else
                    {
                        int valL;
                        if (!valueToNumber.TryGetValue(Li, out valL))
                        {
                            valL = currentNumber++;
                            valueToNumber.Add(Li, valL);
                        }
                        valueDict[valL] = node;
                        valueToNumber[Ti] = valL;
                        numberToT[valL] = Ti;

                    }    
                }
                node = node.Next;
            }
            return isUsed;
        }
    }
}
