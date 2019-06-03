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
            gen.Add(line);
        }

        public void AddToFirstSet(TacNode line)
        {
            kill.Add(line);
        }

        public HashSet<TacNode> GetSecondSet() => gen;

        public HashSet<TacNode> GetFirstSet() => kill;
    }
}
