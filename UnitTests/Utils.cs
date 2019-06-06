using System.Linq;
using SimpleLang.CFG;
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

        /// <summary>
        /// Check whether the graph contains an edge with specified source and target
        /// </summary>
        public static bool IsContainsEdge(BidirectionalGraph graph,
            ThreeAddressCode source, ThreeAddressCode target) =>
            graph.Edges.Any(edge =>
                edge.Source.ToString() == source.ToString() &&
                edge.Target.ToString() == target.ToString());

        /// <summary>
        /// Return the code containing nested loop with counter converted to TAC
        /// </summary>
        public static ThreeAddressCode GetNestedLoopWithCounterInTAC()
        {
            /*
             * a = 1;
             * for (i = 1 to 10)
             *     for (j = 1 to 10)
             *         a = a + 1;
             */

            var tacContainer = new ThreeAddressCode();

            // basic block #0
            AddAssignmentNode(tacContainer, null, "a", "1");
            AddAssignmentNode(tacContainer, null, "i", "1");
            // basic block #1
            AddAssignmentNode(tacContainer, "L1", "j", "1");
            // basic block #2
            AddAssignmentNode(tacContainer, "L2", "t1", "a", "+", "1");
            AddAssignmentNode(tacContainer, null, "a", "t1");
            AddAssignmentNode(tacContainer, null, "j", "j", "+", "1");
            AddAssignmentNode(tacContainer, null, "t2", "j", "<", "10");
            AddIfGotoNode(tacContainer, null, "L2", "t2");
            // basic block #3
            AddAssignmentNode(tacContainer, null, "i", "i", "+", "1");
            AddAssignmentNode(tacContainer, null, "t3", "i", "<", "10");
            AddIfGotoNode(tacContainer, null, "L1", "t3");

            return tacContainer;
        }
    }
}
