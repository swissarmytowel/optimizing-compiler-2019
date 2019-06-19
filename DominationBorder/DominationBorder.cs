using SimpleLang.CFG;
using SimpleLang.CFG.DominatorsTree;
using SimpleLang.TACode;
using System.Collections.Generic;
using System.Linq;

namespace DominationBorder
{
    public static class DominationBorder
    {
        public static HashSet<ThreeAddressCode> Execute(ControlFlowGraph cfg, ThreeAddressCode n)
        {
            var res = new HashSet<ThreeAddressCode>();
            var dominatorsFinder = new DominatorsFinder(cfg);
            var Pred = cfg.InEdges(n).Select(edge => edge.Source);
            var iDomN = dominatorsFinder.ImmediateDominators[n];
            if (Pred.Count() > 1)
            {
                foreach(var p in Pred)
                {
                    var r = p;
                    while(r != iDomN)
                    {
                        res.Add(r);
                        r = dominatorsFinder.ImmediateDominators[r];
                    }
                }
            }
            return res;
        }
    }
}
