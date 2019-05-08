using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{
    class AvailableExprOptimization : IBlockOptimizer
    {
        class TacExpr
        {
            public string FisrtOperand;
            public string Operation;
            public string SecondOperand;

            public TacExpr(string fisrtOperand, string secondOperand, string operation)
            {
                FisrtOperand = fisrtOperand;
                Operation = operation;
                SecondOperand = secondOperand;      
            }
        }

        public bool Optimize(BasicBlocks bb)
        {
            bool isUsed = false;
            Dictionary<TacExpr, Dictionary<ThreeAddressCode, ThreeAddressCode>> links;
            foreach (var block in bb.BasicBlockItems)
            {
                foreach (var elem in block.TACodeLines)
                {
                    if (elem is TacAssignmentNode aNode)
                    {
                        TacExpr expr = new TacExpr(aNode.FirstOperand, aNode.Operation, aNode.SecondOperand);
                    }
                    if (elem is TacGotoNode gNode)
                    {
                        //gNode.
                    }
                }
            }
            return isUsed;
        }
    }
}
