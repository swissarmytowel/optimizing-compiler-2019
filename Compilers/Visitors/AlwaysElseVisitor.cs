using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class AlwaysElseVisitor: ChangeVisitor
    {
        public override void VisitIfNode(IfNode inode)
        {
            base.VisitIfNode(inode);
            if (inode.Expr is BoolNode expr && expr.Value == false)
            {
                 ReplaceStatement(inode, inode.Stat2 ?? new EmptyNode());
            }
            else
            {
                base.VisitIfNode(inode);
            }
        }

    }
}
