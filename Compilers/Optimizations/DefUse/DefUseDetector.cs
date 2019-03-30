using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using VarLabelPair = System.Tuple<string, string>;

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
        /// <summary>
        /// Usages of variables in a basic block
        /// Key: a pair of variable name and label, marking the line in which it is used
        /// Value: a label, marking a line in which current usage is defined
        /// </summary>
        public readonly Dictionary<VarLabelPair, string> Usages = new Dictionary<VarLabelPair, string>();

        public void DetectAndFillDefUse(ThreeAddressCode threeAddressCode)
        {
            // Acquiring a reference to the last Three-address code list node
            var lastNode = threeAddressCode.Last;
            
            // Temporary usages watcher
            // Key is variable name
            // Value is labels, marking lines of TAC, where they are used
            var tmpUsages = new Dictionary<string, List<string>>();

            // Reverse-iterating TAC lines list
            while (lastNode != null)
            {
                switch (lastNode.Value)
                {
                    case TacAssignmentNode assignmentNode:
                    {
                        // Registration and pushing to tmpUsages of an assignment TAC operands
                        FillTmpUsagesForNode(assignmentNode.FirstOperand, assignmentNode.Label, tmpUsages);

                        if (assignmentNode.SecondOperand != null)
                        {
                            FillTmpUsagesForNode(assignmentNode.SecondOperand, assignmentNode.Label, tmpUsages);
                        }
                        
                        // Creating a new definition entry with the current assignment identifier 
                        var key = new VarLabelPair(assignmentNode.LeftPartIdentifier, assignmentNode.Label);
                        Definitions[key] = new List<string>();
                        
                        // In case, that we already encountered a usage of defined variable
                        if (tmpUsages.ContainsKey(key.Item1))
                        {
                            // Fill a list of usages of a current definition with already registered tmp usages
                            Definitions[key] = tmpUsages[key.Item1];
                            
                            // Filling usages of registered tmp labels
                            foreach (var tmp in tmpUsages[key.Item1])
                            {
                                Usages[new VarLabelPair(assignmentNode.LeftPartIdentifier, tmp)] = assignmentNode.Label;
                            }
                            
                            // Removing a list of usages from tmp registration dictionary
                            tmpUsages.Remove(key.Item1);
                        }

                        break;
                    }
                    case TacIfGotoNode ifGotoNode:
                    {
                        // In goto we encounter only usages of variables, so we fill it accordingly
                        FillTmpUsagesForNode(ifGotoNode.Condition, ifGotoNode.Label, tmpUsages);
                        break;
                    }
                }

                lastNode = lastNode.Previous;
            }
            // Fill the rest of usages, that we encounter, definitions of which are absent 
            FillUsagesWithoutDefinitions(tmpUsages);
        }

        /// <summary>
        /// Register and fill temporary usages list with a given name-label pair
        /// </summary>
        /// <param name="operand">Current processed operand</param>
        /// <param name="label">Label, marking the line where operand is encountered</param>
        /// <param name="tmpUses">Temporary usages dictionary</param>
        private void FillTmpUsagesForNode(string operand, string label, IDictionary<string, List<string>> tmpUses)
        {
            if (!IsVariable(operand)) return;
            if (tmpUses.ContainsKey(operand))
            {
                tmpUses[operand].Add(label);
            }
            else
            {
                tmpUses[operand] = new List<string>() {label};
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

        /// <summary>
        /// Checking if an operand (expression) is a variable, not a bool, int or double value
        /// </summary>
        /// <param name="expression">operand to be checked</param>
        /// <returns>If operand is a variable</returns>
        private bool IsVariable(string expression) => int.TryParse(expression, out _) == false
                                                      && double.TryParse(expression, out _) == false
                                                      && bool.TryParse(expression, out _) == false;

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
    }
}