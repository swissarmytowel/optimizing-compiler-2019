using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.CFG;
using SimpleLang.TacBasicBlocks;
using SimpleLang.TACode;

namespace UnitTests.CFG
{
    [TestClass]
    public class DepthSpanningTreeTests
    {
        [TestMethod]
        public void Constructor_NestedLoopWithCounter()
        {
            /*
             * a = 1;
             * for (i = 1 to 10)
             *     for (j = 1 to 10)
             *         a = a + 1;
             *
             * VERTICES
             * #0:
             * a = 1  
             * i = 1  
             * 
             * #1:
             * L1: j = 1  
             * 
             * #2:
             * L2: t1 = a + 1
             * a = t1  
             * j = j + 1
             * t2 = j < 10
             * if t2 goto L2
             * 
             * #3:
             * i = i + 1
             * t3 = i < 10
             * if t3 goto L1
             * 
             * EDGES
             * 0 -> [ 1 ]
             * 1 -> [ 2 ]
             * 2 -> [ 3 2 ]
             * 3 -> [ 1 ]
             */

            var tacContainer = Utils.GetNestedLoopWithCounterInTAC();
            var basicBlocks = new BasicBlocks();
            basicBlocks.SplitTACode(tacContainer);

            var cfg = new ControlFlowGraph(tacContainer);
            var dst = new DepthSpanningTree(cfg);
            
            Assert.AreEqual(cfg[0], dst[0]);
            Assert.AreEqual(cfg[1], dst[1]);
            Assert.AreEqual(cfg[2], dst[2]);
            Assert.AreEqual(cfg[3], dst[3]);

            Assert.IsTrue(Utils.IsContainsEdge(dst, dst[0], dst[1]));
            Assert.IsTrue(Utils.IsContainsEdge(dst, dst[1], dst[2]));
            Assert.IsTrue(Utils.IsContainsEdge(dst, dst[2], dst[3]));

            /*
             * VERTICES
             * #0:
             * a = 1
             * i = 1
             * 
             * #1:
             * L1: j = 1
             * 
             * #2:
             * L2: t1 = a + 1
             * a = t1
             * j = j + 1
             * t2 = j < 10
             * if t2 goto L2
             * 
             * #3:
             * i = i + 1
             * t3 = i < 10
             * if t3 goto L1
             * 
             * EDGES
             * 0 -> [ 1 ]
             * 1 -> [ 2 ]
             * 2 -> [ 3 ]
             * 3 -> [ ]
             */
        }

