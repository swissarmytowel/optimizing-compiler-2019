using System;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;

namespace SimpleLang.DefUse.Interfaces
{
    public interface IExpressionSetsContainer
    {
        HashSet<TacNode> GetFirstSet();
        HashSet<TacNode> GetSecondSet();
    }
}
