using System;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.GenKill.Interfaces
{
    public interface ITransmissionFunction<T>
    {
        HashSet<T> Calculate(HashSet<T> _in, ThreeAddressCode bbloc);

        HashSet<T> GetLineGen(T tacNode);
        HashSet<T> GetLineKill(T tacNode);

        ThreeAddressCode GetBasicBlock();
    }
}
