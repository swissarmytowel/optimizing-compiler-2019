using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Visitors;

namespace UnitTests
{
    public static class Utils
    {
        /// <summary>
        /// Create and push assignment node to the end of the visitor's tac container 
        /// </summary>
        public static void AddAssignmentNode(ThreeAddressCodeVisitor tacVisitor, string label, 
            string id, string firstOp, string op = null, string secondOp = null)
        {
            AddAssignmentNode(tacVisitor.TACodeContainer, label, id, firstOp, op, secondOp);
        }

        /// <summary>
        /// Create and push assignment node to the end of the tac container 
        /// </summary>
        public static void AddAssignmentNode(ThreeAddressCode tacContainer, string label,
            string id, string firstOp, string op = null, string secondOp = null)
        {
            tacContainer.PushNode(new TacAssignmentNode
            {
                Label = label,
                LeftPartIdentifier = id,
                FirstOperand = firstOp,
                Operation = op,
                SecondOperand = secondOp
            });
        }

        /// <summary>
        /// Create and push empty node to the end of the visitor's tac container 
        /// </summary>
        public static void AddEmptyNode(ThreeAddressCodeVisitor tacVisitor)
        {
            AddEmptyNode(tacVisitor.TACodeContainer);
        }

        /// <summary>
        /// Create and push empty node to the end of the tac container 
        /// </summary>
        public static void AddEmptyNode(ThreeAddressCode tacContainer)
        {
            tacContainer.PushNode(new TacEmptyNode());
        }

        /// <summary>
        /// Create and push goto node to the end of the visitor's tac container 
        /// </summary>
        public static void AddGotoNode(ThreeAddressCodeVisitor tacVisitor,
            string label, string targetLabel)
        {
            AddGotoNode(tacVisitor.TACodeContainer, label, targetLabel);
        }

        /// <summary>
        /// Create and push goto node to the end of the tac container 
        /// </summary>
        public static void AddGotoNode(ThreeAddressCode tacContainer, 
            string label, string targetLabel)
        {
            tacContainer.PushNode(new TacGotoNode
            {
                Label = label,
                TargetLabel = targetLabel
            });
        }

        /// <summary>
        /// Create and push if-goto node to the end of the visitor's tac container 
        /// </summary>
        public static void AddIfGotoNode(ThreeAddressCodeVisitor tacVisitor,
            string label, string targetLabel, string condition)
        {
            AddIfGotoNode(tacVisitor.TACodeContainer, label, targetLabel, condition);
        }

        /// <summary>
        /// Create and push if-goto node to the end of the tac container 
        /// </summary>
        public static void AddIfGotoNode(ThreeAddressCode tacContainer, 
            string label, string targetLabel, string condition)
        {
            tacContainer.PushNode(new TacIfGotoNode
            {
                Label = label,
                Condition = condition,
                TargetLabel = targetLabel
            });
        }
    }
}
