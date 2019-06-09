using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode.TacNodes;
using System.Collections.Generic;

namespace SimpleLang.DefUse
{
    class DefUseContainer : IExpressionSetsContainer
    {
        //множество переменных, значения которых могут использоваться в B до любого их определения
        public HashSet<TacNodeVarDecorator> use = new HashSet<TacNodeVarDecorator>();
        //множество переменных, определённых в B до любого их использования
        public HashSet<TacNodeVarDecorator> def = new HashSet<TacNodeVarDecorator>();

        public HashSet<TacNode> GetFirstSet() {
            HashSet<TacNode> res = new HashSet<TacNode>();
            foreach (var el in use)
                res.Add(el);
            return res;
        }

        public HashSet<TacNode> GetSecondSet() {
            HashSet<TacNode> res = new HashSet<TacNode>();
            foreach (var el in def)
                res.Add(el);
            return res;
        }

        public void AddToFirstSet(TacNode line)
        {
            use.Add(line as TacNodeVarDecorator);
        }

        public void AddToSecondSet(TacNode line)
        {
            def.Add(line as TacNodeVarDecorator);
        }
    }
}
