using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.CFG;
using SimpleLang.TACode;
using SimpleLang.TacBasicBlocks;

namespace UnitTests.CFG
{
    [TestClass]
    public class ControlFlowGraphTests
    {
        [TestMethod]
        public void Constructor_EmptyOrNullSourceCode()
        {
            var tacContainer0 = new ThreeAddressCode();
            ThreeAddressCode tacContainer1 = null;

            var cfg0 = new ControlFlowGraph(tacContainer0);
            var cfg1 = new ControlFlowGraph(tacContainer1);

            CheckGraphForEmptiness(cfg0);
            CheckGraphForEmptiness(cfg1);
        }

        [TestMethod]
        public void Constructor_NestedLoopWithCounter()
        {
            /*
             * a = 1;
             * for (i = 1 to 10)
             *     for (j = 1 to 10)
             *         a = a + 1;
             */

            var tacContainer = Utils.GetNestedLoopWithCounterInTAC();
            var basicBlocks = new BasicBlocks();
            basicBlocks.SplitTACode(tacContainer);

            var cfg = new ControlFlowGraph(tacContainer);

            Assert.IsFalse(cfg.IsEdgesEmpty);
            Assert.IsFalse(cfg.IsVerticesEmpty);

            Assert.AreEqual(cfg.EdgeCount, 5);
            Assert.AreEqual(cfg.VertexCount, basicBlocks.BasicBlockItems.Count);

            Assert.AreEqual(cfg.Edges.Count(), 5);
            Assert.AreEqual(cfg.Vertices.Count(), basicBlocks.BasicBlockItems.Count);

            Assert.IsNotNull(cfg.SourceBasicBlocks);
            Assert.AreEqual(cfg.EntryBlock.ToString(), basicBlocks.BasicBlockItems.First().ToString());
            Assert.AreEqual(cfg.ExitBlock.ToString(), basicBlocks.BasicBlockItems.Last().ToString());

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

            Assert.IsTrue(Utils.IsContainsEdge(cfg, cfg[0], cfg[1]));
            Assert.IsTrue(Utils.IsContainsEdge(cfg, cfg[1], cfg[2]));
            Assert.IsTrue(Utils.IsContainsEdge(cfg, cfg[2], cfg[2]));
            Assert.IsTrue(Utils.IsContainsEdge(cfg, cfg[2], cfg[3]));
            Assert.IsTrue(Utils.IsContainsEdge(cfg, cfg[3], cfg[1]));
        }

        [TestMethod]
        public void Rebuild_EmptyOrNullSourceCode()
        {
            var tacContainer0 = Utils.GetNestedLoopWithCounterInTAC();
            var tacContainer1 = new ThreeAddressCode();
            ThreeAddressCode tacContainer2 = null;

            var cfg0 = new ControlFlowGraph(tacContainer0);
            var cfg1 = new ControlFlowGraph(tacContainer0);

            cfg0.Rebuild(tacContainer1);
            cfg1.Rebuild(tacContainer2);

            CheckGraphForEmptiness(cfg0);
            CheckGraphForEmptiness(cfg1);
        }

        [TestMethod]
        public void Rebuild_SingleBasicBlock()
        {
            var tacContainer0 = Utils.GetNestedLoopWithCounterInTAC();
            var tacContainer1 = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer1, null, "a", "1");
            Utils.AddAssignmentNode(tacContainer1, null, "b", "10");
            Utils.AddAssignmentNode(tacContainer1, null, "c", "a", "+", "b");

            var cfg = new ControlFlowGraph(tacContainer0);

            cfg.Rebuild(tacContainer1);

            Assert.IsTrue(cfg.IsEdgesEmpty);
            Assert.IsFalse(cfg.IsVerticesEmpty);

            Assert.AreEqual(cfg.EdgeCount, 0);
            Assert.AreEqual(cfg.VertexCount, 1);

            Assert.AreEqual(cfg.Edges.Count(), 0);
            Assert.AreEqual(cfg.Vertices.Count(), 1);

            Assert.IsNotNull(cfg.SourceBasicBlocks);
            Assert.AreEqual(cfg.EntryBlock.ToString(), tacContainer1.ToString());
            Assert.AreEqual(cfg.ExitBlock.ToString(), tacContainer1.ToString());
        }

        private static void CheckGraphForEmptiness(ControlFlowGraph cfg)
        {
            Assert.IsTrue(cfg.IsEdgesEmpty);
            Assert.IsTrue(cfg.IsVerticesEmpty);

            Assert.AreEqual(cfg.EdgeCount, 0);
            Assert.AreEqual(cfg.VertexCount, 0);

            Assert.AreEqual(cfg.Edges.Count(), 0);
            Assert.AreEqual(cfg.Vertices.Count(), 0);

            Assert.IsNull(cfg.EntryBlock);
            Assert.IsNull(cfg.ExitBlock);

            if (cfg.SourceCode != null)
                Assert.AreEqual(cfg.SourceCode.TACodeLines.Count, 0);

            Assert.AreEqual(cfg.ToString(), "Empty graph.");
        }
    }
}
