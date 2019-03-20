using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class MaxDeepCycleVistor: AutoVisitor
    {
        private int curDeep = 0;
        private int maxDeep = 0;

        public string Result
        {
            get { return $"Максимальная глубина: = {maxDeep}"; }
        }

        private void CheckDeepInnerCycle(StatementNode stat)
        {
            if (stat == null) return;
            if (stat is WhileNode node1)
            {
                curDeep += 1;
                CheckDeepInnerCycle(node1.Stat);
            }
            if (stat is ForNode node2)
            {
                curDeep += 1;
                CheckDeepInnerCycle(node2.Stat);
            }
            else if (stat is BlockNode block)
            {
                foreach (StatementNode st in block.StList)
                    CheckDeepInnerCycle(st);
            }
            else if (stat is IfNode ifNode)
            {
                CheckDeepInnerCycle(ifNode.Stat1);
                CheckDeepInnerCycle(ifNode.Stat2);
            }
        }

        private void StartCheckDeepInnerCycle(StatementNode stat)
        {
            curDeep = 1;
            CheckDeepInnerCycle(stat);
            if (curDeep > maxDeep)
                maxDeep = curDeep;
        }

        public override void VisitWhileNode(WhileNode node)
        {
            StartCheckDeepInnerCycle(node.Stat);
        }

        public override void VisitForNode(ForNode node)
        {
            StartCheckDeepInnerCycle(node.Stat);
        }
    }
}
