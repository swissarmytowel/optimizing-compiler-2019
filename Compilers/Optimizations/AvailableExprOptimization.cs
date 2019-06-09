using System.Collections.Generic;
using System.Linq;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.IterationAlgorithms;
using SimpleLang.IterationAlgorithms.Interfaces;
using SimpleLang.TacBasicBlocks;

namespace SimpleLang.Optimizations
{
    struct TacExpr
    {
        public string FirstOperand;
        public string Operation;
        public string SecondOperand;

        public TacExpr(TacAssignmentNode assign)
        {
            FirstOperand = assign.FirstOperand;
            Operation = assign.Operation;
            SecondOperand = assign.SecondOperand;
        }

        public TacNode CreateAssignNode(string idName)
        {
            return new TacAssignmentNode()
            {
                LeftPartIdentifier = idName,
                FirstOperand = this.FirstOperand,
                Operation = this.Operation,
                SecondOperand = this.SecondOperand
            };
        }
    }

    public class AvailableExprOptimization: IIterativeAlgorithmOptimizer<TacNode>
    {
        private Dictionary<TacExpr, string> idsForExprDic = new Dictionary<TacExpr, string>();
        public BasicBlocks Blocks;

        private HashSet<TacExpr> TransformHashSetNodeToExpr(HashSet<TacNode> nodes)
        {
            var result = new HashSet<TacExpr>();
            foreach (var node in nodes)
            {
                var assign = node as TacAssignmentNode;
                result.Add(new TacExpr(assign));
            }
            return result;
        }

        private void AssignRightPartVarReplace(TacAssignmentNode assign, string idName)
        {
            assign.FirstOperand = idName;
            assign.Operation = null;
            assign.SecondOperand = null;
        }

        private bool IsVariable(string var)
        {
            return Utility.Utility.IsVariable(var);
        }


//        public bool Optimize(AvailableExpressionsITA ita)
        public bool Optimize(IterationAlgorithm<TacNode> ita)
        {
            bool isUsed = false;
            var bb = ita.controlFlowGraph.SourceBasicBlocks;
            Blocks = bb;
            Dictionary<ThreeAddressCode, HashSet<TacNode>> IN = ita.InOut.In;
            Dictionary<ThreeAddressCode, HashSet<TacNode>> OUT = ita.InOut.Out;
            // переделываем IN OUT в нужный формат
            var IN_EXPR = new Dictionary<ThreeAddressCode, HashSet<TacExpr>>();
            var OUT_EXPR = new Dictionary<ThreeAddressCode, HashSet<TacExpr>>();
            foreach (var block in bb.BasicBlockItems)
            {
                HashSet<TacExpr> inNode = TransformHashSetNodeToExpr(IN[block]);
                IN_EXPR.Add(block, inNode);
                HashSet<TacExpr> outNode = TransformHashSetNodeToExpr(OUT[block]);
                OUT_EXPR.Add(block, outNode);
            }
            Dictionary<TacExpr, int> tacExprCount = new Dictionary<TacExpr, int>();
            Dictionary<TacExpr, bool> varsExprChange = new Dictionary<TacExpr, bool>();
            // ищем какие выражения нужно оптимизировать
            foreach (var block in bb.BasicBlockItems)
            {
                foreach (TacNode node in block.TACodeLines)
                {
                    if (node is TacAssignmentNode assign)
                    {
                        if (assign.Operation != null && assign.SecondOperand != null)
                        {
                            TacExpr expr = new TacExpr(assign);
                            if (!tacExprCount.Keys.Contains(expr))
                                tacExprCount.Add(expr, 1);
                            else tacExprCount[expr] += 1;
                        }
                    }
                }
            }
            // проводим оптимизацию
            for (int blockInd = 0; blockInd < bb.BasicBlockItems.Count(); blockInd++)
            {
               var block = bb.BasicBlockItems[blockInd];
               var codeLine = block.TACodeLines.First;
               foreach (var _expr in varsExprChange.Keys.ToArray())
                    varsExprChange[_expr] = !IN_EXPR[block].Contains(_expr);
               while (codeLine != null)
               {
                    var node = codeLine.Value;
                    if (node is TacAssignmentNode assign)
                    {
                        string assignId = assign.LeftPartIdentifier;
                        TacExpr expr = new TacExpr(assign);
                        bool isCommonExpr = false;
                        if (idsForExprDic.Keys.Contains(expr) && idsForExprDic[expr] == assignId)
                            isCommonExpr = true;
                        // если выражений больше 1 делаем оптимизацию
                        if (!isCommonExpr && tacExprCount.Keys.Contains(expr) && tacExprCount[expr] > 1)
                        {
                            isUsed = true;
                            if (!varsExprChange.Keys.Contains(expr))
                                varsExprChange.Add(expr, !IN_EXPR[block].Contains(expr));
                            // если это первая замена общего выражения
                            if (!idsForExprDic.Keys.Contains(expr))
                            {
                                // создаём переменную для общего выражения
                                string idName = TmpNameManager.Instance.GenerateTmpVariableName();
                                idsForExprDic.Add(expr, idName);
                                block.TACodeLines.AddBefore(block.TACodeLines.Find(node), expr.CreateAssignNode(idName));
                                AssignRightPartVarReplace(assign, idName);
                                varsExprChange[expr] = false;
                            } else
                            {
                                string idName = idsForExprDic[expr];
                                // если это не замена общего выражения
                                if (assignId != idName)
                                    AssignRightPartVarReplace(assign, idName);
                                // если выражение недоступно на входе
                                if (varsExprChange[expr])
                                {
                                    block.TACodeLines.AddBefore(block.TACodeLines.Find(node), expr.CreateAssignNode(idName));
                                    varsExprChange[expr] = false;
                                }
                            }
                        }
                        // для всех оптимизируемых выражений
                        foreach (var _expr in varsExprChange.Keys.ToArray())
                        {
                            // если выражение недоступно на выходе и присваивание его изменяет
                            if (_expr.FirstOperand == assignId || _expr.SecondOperand == assignId)
                            {
                                varsExprChange[_expr] = true;
                            }
                        }
                    }
                    codeLine = codeLine.Next;
                }
            }
            return isUsed;
        }
    }
}
