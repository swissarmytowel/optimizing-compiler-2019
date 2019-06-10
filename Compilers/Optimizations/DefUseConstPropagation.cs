using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TacBasicBlocks.DefUse;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using VarNodePair = System.Tuple<string, System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>>;

namespace SimpleLang.Optimizations
{
    public class DefUseConstPropagation : IOptimizer
    {
        public DefUseDetector _detector;

        public bool Optimize(ThreeAddressCode tac)
        {
            _detector = new DefUseDetector();
            _detector.DetectAndFillDefUse(tac);
            
            var initialTac = tac.ToString();

            var node = tac.Last;

            while (node != null)
            {
                if (node.Value is TacAssignmentNode assignment)
                {
                    if (assignment.SecondOperand == null && Utility.Utility.IsNum(assignment.FirstOperand))
                    {

                        var key = new VarNodePair(assignment.LeftPartIdentifier, node);
                        if (_detector.Definitions.ContainsKey(key))
                        {
                            foreach (var usage in _detector.Definitions[key])
                            {
                                ChangeByConst(usage.Value, assignment);
                            }
                            _detector.Definitions.Remove(key);
                        }
                    }
                }

                node = node.Previous;
            }
            
            return !initialTac.Equals(tac.ToString());
        }

        private void ChangeByConst(TacNode node, TacAssignmentNode replacingNode)
        {
            switch (node) {
                case TacAssignmentNode assNode:
                    if (assNode.FirstOperand.Equals(replacingNode.LeftPartIdentifier))
                    {
                        assNode.FirstOperand = replacingNode.FirstOperand;
                    } 
                    
                    if (assNode.SecondOperand != null && assNode.SecondOperand.Equals(replacingNode.LeftPartIdentifier))
                    {
                        assNode.SecondOperand = replacingNode.FirstOperand;
                    }
                    break;
                case TacIfGotoNode ifGotoNode:
                    //ifGotoNode.Condition = replacingNode.FirstOperand;
                    break;
            }
        }
    }
}
