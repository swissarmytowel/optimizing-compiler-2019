using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class OperatorCountVisitor : AutoVisitor
    {
        public int Count = 0;

        public string Result
        {
            get { return $"Количество операторов: = {Count}"; }
        }

        public override void VisitAssignNode(AssignNode node)
        {
            Count += 1;
        }

        public override void VisitWhileNode(WhileNode node)
        {
            Count += 1;
            base.VisitWhileNode(node);
        }

        public override void VisitForNode(ForNode node)
        {
            Count += 1;
            base.VisitForNode(node);
        }

        public override void VisitIfNode(IfNode node)
        {
            Count += 1;
            base.VisitIfNode(node);
        }
    }
}