        [TestMethod]
        public void Rebuild_IfWithNestedLoopAndNestedIf()
        {
            var tacContainer0 = Utils.GetNestedLoopWithCounterInTAC();
            var basicBlocks0 = new BasicBlocks();
            basicBlocks0.SplitTACode(tacContainer0);

            /*
             * VERTICES
             * #0:
             * t1 = a < 5
             * if t1 goto L1
             * 
             * #1:
             * t2 = a > 10
             * if t2 goto L3
             * 
             * #2:
             * goto L4
             * 
             * #3:
             * L3: t3 = a - 10
             * a = t3  
             * 
             * #4:
             * L4: goto L2
             * 
             * #5:
             * L1: j = 1  
             * 
             * #6:
             * L5: t4 = a + 1
             * a = t4  
             * j = j + 1
             * t5 = j < 10
             * if t5 goto L5
             * 
             * #7:
             * L2: p = 2  
             * 
             * EDGES
             * 0 -> [ 1 5 ]
             * 1 -> [ 2 3 ]
             * 2 -> [ 4 ]
             * 3 -> [ 4 ]
             * 4 -> [ 7 ]
             * 5 -> [ 6 ]
             * 6 -> [ 7 6 ]
             * 7 -> [ ]
             *
             */

            var tacContainer1 = new ThreeAddressCode();
            // basic block #0
            Utils.AddAssignmentNode(tacContainer1, null, "t1", "a", "<", "5");
            Utils.AddIfGotoNode(tacContainer1, null, "L1", "t1");
            // basic block #1
            Utils.AddAssignmentNode(tacContainer1, null, "t2", "a", ">", "10");
            Utils.AddIfGotoNode(tacContainer1, null, "L3", "t2");
            // basic block #2
            Utils.AddGotoNode(tacContainer1, null, "L4");
            // basic block #3
            Utils.AddAssignmentNode(tacContainer1, "L3", "t3", "a", "-", "10");
            Utils.AddAssignmentNode(tacContainer1, null, "a", "t3");
            // basic block #4
            Utils.AddGotoNode(tacContainer1, "L4", "L2");
            // basic block #5
            Utils.AddAssignmentNode(tacContainer1, "L1", "j", "1");
            // basic block #6
            Utils.AddAssignmentNode(tacContainer1, "L5", "t4", "a", "+", "1");
            Utils.AddAssignmentNode(tacContainer1, null, "a", "t4");
            Utils.AddAssignmentNode(tacContainer1, null, "j", "j", "+", "1");
            Utils.AddAssignmentNode(tacContainer1, null, "t5", "j", "<", "10");
            Utils.AddIfGotoNode(tacContainer1, null, "L5", "t5");
            // basic block #7
            Utils.AddAssignmentNode(tacContainer1, "L2", "p", "2");
            var basicBlocks1 = new BasicBlocks();
            basicBlocks1.SplitTACode(tacContainer1);

            var cfg0 = new ControlFlowGraph(tacContainer0);
            var cfg1 = new ControlFlowGraph(tacContainer1);
            var dst = new DepthSpanningTree(cfg0);
            dst.Rebuild(cfg1);

            /*
             * VERTICES
             * #0:
             * t1 = a < 5
             * if t1 goto L1
             * 
             * #1:
             * L1: j = 1  
             * 
             * #2:
             * L5: t4 = a + 1
             * a = t4  
             * j = j + 1
             * t5 = j < 10
             * if t5 goto L5
             * 
             * #3:
             * L2: p = 2  
             * 
             * #4:
             * t2 = a > 10
             * if t2 goto L3
             * 
             * #5:
             * L3: t3 = a - 10
             * a = t3  
             * 
             * #6:
             * L4: goto L2
             * 
             * #7:
             * goto L4
             * 
             * EDGES
             * 0 -> [ 4 1 ]
             * 1 -> [ 2 ]
             * 2 -> [ 3 ]
             * 3 -> [ ]
             * 4 -> [ 7 5 ]
             * 5 -> [ 6 ]
             * 6 -> [ ]
             * 7 -> [ ]
             */

            Assert.AreEqual(cfg1[0], dst[0]);
            Assert.AreEqual(cfg1[1], dst[4]);
            Assert.AreEqual(cfg1[2], dst[7]);
            Assert.AreEqual(cfg1[3], dst[5]);
            Assert.AreEqual(cfg1[4], dst[6]);
            Assert.AreEqual(cfg1[5], dst[1]);
            Assert.AreEqual(cfg1[6], dst[2]);
            Assert.AreEqual(cfg1[7], dst[3]);

            Assert.IsTrue(Utils.IsContainsEdge(dst, dst[0], dst[1]));
            Assert.IsTrue(Utils.IsContainsEdge(dst, dst[0], dst[4]));
            Assert.IsTrue(Utils.IsContainsEdge(dst, dst[1], dst[2]));
            Assert.IsTrue(Utils.IsContainsEdge(dst, dst[2], dst[3]));
            Assert.IsTrue(Utils.IsContainsEdge(dst, dst[4], dst[5]));
            Assert.IsTrue(Utils.IsContainsEdge(dst, dst[4], dst[7]));
            Assert.IsTrue(Utils.IsContainsEdge(dst, dst[5], dst[6]));
        }
    }
}
