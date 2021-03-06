﻿using System;
using System.Collections.Generic;

using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Optimizations;
using SimpleLang.TacBasicBlocks;

namespace SimpleLang.GenKill.Interfaces
{
    public interface IGenKillVisitor
    {
        Dictionary<ThreeAddressCode, IExpressionSetsContainer> GenerateReachingDefinitionForBlocks(BasicBlocks bblocks);
        Dictionary<TacNode, IExpressionSetsContainer> GenerateReachingDefinitionForLine(BasicBlocks bblocks);

        IExpressionSetsContainer GetGenKillContainer();
    }
}
