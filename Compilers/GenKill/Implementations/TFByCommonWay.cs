using System;
using System.Collections.Generic;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.GenKill.Implementations
{
    public class TFByCommonWay : ITransmissionFunction<TacNode>
    {
        private ThreeAddressCode basicBlock;
        private Dictionary<ThreeAddressCode, IExpressionSetsContainer> lineGenKill;

        public TFByCommonWay(Dictionary<ThreeAddressCode, IExpressionSetsContainer> LineGenKill)
        {
            lineGenKill = LineGenKill;
        }

        public HashSet<TacNode> Calculate(HashSet<TacNode> _in, ThreeAddressCode bblock)
        {
            basicBlock = bblock;
            var func = _in;

            var gen = new HashSet<TacNode>();
            var _line = new TacAssignmentNode();
            gen.UnionWith(GetLineGen(_line));

            var kill = new HashSet<TacNode>();
            kill.UnionWith(GetLineKill(_line));

            foreach (var line in GetBasicBlock())
            {


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
            return lineGenKill[basicBlock].GetFirstSet();
        }

        public HashSet<TacNode> GetLineKill(TacNode tacNode)
        {
            return lineGenKill[basicBlock].GetSecondSet();
        }
    }
}
