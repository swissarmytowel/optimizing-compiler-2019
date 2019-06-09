using System;
using System.Collections.Generic;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.IterationAlgorithms.CollectionOperators
{
    public class UnionCollectionOperator<T> : ICollectionOperator<T>
    {
        public HashSet<T> Collect(HashSet<T> firstSet, HashSet<T> secondSet)
        {
            var res = new HashSet<T>(firstSet);
            res.UnionWith(secondSet);
            return res;
        }
    }
}
