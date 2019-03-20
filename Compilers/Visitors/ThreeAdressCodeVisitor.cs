using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{

    class Five
    {
        public string Label, Op, Arg1, Arg2, Res;
    }

    class ThreeAdressCodeVisitor : AutoVisitor
    {
        private LinkedList<Five> fives = new LinkedList<Five> ();
        private int tmpCount = 0;
        private int labelCount = 0;

        public override void VisitAssignNode(AssignNode a)
        {
            fives.AddLast(GenUnOpFive("=", a.Expr.ToString(), a.Id.ToString()));
        }
     
        private Five GenUnOpFive(string op, string expr, string id) => new Five { Op = op, Arg1 = expr, Res = id };
        private Five GenBinOpFive(string op, string expr1, string expr2, string id) => new Five { Op = op, Arg1 = expr1, Arg2 = expr2, Res = id };
        private string genTmpName() => $"t{tmpCount++}";
        private string genLblName() => $"L{labelCount++}";


    }
}
