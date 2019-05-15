using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations
{
    public class AlgebraicIdentityOptimization : IOptimizer
    {
        public bool Optimize(ThreeAddressCode tac)
        {
            var isOptimized = false;
            var currentNode = tac.TACodeLines.First;
            var nextNode = currentNode.Next;

            while (currentNode != null)
            {
                var currentElement = currentNode.Value;
                if (currentElement is TacAssignmentNode assign && assign.SecondOperand != null)
                {
                    switch (assign.Operation)
                    {
                        case "+":
                            if (assign.SecondOperand == "0")
                            {
                                assign.SecondOperand = null;
                                assign.Operation = null;
                            }
                            else if (assign.FirstOperand == "0")
                            {
                                assign.FirstOperand = assign.SecondOperand;
                                assign.SecondOperand = null;
                                assign.Operation = null;
                            }
                            break;
                        case "-":
                            if (assign.SecondOperand == "0")
                            {
                                assign.SecondOperand = null;
                                assign.Operation = null;
                            }
                            else if (assign.FirstOperand == assign.SecondOperand)
                            {
                                assign.FirstOperand = "0";
                                assign.SecondOperand = null;
                                assign.Operation = null;
                            }
                            break;
                        case "*":
                            if (assign.SecondOperand == "1")
                            {
                                assign.SecondOperand = null;
                                assign.Operation = null;
                            }
                            else if (assign.FirstOperand == "1")
                            {
                                assign.FirstOperand = assign.SecondOperand;
                                assign.SecondOperand = null;
                                assign.Operation = null;
                            }
                            else if (assign.FirstOperand == "0" || assign.SecondOperand == "0")
                            {
                                assign.FirstOperand = "0";
                                assign.SecondOperand = null;
                                assign.Operation = null;
                            }
                            break;
                        case "/":
                            if (assign.SecondOperand == "1")
                            {
                                assign.SecondOperand = null;
                                assign.Operation = null;
                            }
                            else if (assign.FirstOperand == assign.SecondOperand)
                            {
                                assign.FirstOperand = "1";
                                assign.SecondOperand = null;
                                assign.Operation = null;
                            }
                            break;
                    }
                    isOptimized = true;
                }
                currentNode = nextNode;
                if (currentNode != null)
                    nextNode = currentNode.Next;
            }
            return isOptimized;
        }
    }
}
