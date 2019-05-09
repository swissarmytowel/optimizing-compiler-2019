using System;
using System.Collections.Generic;
using SimpleLang.TACode;

namespace SimpleLang.GenKill
{
    public interface IReachingDefinition
    {
        List<ThreeAddressCode> GetDefinitions();
    }
}
