using System;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations.BooleanOptimization
{
    public class AndRuleOptimization : IRule
    {
        public AndRuleOptimization()
        {
        }

        public bool IsThisRule(TacNode node)
        {
            if (node is TacAssignmentNode assignmentNode)
            {
                return assignmentNode != null
                    && (assignmentNode.FirstOperand == "false" 
                    || assignmentNode.SecondOperand == "false")
                    && assignmentNode.Operation == "&&";
            }

            return false;
        }

        public void Apply(TacNode node)
        {
            var assignmentNode = (node as TacAssignmentNode);

            assignmentNode.Operation = null;
            assignmentNode.FirstOperand = "false";
            assignmentNode.SecondOperand = null;
        }
    }
}
