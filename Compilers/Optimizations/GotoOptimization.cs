using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations
{
    public class GotoOptimization : IOptimizer
    {
        public bool Optimize(ThreeAddressCode tac)
        {
            bool isUsed = false;
            var node = tac.TACodeLines.First;
            while (node != null)
            {
                var next = node.Next;
                var val = node.Value;
                var label = val.Label;
                if (next != null)
                {
                    var nextVal = next.Value;
                    if (val is TacIfGotoNode && nextVal is TacGotoNode)
                    {
                        isUsed = true;
                        var ifVal = val as TacIfGotoNode;
                        string tempVar = TmpNameManager.Instance.GenerateTmpVariableName();
                        TacNode tempAssign = new TacAssignmentNode()
                        {
                            LeftPartIdentifier = tempVar,
                            Operation = "!",
                            SecondOperand = ifVal.Condition
                        };
                        tac.TACodeLines.AddBefore(tac.TACodeLines.Find(val), tempAssign);
                        ifVal.Condition = tempVar;
                        ifVal.TargetLabel = (nextVal as TacGotoNode).TargetLabel;
                        var remove = next;
                        next = node;
                        tac.TACodeLines.Remove(remove);
                    }
                }
                node = next;
                
            }
            return isUsed;
        }
    }
}
