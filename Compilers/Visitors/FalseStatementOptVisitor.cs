using System;
using ProgramTree;
using SimpleLang.Visitors;

class FalseStatementOptVisitor : ChangeVisitor
{
    public override void VisitAssignNode(AssignNode assign)
    {
        if (assign.Expr is IdNode idn && assign.Id.Name == idn.Name)
        {
            ReplaceStatement(assign, null); // Заменить на null.

            // Потом этот null надо специально проверять!!!

        }
        // Не обходить потомков
    }
    public override void VisitIfNode(IfNode ifnode)
    {
        if (ifnode.Expr is BoolNode bnn && bnn.Value == false)
        {
            //if (ifnode.Stat2 != null)
            //{
            //    ifnode.Stat2.Visit(this); // Вначале обойти потомка

            //}
            ReplaceStatement(ifnode, ifnode.Stat2);
        }
        else
        {
            base.VisitIfNode(ifnode);
        }
    }
}