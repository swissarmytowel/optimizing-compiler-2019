using System;
using System.Collections.Generic;
using SimpleLang.DefUse.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.DefUse.Implementations
{
    class TFByComposition
    {
        private ThreeAddressCode basicBlock;
        private Dictionary<ThreeAddressCode, IExpressionSetsContainer> lineDefUse;

        public TFByComposition(Dictionary<ThreeAddressCode, IExpressionSetsContainer> LineDefUse)
        {
            lineDefUse = LineDefUse;
        }

        public HashSet<TacNode> Calculate(HashSet<TacNode> _in, ThreeAddressCode bblock)
        {
            basicBlock = bblock;
            var func = _in;

            foreach (var line in basicBlock)
            {
                var def = new HashSet<TacNode>();
                var lineDef = lineDefUse[basicBlock].GetFirstSet();
                def.UnionWith(lineDef);

                var use = new HashSet<TacNode>();
                var lineUse = lineDefUse[basicBlock].GetSecondSet();
                use.UnionWith(lineUse);

                var exceptDef = new HashSet<TacNode>();
                exceptDef.UnionWith(func);

                exceptDef.ExceptWith(def);

                use.UnionWith(exceptDef);

                func = new HashSet<TacNode>();
                func.UnionWith(use);
            }

            return func;
        }
    }
}
