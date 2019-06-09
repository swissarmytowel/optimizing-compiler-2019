using QuickGraph;
using SimpleLang.CFG.DominatorsTree;
using SimpleLang.TACode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.CFG
{
    public static class DSTReducibility
    {
        public static bool IsReducibility(ControlFlowGraph cfg)
        {
            var edgeClassifierService = new EdgeClassifierService(cfg);
            var retreatingEdges = edgeClassifierService.RetreatingEdges;
            var backEdges = edgeClassifierService.BackEdges;
            return retreatingEdges.SetEquals(backEdges);
        }
    }
}
