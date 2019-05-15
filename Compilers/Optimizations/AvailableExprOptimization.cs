using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.CFG;

namespace SimpleLang.Optimizations
{
    using TacExprInfoDictionary = Dictionary<TacExpr, Tuple<string, HashSet<ThreeAddressCode>>>;

    struct TacExpr
    {
        public string FirstOperand;
        public string Operation;
        public string SecondOperand;

        public TacExpr(string firstOperand, string operation, string secondOperand)
        {
            FirstOperand = firstOperand;
            Operation = operation;
            SecondOperand = secondOperand;
        }

        public TacNode CreateAssignNode(string idName)
        {
            TacNode assignNode = new TacAssignmentNode()
            {
                LeftPartIdentifier = idName,
                FirstOperand = this.FirstOperand,
                Operation = this.Operation,
                SecondOperand = this.SecondOperand
            };
            return assignNode;
        }
    }

    class AvailableExprOptimization : ICFGOptimizer
    {
        TacExprInfoDictionary infoDictionary = new TacExprInfoDictionary();

        private void OptimizationInBlock(TacExpr expr, ThreeAddressCode block, string tmpName)
        {
            var taCode = block.TACodeLines.First;
            while (taCode != null)
            {
                var node = taCode.Value;
                if (node is TacAssignmentNode assign)
                {
                    TacExpr nodeExpr = new TacExpr(assign.FirstOperand, assign.Operation, assign.SecondOperand);
                    string id = assign.LeftPartIdentifier;
                    if (id == expr.FirstOperand || id == expr.SecondOperand)
                        block.TACodeLines.AddAfter(block.TACodeLines.Find(node), expr.CreateAssignNode(tmpName));
                    if (nodeExpr.Equals(expr) && id != tmpName)
                    {
                        assign.FirstOperand = tmpName;
                        assign.Operation = null;
                        assign.SecondOperand = null;
                    }
                }
                taCode = taCode.Next;
            }
        }

        public bool Optimize(ControlFlowGraph cfg)
        {
            bool isUsed = false;
            var bb = cfg.Blocks;
            for (int source = 0; source < bb.BasicBlockItems.Count(); source++)
            {
                var sourceBlock = bb.BasicBlockItems[source];
                var sourceCodeLine = sourceBlock.TACodeLines.First;
                while (sourceCodeLine != null)
                {
                    var sourceNode = sourceCodeLine.Value;
                    if (sourceNode is TacAssignmentNode sourceAssign)
                    {
                        if (sourceAssign.SecondOperand == null) break;
                        TacExpr sourceExpr = new TacExpr(sourceAssign.FirstOperand, sourceAssign.Operation, sourceAssign.SecondOperand);
                        for (int target = 0; target < bb.BasicBlockItems.Count(); target++)
                        {
                            var targetBlock = bb.BasicBlockItems[target];
                            var targetCodeLine = targetBlock.TACodeLines.First;
                            while (targetCodeLine != null)
                            {
                                var targetNode = targetCodeLine.Value;
                                if (targetNode == sourceNode)
                                    break;
                                if (targetNode is TacAssignmentNode targetAssign)
                                {
                                    string targetAssignId = targetAssign.LeftPartIdentifier;
                                    bool isNeedAdditionaly = targetAssignId == sourceExpr.FirstOperand || targetAssignId == sourceExpr.SecondOperand;
                                    TacExpr targetExpr = new TacExpr(targetAssign.FirstOperand, targetAssign.Operation, targetAssign.SecondOperand);
                                    if (targetExpr.Equals(sourceExpr) || isNeedAdditionaly)
                                    {
                                        string tmpName = null;
                                        HashSet<ThreeAddressCode> hashSet = null;
                                        bool isIdVar = infoDictionary.ContainsKey(sourceExpr);
                                        if (!isIdVar)
                                        {
                                            tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
                                            sourceBlock.TACodeLines.AddBefore(sourceBlock.TACodeLines.Find(sourceNode), sourceExpr.CreateAssignNode(tmpName));
                                            OptimizationInBlock(sourceExpr, sourceBlock, tmpName);
                                            hashSet = new HashSet<ThreeAddressCode>();
                                            hashSet.Add(sourceBlock);
                                            infoDictionary.Add(sourceExpr, new Tuple<string, HashSet<ThreeAddressCode>>(tmpName, hashSet));
                                        }
                                        else
                                        {
                                            if (!infoDictionary.ContainsKey(sourceExpr))
                                                break;
                                            var info = infoDictionary[sourceExpr];
                                            tmpName = info.Item1;
                                            hashSet = info.Item2;
                                        }
                                        if (sourceBlock == targetBlock)
                                            break;
                                        bool isOptim = hashSet.Contains(targetBlock);
                                        if (!isOptim)
                                        {
                                            isUsed = true;
                                            hashSet.Add(targetBlock);
                                            OptimizationInBlock(sourceExpr, targetBlock, tmpName);
                                            break;
                                        }
                                    }
                                }
                                targetCodeLine = targetCodeLine.Next;
                            }
                        }
                    }
                    sourceCodeLine = sourceCodeLine.Next;
                }
            }

            return isUsed;
        }
    }
}
