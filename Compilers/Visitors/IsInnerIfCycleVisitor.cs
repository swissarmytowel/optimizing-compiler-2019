using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class IsInnerIfCycleVisitor : AutoVisitor
    {
        private bool isInnerCycle = false;

        public string Result
        {
            get { return $"В if есть вложенные циклы: = {isInnerCycle}"; }
        }

        private void CheckIsInnerCycle(StatementNode stat)
        {
            if (stat == null) return;
            if (stat is WhileNode || stat is ForNode)
                isInnerCycle = true;
            else if (stat is BlockNode block)
            {
                foreach (StatementNode st in block.StList)
                    CheckIsInnerCycle(st);
            }
            else if (stat is IfNode ifNode)
            {
                CheckIsInnerCycle(ifNode.Stat1);
                CheckIsInnerCycle(ifNode.Stat2);
            }
        }

        public override void VisitIfNode(IfNode c)
        {
            CheckIsInnerCycle(c.Stat1);
            CheckIsInnerCycle(c.Stat2);
        }
    }
}
