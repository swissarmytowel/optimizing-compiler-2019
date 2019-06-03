using System;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.DefUse.Interfaces
{
    interface ITransmissionFunction
    {
        HashSet<TacNode> Calculate(HashSet<TacNode> _in, ThreeAddressCode bbloc);
    }
}
