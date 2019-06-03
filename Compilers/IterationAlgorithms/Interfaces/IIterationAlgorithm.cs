using System;
using System.Collections.Generic;
using System.Linq;
using SimpleLang.TACode;
using System.Text;

namespace SimpleLang.IterationAlgorithms.Interfaces
{
    interface IIterationAlgorithm<T>
    {
        Dictionary<ThreeAddressCode, HashSet<T>> In { get; set; }
        Dictionary<ThreeAddressCode, HashSet<T>> Out { get; set; }
    }
}
