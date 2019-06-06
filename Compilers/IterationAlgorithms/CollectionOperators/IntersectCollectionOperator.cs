﻿using System;
using System.Collections.Generic;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.IterationAlgorithms.CollectionOperators
{
    public class IntersectCollectionOperator : ICollectionOperator
    {
        public HashSet<TacNode> Collect(HashSet<TacNode> firstSet, HashSet<TacNode> secondSet)
        {
            var res = new HashSet<TacNode>(firstSet);
            res.IntersectWith(secondSet);
            return res;
        }
    }
}