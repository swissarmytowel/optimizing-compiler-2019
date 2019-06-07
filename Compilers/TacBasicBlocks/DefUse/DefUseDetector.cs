using System.Collections.Generic;
using System.Text;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using VarLabelPair = System.Tuple<string, string>;
using LinkedListTacNode = System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>;
using VarNodePair = System.Tuple<string, System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>>;


namespace SimpleLang.TacBasicBlocks.DefUse
{
    /// <summary>
    /// Detects definitions and usages of three-address code variables in a basic block
    /// </summary>
    public class DefUseDetector
    {
        /// <summary>
        /// Definitions of variables in a basic block
        /// Key: a pair of variable name and label, marking the line in which it is defined
        /// Value: a list of labels, each marking a line of code, in which usage of a current definition occurs
        /// </summary>
        public readonly Dictionary<VarNodePair, List<LinkedListTacNode>> Definitions =
            new Dictionary<VarNodePair, List<LinkedListTacNode>>();

        /// <summary>
        /// Usages of variables in a basic block
        /// Key: a pair of variable name and label, marking the line in which it is used
        /// Value: a label, marking a line in which current usage is defined
        /// </summary>
        public readonly Dictionary<VarNodePair, LinkedListTacNode> Usages =
            new Dictionary<VarNodePair, LinkedListTacNode>();

        public void DetectAndFillDefUse(ThreeAddressCode threeAddressCode)
        {
            // Acquiring a reference to the last Three-address code list node
            var lastNode = threeAddressCode.Last;

            // Temporary usages watcher
            // Key is variable name
            // Value is labels, marking lines of TAC, where they are used
            var tmpUsagesNodes = new Dictionary<string, List<LinkedListTacNode>>();

            // Reverse-iterating TAC lines list
            while (lastNode != null)
            {
                switch (lastNode.Value)
                {
                    case TacAssignmentNode assignmentNode:
                    {
                        // Registration and pushing to tmpUsages of an assignment TAC operands
                        FillTmpUsagesForNode(assignmentNode.FirstOperand, lastNode, tmpUsagesNodes);

                        if (assignmentNode.SecondOperand != null)
                        {
                            FillTmpUsagesForNode(assignmentNode.SecondOperand, lastNode, tmpUsagesNodes);
                        }

                        // Creating a new definition entry with the current assignment identifier 
                        var keyNode = new VarNodePair(assignmentNode.LeftPartIdentifier, lastNode);
                        Definitions[keyNode] = new List<LinkedListTacNode>();

                        // In case, that we already encountered a usage of defined variable
                        if (tmpUsagesNodes.ContainsKey(keyNode.Item1))
                        {
                            // Fill a list of usages of a current definition with already registered tmp usages
                            Definitions[keyNode] = tmpUsagesNodes[keyNode.Item1];

                            // Filling usages of registered tmp labels
                            foreach (var tmp in tmpUsagesNodes[keyNode.Item1])
                            {
                                Usages[new VarNodePair(assignmentNode.LeftPartIdentifier, tmp)] = lastNode;
                            }

                            // Removing a list of usages from tmp registration dictionary
                            tmpUsagesNodes.Remove(keyNode.Item1);
                        }

                        break;
                    }
                    case TacIfGotoNode ifGotoNode:
                    {
                        // In goto we encounter only usages of variables, so we fill it accordingly
                        FillTmpUsagesForNode(ifGotoNode.Condition, lastNode, tmpUsagesNodes);
                        break;
                    }
                }

                lastNode = lastNode.Previous;
            }

            // Fill the rest of usages, that we encounter, definitions of which are absent 
            FillUsagesWithoutDefinitions(tmpUsagesNodes);
        }

        /// <summary>
        /// Register and fill temporary usages list with a given name-label pair
        /// </summary>
        /// <param name="operand">Current processed operand</param>
        /// <param name="node"></param>
        /// <param name="tmpUses">Temporary usages dictionary</param>
        private void FillTmpUsagesForNode(string operand, LinkedListTacNode node,
            IDictionary<string, List<LinkedListTacNode>> tmpUses)
        {
            if (!Utility.Utility.IsVariable(operand)) return;
            if (tmpUses.ContainsKey(operand))
            {
                tmpUses[operand].Add(node);
            }
            else
            {
                tmpUses[operand] = new List<LinkedListTacNode>() {node};
            }
        }

        /// <summary>
        /// Traversing the temporary usages list and filling those usages, which definitions are absent
        /// </summary>
        /// <param name="tmpUses">Temporary usages dictionary</param>
        private void FillUsagesWithoutDefinitions(Dictionary<string, List<LinkedListTacNode>> tmpUses)
        {
            foreach (var tmpUse in tmpUses)
            {
                foreach (var usageTacNode in tmpUse.Value)
                {
                    Usages[new VarNodePair(tmpUse.Key, usageTacNode)] = null;
                }
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("DEF:\n");

            foreach (var definition in Definitions)
            {
                builder.Append("[" + definition.Key.Item1 + ", " + definition.Key.Item2.Value.Label + "]: ");
                if (definition.Value.Count > 0)
                {
                    foreach (var elem in definition.Value)
                    {
                        builder.Append($"{elem.Value.Label} ");
                    }

                    builder.Append("\n");
                }
                else
                {
                    builder.Append("no usages\n");
                }
            }

            builder.Append("USE:\n");
            foreach (var usage in Usages)
            {
                builder.Append("[" + usage.Key.Item1 + ", " + usage.Key.Item2.Value.Label + "]: " +
                               (usage.Value == null ? "no definitions" : usage.Value.Value.Label) + "\n");
            }

            return builder.ToString();
        }
    }
}