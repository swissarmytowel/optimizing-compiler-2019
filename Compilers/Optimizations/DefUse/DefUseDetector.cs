using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations.DefUse
{
    public class DefUseDetector
    {
        public readonly Dictionary<string, string> Definitions = new Dictionary<string, string>();
        public readonly Dictionary<string, List<string>> Usages = new Dictionary<string, List<string>>();

        public void DetectAndFillDefUse(ThreeAddressCode threeAddressCode)
        {
            foreach (var node in threeAddressCode)
            {
                switch (node)
                {
                    case TacAssignmentNode assignmentNode:
                    {
                        Definitions[assignmentNode.LeftPartIdentifier] = assignmentNode.Label;

                        AddSingleAssignmentUsageEntry(assignmentNode.FirstOperand, assignmentNode.Label);
                        if (assignmentNode.SecondOperand != null)
                        {
                            var isNotConstant = !int.TryParse(assignmentNode.SecondOperand, out _)
                                                && !double.TryParse(assignmentNode.SecondOperand, out _);
                            if (isNotConstant)
                                AddSingleAssignmentUsageEntry(assignmentNode.SecondOperand, assignmentNode.Label);
                        }

                        break;
                    }
                    case TacIfGotoNode ifGotoNode:
                    {
                        if (Usages.ContainsKey(ifGotoNode.Condition))
                        {
                            Usages[ifGotoNode.Condition].Add(ifGotoNode.Label);
                        }
                        else
                        {
                            Usages[ifGotoNode.Condition] = new List<string>() {ifGotoNode.Label};
                        }

                        break;
                    }
                    default:
                        break;
                }
            }
        }

        private void AddSingleAssignmentUsageEntry(string operand, string label)
        {
            if (Usages.ContainsKey(operand))
            {
                Usages[operand].Add(label);
            }
            else
            {
                Usages[operand] = new List<string>() {label};
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("DEF:\n");
            foreach (var definition in Definitions)
            {
                builder.Append(definition.Key + ": " + definition.Value + "\n");
            }

            builder.Append("USE:\n");
            foreach (var usage in Usages)
            {
                builder.Append(usage.Key + ": " + usage.Value.Aggregate((source, result) => result + " " + source) +
                               "\n");
            }

            return builder.ToString();
        }
    }
}