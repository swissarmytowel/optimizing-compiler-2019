using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using VarNodePair = System.Tuple<string, System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>>;

namespace SimpleLang.Optimizations.DefUse
{
    class DefUseConstPropagation : IOptimizer
    {
        private DefUseDetector detector;

        public DefUseConstPropagation(DefUseDetector _detector)
        {
            detector = _detector;
        }

        public bool Optimize(ThreeAddressCode tac)
        {
            var initialTac = tac.ToString();

            var node = tac.Last;

            while (node != null)
            {
                if (node.Value is TacAssignmentNode assignment)
                {
                    if (assignment.SecondOperand == null && Utility.Utility.IsNum(assignment.FirstOperand))
                    {

                        var key = new VarNodePair(assignment.LeftPartIdentifier, node);
                        if (detector.DefNodes.ContainsKey(key))
                        {
                            foreach (var usage in detector.DefNodes[key])
                            {
                                ChangeByConst(usage.Value, assignment);
                            }
                            detector.DefNodes.Remove(key);
                        }
                    }
                }

                node = node.Previous;
            }
            
            return !initialTac.Equals(tac.ToString());
        }

        private void ChangeByConst(TacNode _tacNode, TacAssignmentNode replacingNode)
        {
            switch (_tacNode) {
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
