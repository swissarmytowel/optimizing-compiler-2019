using System;

using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations
{
    class UnreachableCodeOpt : IOptimizer
    {
        public bool Optimize(ThreeAddressCode tac)
        {
            var isApplied = false;
            var isDeleting = false;
            var tagetLabel = "";

            foreach(var line in tac)
            {
                isDeleting &= !Equals(line.Label, tagetLabel);
                if (isDeleting)
                    tac.RemoveNode(line);
                else
                    if (line is TacIfGotoNode ifGotoNode)
                        if (Equals(ifGotoNode.Condition, "True"))
                        {
                            tagetLabel = ifGotoNode.TargetLabel; // нужно заменить текущую line на TACNodeGoToNode
                            tac[ifGotoNode.Label] = new TacGotoNode
                            {
                                Label = ifGotoNode.Label,
                                TargetLabel = ifGotoNode.TargetLabel
                            };
                            isApplied = true;
                            isDeleting = true;
                        }
            }

            return isApplied;
        }
    }
}
