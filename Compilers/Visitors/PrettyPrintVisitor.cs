using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class PrettyPrintVisitor: Visitor
    {
        public string Text = "";
        private int Indent = 0;
        private bool isRoot = false;
        private bool IsBlockIndentMinus = false;
        private bool IsDisableIndent = false;

        public PrettyPrintVisitor(bool isRoot = false)
        {
            this.isRoot = isRoot;
        }

        private string IndentStr()
        {
            if (IsDisableIndent) return "";
            return new string(' ', Indent);
        }
        private void IndentPlus()
        {
            Indent += 2;
        }
        private void IndentMinus()
        {
            Indent -= 2;
        }

        public override void VisitIdNode(IdNode id) 
        {
            Text += id.Name;
        }
        public override void VisitIntNumNode(IntNumNode num) 
        {
            Text += num.Num.ToString();
        }
        public override void VisitBinOpNode(BinOpNode binop) 
        {
            Text += "(";
            binop.Left.Visit(this);
            Text += " " + binop.Op + " ";
            binop.Right.Visit(this);
            Text += ")";
        }
        public override void VisitAssignNode(AssignNode a) 
        {
            Text += IndentStr();
            a.Id.Visit(this);
            Text += " = ";
            a.Expr.Visit(this);
            Text += ';';
        }
        public override void VisitWhileNode(WhileNode node)
        {
            Text += IndentStr() + "while (";
            IsDisableIndent = true;
            node.Expr.Visit(this);
            IsDisableIndent = false;
            Text += ")";
            Text += Environment.NewLine;
            IndentPlus();
            IsBlockIndentMinus = true;
            node.Stat.Visit(this);
            if (IsBlockIndentMinus)
            {
                IndentMinus();
                IsBlockIndentMinus = false;
            }
        }
        public override void VisitForNode(ForNode node)
        {
            Text += IndentStr() + "for (";
            IsDisableIndent = true;
            node.Assign.Visit(this);
            Text += " to ";
            node.Expr.Visit(this);
            IsDisableIndent = false;
            Text += ")";
            Text += Environment.NewLine;
            IndentPlus();
            IsBlockIndentMinus = true;
            node.Stat.Visit(this);
            if (IsBlockIndentMinus)
            {
                IndentMinus();
                IsBlockIndentMinus = false;
            }
        }
        public override void VisitIfNode(IfNode node)
        {
            Text += IndentStr() + "if (";
            IsDisableIndent = true;
            node.Expr.Visit(this);
            IsDisableIndent = false;
            Text += ")";
            Text += Environment.NewLine;
            node.Stat1.Visit(this);
            if (node.Stat2 != null)
            {
                Text += Environment.NewLine;
                Text += IndentStr() + "else ";
                node.Stat2.Visit(this);
            }
        }
        public override void VisitBlockNode(BlockNode bl) 
        {
            if (IsBlockIndentMinus)
            {
                IndentMinus();
                IsBlockIndentMinus = false;
            }
            bool isFirstIndent = false;
            if (!isRoot)
            {
                Text += IndentStr() + "{" + Environment.NewLine;
                IndentPlus();
                isFirstIndent = true;
            }
            isRoot = false;

            var Count = bl.StList.Count;

            if (Count>0)
                bl.StList[0].Visit(this);
            for (var i = 1; i < Count; i++)
            {
                if (!(bl.StList[i] is EmptyNode))
                    Text += Environment.NewLine;
                bl.StList[i].Visit(this);
            }
            if (isFirstIndent)
            {
                IndentMinus();
                Text += Environment.NewLine + IndentStr() + "}";
            }
        }
    }
}
