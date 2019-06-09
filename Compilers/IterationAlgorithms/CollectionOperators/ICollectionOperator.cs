using System;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;

namespace SimpleLang.IterationAlgorithms.CollectionOperators
{
    public interface ICollectionOperator<T>
    {
        HashSet<T> Collect(HashSet<T> firstSet, HashSet<T> secondSet);
    }
}
