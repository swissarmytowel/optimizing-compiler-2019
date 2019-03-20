using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class FillParentVisitor : AutoVisitor
    {
        Stack<Node> st = new Stack<Node>();

        public FillParentVisitor ()
        {
            st.Push(null);
        }

        public override void VisitBinOpNode(BinOpNode binop)
        {
            binop.Parent = st.Peek();
            st.Push(binop);
            base.VisitBinOpNode(binop);
            st.Pop();
        }

        public override void VisitAssignNode(AssignNode a)
        {
            a.Parent = st.Peek();
            st.Push(a);
            base.VisitAssignNode(a);
            st.Pop();
        }

        public override void VisitBlockNode(BlockNode bl)
        {
            bl.Parent = st.Peek();
            st.Push(bl);
            base.VisitBlockNode(bl);
            st.Pop();
        }

        public override void VisitEmptyNode(EmptyNode w)
        {
            w.Parent = st.Peek();
            st.Push(w);
            base.VisitEmptyNode(w);
            st.Pop();
        }

        public override void VisitForNode(ForNode c)
        {
            c.Parent = st.Peek();
            st.Push(c);
            base.VisitForNode(c);
            st.Pop();
        }

        public override void VisitIfNode(IfNode c)
        {
            c.Parent = st.Peek();
            st.Push(c);
            base.VisitIfNode(c);
            st.Pop();
        }

        public override void VisitWhileNode(WhileNode c)
        {
            c.Parent = st.Peek();
            st.Push(c);
            base.VisitWhileNode(c);
            st.Pop();
        }

        public override void VisitIdNode(IdNode id)
        {
            id.Parent = st.Peek();
            st.Push(id);
            base.VisitIdNode(id);
            st.Pop();
        }

        public override void VisitIntNumNode(IntNumNode num)
        {
            num.Parent = st.Peek();
            st.Push(num);
            base.VisitIntNumNode(num);
            st.Pop();
        }
    }
}
