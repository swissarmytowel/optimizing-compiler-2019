using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class WhileFalseOptVisitor: ChangeVisitor
    {
        public override void VisitWhileNode(WhileNode c)
        {
            if (c.Expr is BoolNode bn && bn.Value == false)
            {
                ReplaceStatement(c, new EmptyNode());
            }
        }
    }
}
