using System;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;

namespace SimpleLang.GenKill.Interfaces
{
    public interface IGenKillContainer
    {
        HashSet<TacNode> GetKill();
        HashSet<TacNode> GetGen();

        void AddKill(TacNode line);
        void AddGen(TacNode line);
    }
}
