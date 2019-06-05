using System;
using System.Collections.Generic;

using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Optimizations;

namespace SimpleLang.DefUse.Interfaces
{
    interface IDefUseVisitor
    {
        Dictionary<ThreeAddressCode, IExpressionSetsContainer> GenerateDefUseForBlocks(BasicBlocks bblocks);
    }
}
