using System;
using System.Collections.Generic;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.GenKill.Implementations
{
    public class TFByComposition : ITransmissionFunction
    {
        private ThreeAddressCode basicBlock;
        private Dictionary<ThreeAddressCode, IExpressionSetsContainer> lineGenKill;

        public TFByComposition(ThreeAddressCode BasicBlock, Dictionary<ThreeAddressCode, IExpressionSetsContainer> LineGenKill)
        {
            basicBlock = BasicBlock;
            lineGenKill = LineGenKill;
        }

        public HashSet<TacNode> Calculate(HashSet<TacNode> _in)
        {
            var func = _in;

            foreach (var line in GetBasicBlock())
            {
                var gen = new HashSet<TacNode>();
                gen.UnionWith(GetLineGen(line));

                var kill = new HashSet<TacNode>();
                kill.UnionWith(GetLineKill(line));

                var exceptKill = new HashSet<TacNode>();
                exceptKill.UnionWith(func);

                exceptKill.ExceptWith(kill);

                gen.UnionWith(exceptKill);

                func = new HashSet<TacNode>();
                func.UnionWith(gen);
            }

            return func;
        }

        public ThreeAddressCode GetBasicBlock()
        {
            return basicBlock;
        }

        public HashSet<TacNode> GetLineGen(TacNode tacNode)
        {
            return lineGenKill[basicBlock].GetSecondSet();
        }

        public HashSet<TacNode> GetLineKill(TacNode tacNode)
        {
            return lineGenKill[basicBlock].GetFirstSet();
        }
    }
}
