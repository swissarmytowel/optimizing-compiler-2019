using System;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations.BooleanOptimization
{
    public class OrRuleOptimization : IRule
    {
        public OrRuleOptimization()
        {
        }

        public bool IsThisRule(TacNode node)
        {
            if (node is TacAssignmentNode assignmentNode)
            {
                return assignmentNode != null
                    && (assignmentNode.FirstOperand == "true"
                    || assignmentNode.SecondOperand == "true")
                    && assignmentNode.Operation == "||";
            }

            return false;
        }

        public void Apply(TacNode node)
        {
            var assignmentNode = (node as TacAssignmentNode);

            assignmentNode.Operation = null;
            assignmentNode.FirstOperand = "true";
            assignmentNode.SecondOperand = null;
        }
    }
}
