using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class IsInnerCycleVisitor : AutoVisitor
    {
        private bool isInnerCycle = false;
        private bool isInnerIf = false;

        public string Result
        {
            get { return $"Есть вложенные циклы: = {isInnerCycle} Есть вложенный if: = {isInnerIf}"; }
        }

        private void CheckIsInnerCycle(StatementNode stat)
        {
            if (stat == null) return;
            if (stat is WhileNode || stat is ForNode)
            {
                isInnerCycle = true;
            }
            else if (stat is BlockNode block)
            {
                foreach (StatementNode st in block.StList)
                    CheckIsInnerCycle(st);
            }
            else if (stat is IfNode ifNode)
            {
                isInnerIf = true;
                CheckIsInnerCycle(ifNode.Stat1);
                CheckIsInnerCycle(ifNode.Stat2);
            }
        }

        public override void VisitWhileNode(WhileNode node)
        {
            CheckIsInnerCycle(node.Stat);
        }

        public override void VisitForNode(ForNode node)
        {
            CheckIsInnerCycle(node.Stat);
        }
    }
}
