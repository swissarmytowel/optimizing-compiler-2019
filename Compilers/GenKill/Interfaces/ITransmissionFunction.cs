using System;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.GenKill.Interfaces
{
    public interface ITransmissionFunction
    {
        HashSet<TacNode> Calculate(HashSet<TacNode> _in, ThreeAddressCode bbloc);

        HashSet<TacNode> GetLineGen(TacNode tacNode);
        HashSet<TacNode> GetLineKill(TacNode tacNode);

        ThreeAddressCode GetBasicBlock();
    }
}
