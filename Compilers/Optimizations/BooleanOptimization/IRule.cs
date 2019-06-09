using System;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations.BooleanOptimization
{
    public interface IRule
    {
        bool IsThisRule(TacNode node);

        void Apply(TacNode node);
    }
}
