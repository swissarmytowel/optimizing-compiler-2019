using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    // базовая логика обхода без действий
    // Если нужны действия или другая логика обхода, то соответствующие методы надо переопределять
    // При переопределении методов для задания действий необходимо не забывать обходить подузлы
    class AutoVisitor: Visitor
    {
        public override void VisitBinOpNode(BinOpNode binop) 
        {
            binop.Left.Visit(this);
            binop.Right.Visit(this);
        }
        public override void VisitAssignNode(AssignNode a) 
        {
            // для каких-то визиторов порядок может быть обратный - вначале обойти выражение, потом - идентификатор
            a.Id.Visit(this);
            a.Expr.Visit(this);
        }
        public override void VisitCycleNode(CycleNode c) 
        {
            c.Expr.Visit(this);
            c.Stat.Visit(this);
        }
        public override void VisitWhileNode(WhileNode c)
        {
            c.Expr.Visit(this);
            c.Stat.Visit(this);
        }
        public override void VisitForNode(ForNode c)
        {
            c.Assign.Visit(this);
            c.Expr.Visit(this);
            c.Stat.Visit(this);
        }
        public override void VisitIfNode(IfNode c)
        {
            c.Expr.Visit(this);
            c.Stat1.Visit(this);
            if (c.Stat2 != null)
                c.Stat2.Visit(this);
        }
        public override void VisitBlockNode(BlockNode bl) 
        {
            foreach (var st in bl.StList)
                st.Visit(this);
        }
        public override void VisitWriteNode(WriteNode w) 
        {
            w.Expr.Visit(this);
        }
        public override void VisitVarDefNode(VarDefNode w) 
        {
            foreach (var v in w.vars)
                v.Visit(this);
        }
    }
}
