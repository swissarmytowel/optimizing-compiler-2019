using System;
using System.Collections.Generic;

using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Optimizations;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TacBasicBlocks;
namespace SimpleLang.E_GenKill.Interfaces
{
    public interface IEGenKillVisitor
    {
        Dictionary<ThreeAddressCode, IExpressionSetsContainer> GenerateAvailableExpressionForBlocks(BasicBlocks bblocks);

        IExpressionSetsContainer GetGenKillContainer();
    }
}