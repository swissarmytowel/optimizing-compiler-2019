using System;

using SimpleLang.Optimizations;
using SimpleLang.E_GenKill.Interfaces;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using System.Collections.Generic;
using SimpleLang.TacBasicBlocks;

namespace SimpleLang.E_GenKill.Implementations
{
    public class E_GenKillConatainer : IExpressionSetsContainer
    {
        private HashSet<TacNode> e_gen = new HashSet<TacNode>();
        private HashSet<TacNode> e_kill = new HashSet<TacNode>();

        public void AddToSecondSet(TacNode line)
        {
            e_kill.Add(line);
        }

        public void AddToFirstSet(TacNode line)
        {
            e_gen.Add(line);
        }

        public HashSet<TacNode> GetSecondSet() => e_kill;

        public HashSet<TacNode> GetFirstSet() => e_gen;

        public override string ToString()
        {
            var resStr = "EGen";
            foreach (var item in e_gen)
                resStr += $"\n{item}";

            resStr += "\nEKill";
            foreach (var item in e_kill)
                resStr += $"\n{item}";
            return resStr;
        }
    }
}
