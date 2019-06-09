using System;
using System.Collections.Generic;
using SimpleLang.TACode.TacNodes;


namespace SimpleLang.IterationAlgorithms.CollectionOperators
{
    public class IntersectCollectionOperator<TacNode> : ICollectionOperator<TacNode>
    {
        private HashSet<TacNode> IntersectAvailableExpressions(HashSet<TacNode> firstSet, HashSet<TacNode> secondSet)
        {
            var res = new HashSet<TacNode>();
            foreach (var item1 in firstSet)
                foreach (var item2 in secondSet)
                    if ((item1 as TacAssignmentNode).FirstOperand == (item2 as TacAssignmentNode).FirstOperand && 
                        (item1 as TacAssignmentNode).Operation == (item2 as TacAssignmentNode).Operation &&
                        (item1 as TacAssignmentNode).SecondOperand == (item2 as TacAssignmentNode).SecondOperand)
                    {
                        res.Add(item1);
                    }
            return res;
        }
        public HashSet<TacNode> Collect(HashSet<TacNode> firstSet, HashSet<TacNode> secondSet)
        {         
            return IntersectAvailableExpressions(firstSet as HashSet<TacNode>, secondSet as HashSet<TacNode>);
            //var res = new HashSet<T>(firstSet);
            //res.IntersectWith(secondSet);
            //return res;
        }
    }
}
