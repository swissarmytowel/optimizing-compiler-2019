using SimpleLang.Optimizations.Interfaces;
using SimpleLang.TacBasicBlocks.DefUse;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using VarNodePair = System.Tuple<string, System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>>;

namespace SimpleLang.Optimizations
{
    public class DefUseCopyPropagation : IOptimizer
    {
        private DefUseDetector _detector;

        public bool Optimize(ThreeAddressCode tac)
        {
            _detector = new DefUseDetector();
            _detector.DetectAndFillDefUse(tac);
            
            var initialTac = tac.ToString();

            var node = tac.First;

            while (node != null)
            {
                if (node.Value is TacAssignmentNode assignment)
                {
                    if (assignment.SecondOperand == null && Utility.Utility.IsVariable(assignment.FirstOperand))
                    {

                        var key = new VarNodePair(assignment.LeftPartIdentifier, node);
                        if (_detector.Definitions.ContainsKey(key))
                        {
                            foreach (var usage in _detector.Definitions[key])
                            {
                                if (usage.Value == node.Value) continue;

                                ChangeByVariable(usage.Value, assignment);
                                var keyAdded = new VarNodePair(assignment.FirstOperand, usage);
                                var keyDefenition = new VarNodePair(assignment.FirstOperand, node);

                                _detector.Usages[keyAdded] = _detector.Usages[keyDefenition];
                                if (_detector.Usages[keyDefenition] != null)
                                {
                                    var keyUsage = new VarNodePair(assignment.FirstOperand, _detector.Usages[keyDefenition]);
                                    _detector.Definitions[keyUsage].Add(node);
                                }
                            }
                            _detector.Definitions.Remove(key);
                        }
                    }
                }

                node = node.Next;
            }

            return !initialTac.Equals(tac.ToString());
        }

        private void ChangeByVariable(TacNode node, TacAssignmentNode replacingNode)
        {
            switch (node)
            {
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
