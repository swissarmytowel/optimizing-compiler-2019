using System;

using SimpleLang.Optimizations;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using System.Collections.Generic;

namespace SimpleLang.GenKill.Implementations
{
    public class GenKillConatainer: IExpressionSetsContainer
    {
        private HashSet<TacNode> gen = new HashSet<TacNode>();
        private HashSet<TacNode> kill = new HashSet<TacNode>();

        public void AddToSecondSet(TacNode line)
        {
            kill.Add(line);
        }

        public void AddToFirstSet(TacNode line)
        {
            gen.Add(line);
        }

        public HashSet<TacNode> GetSecondSet() => kill;

        public HashSet<TacNode> GetFirstSet() => gen;

        public override string ToString()
        {
            var resStr = "Gen";
            foreach(var item in gen)
                resStr += $"\n{item}";

            resStr += "\nKill";
            foreach (var item in kill)
                resStr += $"\n{item}";
            return resStr;
        }
    }
}
