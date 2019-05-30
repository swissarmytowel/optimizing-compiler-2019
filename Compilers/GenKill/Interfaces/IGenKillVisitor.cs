using System;
using System.Collections.Generic;

using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Optimizations;

namespace SimpleLang.GenKill.Interfaces
{
    public interface IGenKillVisitor
    {
        Dictionary<ThreeAddressCode, IGenKillContainer> GenerateReachingDefinitionForBlocks(BasicBlocks bblocks);
        Dictionary<TacNode, IGenKillContainer> GenerateReachingDefinitionForLine(BasicBlocks bblocks);

        IGenKillContainer GetGenKillContainer();
    }
}
