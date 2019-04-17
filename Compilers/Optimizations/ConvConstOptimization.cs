using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;


namespace SimpleLang.Optimizations
{
    public class ConvConstOptimization : IOptimizer
    {

        public bool Optimize(ThreeAddressCode tac)
        {

            var isOptimized = false;

            var currentNode = tac.TACodeLines.First;
            var next = currentNode.Next;

            while (currentNode != null)
            {
                var currentElement = currentNode.Value;
                if (currentElement is TacAssignmentNode assigned && assigned.SecondOperand != null)
                {
                    int int_1;
                    int int_2;

                    var leftIntParsed = int.TryParse(assigned.FirstOperand, out int_1);
                    var RightIntParsed = int.TryParse(assigned.SecondOperand, out int_2);

                    if (leftIntParsed && RightIntParsed)
                    {
                        switch (assigned.Operation)
                        {
                            case "+":
                                assigned.FirstOperand = (int_1 + int_2).ToString();
                                break;
                            case "-":
                                assigned.FirstOperand = (int_1 - int_2).ToString();
                                break;
                            case "*":
                                assigned.FirstOperand = (int_1 * int_2).ToString();
                                break;
                            case "/":
                                assigned.FirstOperand = (int_1 / int_2).ToString();
                                break;
                            case ">":
                                assigned.FirstOperand = (int_1 > int_2).ToString();
                                break;
                            case "<":
                                assigned.FirstOperand = (int_1 < int_2).ToString();
                                break;
                            case ">=":
                                assigned.FirstOperand = (int_1 >= int_2).ToString();
                                break;
                            case "<=":
                                assigned.FirstOperand = (int_1 <= int_2).ToString();
                                break;
                            case "==":
                                assigned.FirstOperand = (int_1 == int_2).ToString();
                                break;
                            case "!=":
                                assigned.FirstOperand = (int_1 != int_2).ToString();
                                break;
                        }

                        assigned.Operation = null;
                        assigned.SecondOperand = null;
                        isOptimized = true;
                    }

                    double d_1;
                    double d_2;

                    var leftDoubleParsed = double.TryParse(assigned.FirstOperand, out d_1);
                    var RightDoubleParsed = double.TryParse(assigned.SecondOperand, out d_2);

                    if (leftDoubleParsed && RightDoubleParsed)
                    {
                        switch (assigned.Operation)
                        {
                            case "+":
                                assigned.FirstOperand = (d_1 + d_2).ToString();
                                break;
                            case "-":
                                assigned.FirstOperand = (d_1 - d_2).ToString();
                                break;
                            case "*":
                                assigned.FirstOperand = (d_1 * d_2).ToString();
                                break;
                            case "/":
                                assigned.FirstOperand = (d_1 / d_2).ToString();
                                break;
                            case ">":
                                assigned.FirstOperand = (d_1 > d_2).ToString();
                                break;
                            case "<":
                                assigned.FirstOperand = (d_1 < d_2).ToString();
                                break;
                            case ">=":
                                assigned.FirstOperand = (d_1 >= d_2).ToString();
                                break;
                            case "<=":
                                assigned.FirstOperand = (d_1 <= d_2).ToString();
                                break;
                            case "==":
                                assigned.FirstOperand = (d_1 == d_2).ToString();
                                break;
                            case "!=":
                                assigned.FirstOperand = (d_1 != d_2).ToString();
                                break;
                        }

                        assigned.Operation = null;
                        assigned.SecondOperand = null;
                        isOptimized = true;
                    }
                 
                }

                currentNode = next;
                if (currentNode != null)
                    next = currentNode.Next;
            }

            return isOptimized;
        }
    }
}
