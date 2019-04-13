using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using VarLabelPair = System.Tuple<string, string>;
using LinkedListTacNode = System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>;
using VarNodePair = System.Tuple<string, System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>>;


namespace SimpleLang.Optimizations.DefUse
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
        public readonly Dictionary<VarLabelPair, List<string>> Definitions =
            new Dictionary<VarLabelPair, List<string>>();

        public readonly Dictionary<VarNodePair, List<LinkedListTacNode>> DefNodes = new Dictionary<VarNodePair, List<LinkedListTacNode>>();
        /// <summary>
        /// Usages of variables in a basic block
        /// Key: a pair of variable name and label, marking the line in which it is used
        /// Value: a label, marking a line in which current usage is defined
        /// </summary>
        public readonly Dictionary<VarLabelPair, string> Usages = new Dictionary<VarLabelPair, string>();
        public readonly Dictionary<VarNodePair, LinkedListTacNode> UseNodes = new Dictionary<VarNodePair, LinkedListTacNode>();

        public void DetectAndFillDefUse(ThreeAddressCode threeAddressCode)
        {
            // Acquiring a reference to the last Three-address code list node
            var lastNode = threeAddressCode.Last;

            // Temporary usages watcher
            // Key is variable name
            // Value is labels, marking lines of TAC, where they are used
            var tmpUsages = new Dictionary<string, List<string>>();

            var tmpUsagesNodes = new Dictionary<string, List<LinkedListTacNode>>();

            // Reverse-iterating TAC lines list
            while (lastNode != null)
            {
                switch (lastNode.Value)
                {
                    case TacAssignmentNode assignmentNode:
                        {
                            // Registration and pushing to tmpUsages of an assignment TAC operands
                            FillTmpUsagesForNode(assignmentNode.FirstOperand, assignmentNode.Label, tmpUsages);
                            FillTmpUsagesForNode(assignmentNode.FirstOperand, lastNode, tmpUsagesNodes);

                            if (assignmentNode.SecondOperand != null)
                            {
                                FillTmpUsagesForNode(assignmentNode.SecondOperand, assignmentNode.Label, tmpUsages);
                                FillTmpUsagesForNode(assignmentNode.SecondOperand, lastNode, tmpUsagesNodes);
                            }

                            // Creating a new definition entry with the current assignment identifier 
                            var key = new VarLabelPair(assignmentNode.LeftPartIdentifier, assignmentNode.Label);
                            var keyNode = new VarNodePair(assignmentNode.LeftPartIdentifier, lastNode);
                            Definitions[key] = new List<string>();
                            DefNodes[keyNode] = new List<LinkedListTacNode>();

                            // In case, that we already encountered a usage of defined variable
                            if (tmpUsages.ContainsKey(key.Item1))
                            {
                                // Fill a list of usages of a current definition with already registered tmp usages
                                Definitions[key] = tmpUsages[key.Item1];
                                DefNodes[keyNode] = tmpUsagesNodes[keyNode.Item1];

                                // Filling usages of registered tmp labels
                                foreach (var tmp in tmpUsages[key.Item1])
                                {
                                    Usages[new VarLabelPair(assignmentNode.LeftPartIdentifier, tmp)] = assignmentNode.Label;
                                }

                                foreach (var tmp in tmpUsagesNodes[keyNode.Item1])
                                {
                                    UseNodes[new VarNodePair(assignmentNode.LeftPartIdentifier, tmp)] = lastNode;
                                }

                                // Removing a list of usages from tmp registration dictionary
                                tmpUsages.Remove(key.Item1);
                                tmpUsagesNodes.Remove(keyNode.Item1);
                            }

                            break;
                        }
                    case TacIfGotoNode ifGotoNode:
                        {
                            // In goto we encounter only usages of variables, so we fill it accordingly
                            FillTmpUsagesForNode(ifGotoNode.Condition, ifGotoNode.Label, tmpUsages);
                            FillTmpUsagesForNode(ifGotoNode.Condition, lastNode, tmpUsagesNodes);
                            break;
                        }
                }

                lastNode = lastNode.Previous;
            }
            // Fill the rest of usages, that we encounter, definitions of which are absent 
            FillUsagesWithoutDefinitions(tmpUsages);
            FillUsagesWithoutDefinitions(tmpUsagesNodes);
        }

        /// <summary>
        /// Register and fill temporary usages list with a given name-label pair
        /// </summary>
        /// <param name="operand">Current processed operand</param>
        /// <param name="label">Label, marking the line where operand is encountered</param>
        /// <param name="tmpUses">Temporary usages dictionary</param>
        private void FillTmpUsagesForNode(string operand, string label, IDictionary<string, List<string>> tmpUses)
        {
            if (!Utility.Utility.IsVariable(operand)) return;
            if (tmpUses.ContainsKey(operand))
            {
                tmpUses[operand].Add(label);
            } else
            {
                tmpUses[operand] = new List<string>() { label };
            }
        }

        private void FillTmpUsagesForNode(string operand, LinkedListTacNode node, IDictionary<string, List<LinkedListTacNode>> tmpUses)
        {
            if (!Utility.Utility.IsVariable(operand)) return;
            if (tmpUses.ContainsKey(operand))
            {
                tmpUses[operand].Add(node);
            } else
            {
                tmpUses[operand] = new List<LinkedListTacNode>() { node };
            }
        }

        /// <summary>
        /// Traversing the temporary usages list and filling those usages, which definitions are absent
        /// </summary>
        /// <param name="tmpUses">Temporary usages dictionary</param>
        private void FillUsagesWithoutDefinitions(Dictionary<string, List<string>> tmpUses)
        {
            foreach (var tmpUse in tmpUses)
            {
                foreach (var usageLabel in tmpUse.Value)
                {
                    Usages[new VarLabelPair(tmpUse.Key, usageLabel)] = null;
                }
            }
        }

        private void FillUsagesWithoutDefinitions(Dictionary<string, List<LinkedListTacNode>> tmpUses)
        {
            foreach (var tmpUse in tmpUses)
            {
                foreach (var usageTacNode in tmpUse.Value)
                {
                    UseNodes[new VarNodePair(tmpUse.Key, usageTacNode)] = null;
                }
            }
        }

        
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("DEF:\n");

            foreach (var definition in Definitions)
            {
                builder.Append("[" + definition.Key.Item1 + ", " + definition.Key.Item2 + "]: " +
                               (definition.Value.Count > 0
                                   ? definition.Value.Aggregate((source, result) => result + " " + source)
                                   : "no usages") + "\n");
            }

            builder.Append("USE:\n");
            foreach (var usage in Usages)
            {
                builder.Append("[" + usage.Key.Item1 + ", " + usage.Key.Item2 + "]: " +
                               (usage.Value ?? "no definitions") + "\n");
            }

            return builder.ToString();
        }

        public string ToString2()
        {
            var builder = new StringBuilder();
            builder.Append("DEF:\n");

            foreach (var definition in DefNodes)
            {
                builder.Append("[" + definition.Key.Item1 + ", " + definition.Key.Item2.Value.Label + "]: ");
                if (definition.Value.Count > 0)
                {
                    foreach (var elem in definition.Value)
                    {
                        builder.Append(string.Format("{0} ", elem.Value.Label));
                    }
                    builder.Append("\n");
                } else
                {
                    builder.Append("no usages\n");
                }
            }

            builder.Append("USE:\n");
            foreach (var usage in UseNodes)
            {
                builder.Append("[" + usage.Key.Item1 + ", " + usage.Key.Item2.Value.Label + "]: " +
                               (usage.Value == null ? "no definitions" : usage.Value.Value.Label) + "\n");
            }

            return builder.ToString();
        }
    }
}