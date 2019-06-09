using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class RemoveSelfAssignment : ChangeVisitor
    {
        public override void VisitAssignNode(AssignNode a)
        {
            IdNode id = a.Id;
            ExprNode expr = a.Expr; 

            if (expr is IdNode idExpr)
            {
  
                if (id.Name == idExpr.Name)
                {
                    ReplaceStatement(a, new EmptyNode());
                }
            }
        }
    }
}
