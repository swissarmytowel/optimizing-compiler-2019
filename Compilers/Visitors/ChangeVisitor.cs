using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class ChangeVisitor : AutoVisitor
    {
        public void ReplaceExpr(ExprNode from, ExprNode to)
        {
            var p = from.Parent;
            to.Parent = p;
            if (p is AssignNode assn)
            {
                assn.Expr = to;
            }
            else if (p is BinOpNode binopn)
            {
                if (binopn.Left == from) // Поиск подузла в Parent
                    binopn.Left = to;
                else if (binopn.Right == from)
                    binopn.Right = to;
            }
            else if (p is BlockNode)
            {
                throw new Exception("Родительский узел не содержит выражений");
            }
            else if (p is WhileNode wnode)
            {
                wnode.Expr = to;
            }
            else if (p is ForNode fnode)
            {
                fnode.Expr = to;
            }
            else if (p is IfNode inode)
            {
                inode.Expr = to;
            }
        }

        public void ReplaceStatement(StatementNode from, StatementNode to)
        {
            var p = from.Parent;
            if (p is AssignNode || p is ExprNode)
            {
                throw new Exception("Родительский узел не содержит операторов");
            }
            to.Parent = p;
            if (p is BlockNode bln)
            {
                for (var i = 0; i < bln.StList.Count; ++i)
                {
                    if (bln.StList[i] == from)
                    {
                        bln.StList[i] = to;
                        break;
                    }
                }
            }
            else if (p is IfNode ifn)
            {
                if (ifn.Stat1 == from)
                {
                    ifn.Stat1 = to;
                }
                else if (ifn.Stat2 == from)
                {
                    ifn.Stat2 = to;
                }
            }
            else
            {
                throw new Exception("ReplaceStatement не определен для данного типа узла");
            }
        }
    }
}
