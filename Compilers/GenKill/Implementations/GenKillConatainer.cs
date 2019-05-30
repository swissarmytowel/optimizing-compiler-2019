using System;

using SimpleLang.Optimizations;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using System.Collections.Generic;

namespace SimpleLang.GenKill.Implementations
{
    public class GenKillConatainer: IGenKillContainer
    {
        private HashSet<TacNode> gen = new HashSet<TacNode>();
        private HashSet<TacNode> kill = new HashSet<TacNode>();

        public void AddGen(TacNode line)
        {
            gen.Add(line);
        }

        public void AddKill(TacNode line)
        {
            kill.Add(line);
        }

        public HashSet<TacNode> GetGen() => gen;

        public HashSet<TacNode> GetKill() => kill;
    }
}
