using System;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;

namespace SimpleLang.GenKill.Interfaces
{
    public interface IExpressionSetsContainer
    {
        HashSet<TacNode> GetFirstSet();
        HashSet<TacNode> GetSecondSet();

        void AddToFirstSet(TacNode line);
        void AddToSecondSet(TacNode line);
    }
}
