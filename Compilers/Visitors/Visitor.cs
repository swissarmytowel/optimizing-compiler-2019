using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public abstract class Visitor
    {
        public virtual void VisitIdNode(IdNode id) { }
        public virtual void VisitIntNumNode(IntNumNode num) { }
        public virtual void VisitDoubleNumNode(DoubleNumNode num) { }
        public virtual void VisitBoolNode(BoolNode v) { }
        public virtual void VisitLogicNotNode(LogicNotNode lnot) { }
        public virtual void VisitBinOpNode(BinOpNode binop) { }
        public virtual void VisitUnOpNode(UnOpNode unop) { }
        public virtual void VisitAssignNode(AssignNode a) { }
        public virtual void VisitWhileNode(WhileNode w) { }
        public virtual void VisitForNode(ForNode f) { }
        public virtual void VisitLabelNode(LabelNode l) { }
        public virtual void VisitGotoNode(GotoNode gt) { }
        public virtual void VisitIfNode(IfNode i) { }
        public virtual void VisitBlockNode(BlockNode bl) { }
        public virtual void VisitEmptyNode(EmptyNode w) { }
        public virtual void VisitFunctionNode(FunctionNode f) { }
    }
}
