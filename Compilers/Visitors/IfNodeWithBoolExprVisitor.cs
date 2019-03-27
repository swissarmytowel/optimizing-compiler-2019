using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class IfNodeWithBoolExprVisitor : ChangeVisitor
    {
        public override void VisitIfNode(IfNode inode)
        {
            if (inode.Expr is BoolNode bln && bln.Value == true)
            {
                inode.Stat1.Visit(this);
                ReplaceStatement(inode, inode.Stat1);
            }
            else
            {
                base.VisitIfNode(inode);
            }
        }
    }
}
