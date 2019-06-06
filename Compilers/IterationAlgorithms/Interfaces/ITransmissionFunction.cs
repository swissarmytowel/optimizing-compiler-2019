using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TACode;

namespace SimpleLang.IterationAlgorithms.Interfaces
{
    interface ITransmissionFunction<T>
    {
        HashSet<T> Calculate(ThreeAddressCode tac, HashSet<T> x);
    }
}
