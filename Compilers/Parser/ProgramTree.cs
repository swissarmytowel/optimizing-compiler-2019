﻿using System.Collections.Generic;
using System.Text;
using SimpleLang.Visitors;

namespace ProgramTree
{
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };

    public abstract class Node // базовый класс для всех узлов    
    {
        public Node Parent;
        public abstract void Visit(Visitor v);
    }

    public abstract class ExprNode : Node // базовый класс для всех выражений
    {
    }

    public class FunctionNode : ExprNode
    {
        public string Сall { get; set; }
        public FunctionNode(IdNode idNode) { Сall = idNode.Name + "()"; }
        public override void Visit(Visitor v)
        {
            v.VisitFunctionNode(this);
        }
        public override string ToString() => Сall;
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public IdNode(string name) { Name = name; }
        public override void Visit(Visitor v)
        {
            v.VisitIdNode(this);
        }
        public override string ToString() => Name;
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }
        public override void Visit(Visitor v)
        {
            v.VisitIntNumNode(this);
        }
        public override string ToString() => Num.ToString();
    }
    public class DoubleNumNode : ExprNode
    {
        public double Num { get; set; }
        public DoubleNumNode(double num) { Num = num; }
        public override void Visit(Visitor v)
        {
            v.VisitDoubleNumNode(this);
        }
        public override string ToString() => Num.ToString();

    }

    public class UnOpNode : ExprNode
    {
        public ExprNode Unary { get; set; }
        public string Op { get; set; }
        public UnOpNode(ExprNode unary,  string op = "-")  {Unary = unary; Op = op; }
        public override void Visit(Visitor v)
        {
            v.VisitUnOpNode(this);
        }
        public override string ToString() => "("+ Op + Unary + ")";
    }
    public class BinOpNode : ExprNode
    {
        public ExprNode Left { get; set; }
        public ExprNode Right { get; set; }
        public string Op { get; set; }
        public BinOpNode(ExprNode Left, ExprNode Right, string op)
        {
            this.Left = Left;
            this.Right = Right;
            this.Op = op;
        }
        public override void Visit(Visitor v)
        {
            v.VisitBinOpNode(this);
        }
        public override string ToString() => "(" + Left + Op + Right + ")";
    }

    public class BoolNode : ExprNode
    {
        public bool Value { get; set; }
        public BoolNode(bool val) { Value = val; }
        public override void Visit(Visitor v)
        {
            v.VisitBoolNode(this);
        }
        public override string ToString() => Value.ToString();

    }
    

    public class LogicNotNode : ExprNode
    {
        public ExprNode LogExpr { get; set; }
        public SimpleParser.Tokens Operation { get; set; }
        public LogicNotNode(ExprNode LogExpr)
        {
            this.LogExpr = LogExpr;
        }
        public override void Visit(Visitor v)
        {
            v.VisitLogicNotNode(this);
        }
        public override string ToString()
        {
            return "!" + LogExpr.ToString();
        }
    }
    public abstract class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class AssignNode : StatementNode
    {
        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }
        public AssignNode(IdNode id, ExprNode expr, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
        }
        public override void Visit(Visitor v)
        {
            v.VisitAssignNode(this);
        }
        public override string ToString() => Id + " = " + Expr;
    }

 

    public class WhileNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public WhileNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
        public override void Visit(Visitor v)
        {
            v.VisitWhileNode(this);
        }
        public override string ToString()
        {
            return "while(" + Expr + ")\n" + Stat;
        }
    }

    public class ForNode : StatementNode
    {
        public AssignNode Assign { get; set; }
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public ForNode(StatementNode assign, ExprNode expr, StatementNode stat)
        {
            Assign = assign as AssignNode;
            Expr = expr;
            Stat = stat;
        }
        public override void Visit(Visitor v)
        {
            v.VisitForNode(this);
        }
        public override string ToString()
        {
            return "for(" + Assign + " to " + Expr + ")\n" + Stat;
        }
    }

    public class LabelNode : StatementNode
    {
        public int Inum { get; set; }
        public LabelNode(int inum)
        {
            Inum = inum;
        }
        public override void Visit(Visitor v)
        {
            v.VisitLabelNode(this);
        }
        public override string ToString()
        {
            return "l" + Inum + ": ";
        }
    }
    public class GotoNode : StatementNode
    {
        public LabelNode L{ get; set; }
        public GotoNode(LabelNode l)
        {
            L = l;
        }
        public override void Visit(Visitor v)
        {
            v.VisitGotoNode(this);
        }
        public override string ToString()
        {
            return "goto " + L;
        }
    }

    public class IfNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat1 { get; set; }
        public StatementNode Stat2 { get; set; }
        public IfNode(ExprNode expr, StatementNode stat1, StatementNode stat2 = null)
        {
            Expr = expr;
            Stat1 = stat1;
            Stat2 = stat2;
        }
        public override void Visit(Visitor v)
        {
            v.VisitIfNode(this);
        }
        public override string ToString()
        {
            string str = "if(" + Expr + ")\n" + Stat1;
            if (Stat2 != null)
                str += "\nelse\n" + Stat2;
            return str;
        }
    }
   

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }
        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }
        public override void Visit(Visitor v)
        {
            v.VisitBlockNode(this);
        }
        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (var st in StList)
                s.Append(st.ToString() + ";\n");
            return s.ToString();
        }
    }

 

    public class EmptyNode : StatementNode
    {
        public override void Visit(Visitor v)
        {
            v.VisitEmptyNode(this);
        }
        public override string ToString() => "";
    }

   
}