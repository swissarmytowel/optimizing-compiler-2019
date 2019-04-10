using System;
using SimpleLang.Optimizations.Interfaces;
using System.Collections.Generic;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations.BooleanOptimization
{
    public class BooleanOptimizer : IOptimizer
    {
        private LinkedList<IRule> rules = new LinkedList<IRule>();

        public BooleanOptimizer()
        {
            rules.AddLast(new AndRuleOptimization());
            rules.AddLast(new OrRuleOptimization());
        }

        public bool Optimize(ThreeAddressCode tac)
        {
            var isChanged = false;

            foreach (var line in tac.TACodeLines)
            {
                foreach (var rule in rules)
                {
                    if (rule.IsThisRule(line))
                    {
                        rule.Apply(line);
                    }
                }
            }

            return isChanged;
        }
    }
}
