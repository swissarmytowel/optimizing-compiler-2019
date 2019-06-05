using SimpleLang.DefUse.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using System.Collections.Generic;

namespace SimpleLang.DefUse.Implementations
{
    class DefUseContainer : IExpressionSetsContainer
    {
        //множество переменных, определённых в B до любого их использования
        public HashSet<TacNode> def = new HashSet<TacNode>();
        //множество переменных, значения которых могут использоваться в B до любого их определения
        public HashSet<TacNode> use = new HashSet<TacNode>();

        public HashSet<TacNode> GetFirstSet() => def;

        public HashSet<TacNode> GetSecondSet() => use;
    }
}
