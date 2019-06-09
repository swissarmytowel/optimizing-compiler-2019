using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.CFG;
using SimpleLang.GenKill.Implementations;
using SimpleLang.IterationAlgorithms;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace UnitTests.IterationAlgorithms
{
    [TestClass]
    public class ReachingDefinitionsTests
    {
        [TestMethod]
        public void Execute_NestedLoopWithCounter()
        {
            /*
             * a = 1;
             * for (i = 1 to 10)
             *     for (j = 1 to 10)
             *         a = a + 1;
             */

            var tacContainer = Utils.GetNestedLoopWithCounterInTAC();
            var cfg = new ControlFlowGraph(tacContainer);

            var genKillVisitor = new GenKillVisitor();
            var ita = new ReachingDefinitionsITA(cfg, 
                genKillVisitor.GenerateReachingDefinitionForBlocks(cfg.SourceBasicBlocks));
            var inData = ita.InOut.In;
            var outData = ita.InOut.Out;

            Assert.AreEqual(inData[cfg[0]].Count, 0);
            Assert.AreEqual(outData[cfg[0]].Count, 2);
            Assert.AreEqual(inData[cfg[1]].Count, 8);
            Assert.AreEqual(outData[cfg[1]].Count, 8);
            Assert.AreEqual(inData[cfg[2]].Count, 9);
            Assert.AreEqual(outData[cfg[2]].Count, 7);
            Assert.AreEqual(inData[cfg[3]].Count, 7);
            Assert.AreEqual(outData[cfg[3]].Count, 6);

            Assert.IsTrue(outData[cfg[0]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[0]].Contains(cfg[0, 1]));

            Assert.IsTrue(inData[cfg[1]].Contains(cfg[0, 0]));
            Assert.IsTrue(inData[cfg[1]].Contains(cfg[0, 1]));
            Assert.IsTrue(inData[cfg[1]].Contains(cfg[2, 0]));
            Assert.IsTrue(inData[cfg[1]].Contains(cfg[2, 1]));
            Assert.IsTrue(inData[cfg[1]].Contains(cfg[2, 2]));
            Assert.IsTrue(inData[cfg[1]].Contains(cfg[2, 3]));
            Assert.IsTrue(inData[cfg[1]].Contains(cfg[3, 0]));
            Assert.IsTrue(inData[cfg[1]].Contains(cfg[3, 1]));
            Assert.IsTrue(outData[cfg[1]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[1]].Contains(cfg[0, 1]));
            Assert.IsTrue(outData[cfg[1]].Contains(cfg[1, 0]));
            Assert.IsTrue(outData[cfg[1]].Contains(cfg[2, 0]));
            Assert.IsTrue(outData[cfg[1]].Contains(cfg[2, 1]));
            Assert.IsTrue(outData[cfg[1]].Contains(cfg[2, 3]));
            Assert.IsTrue(outData[cfg[1]].Contains(cfg[3, 0]));
            Assert.IsTrue(outData[cfg[1]].Contains(cfg[3, 1]));

            Assert.IsTrue(inData[cfg[2]].Contains(cfg[0, 0]));
            Assert.IsTrue(inData[cfg[2]].Contains(cfg[0, 1]));
            Assert.IsTrue(inData[cfg[2]].Contains(cfg[1, 0]));
            Assert.IsTrue(inData[cfg[2]].Contains(cfg[2, 0]));
            Assert.IsTrue(inData[cfg[2]].Contains(cfg[2, 1]));
            Assert.IsTrue(inData[cfg[2]].Contains(cfg[2, 2]));
            Assert.IsTrue(inData[cfg[2]].Contains(cfg[2, 3]));
            Assert.IsTrue(inData[cfg[2]].Contains(cfg[3, 0]));
            Assert.IsTrue(inData[cfg[2]].Contains(cfg[3, 1]));
            Assert.IsTrue(outData[cfg[2]].Contains(cfg[0, 1]));
            Assert.IsTrue(outData[cfg[2]].Contains(cfg[2, 0]));
            Assert.IsTrue(outData[cfg[2]].Contains(cfg[2, 1]));
            Assert.IsTrue(outData[cfg[2]].Contains(cfg[2, 2]));
            Assert.IsTrue(outData[cfg[2]].Contains(cfg[2, 3]));
            Assert.IsTrue(outData[cfg[2]].Contains(cfg[3, 0]));
            Assert.IsTrue(outData[cfg[2]].Contains(cfg[3, 1]));

            Assert.IsTrue(inData[cfg[3]].Contains(cfg[0, 1]));
            Assert.IsTrue(inData[cfg[3]].Contains(cfg[2, 0]));
            Assert.IsTrue(inData[cfg[3]].Contains(cfg[2, 1]));
            Assert.IsTrue(inData[cfg[3]].Contains(cfg[2, 2]));
            Assert.IsTrue(inData[cfg[3]].Contains(cfg[2, 3]));
            Assert.IsTrue(inData[cfg[3]].Contains(cfg[3, 0]));
            Assert.IsTrue(inData[cfg[3]].Contains(cfg[3, 1]));
            Assert.IsTrue(outData[cfg[3]].Contains(cfg[2, 0]));
            Assert.IsTrue(outData[cfg[3]].Contains(cfg[2, 1]));
            Assert.IsTrue(outData[cfg[3]].Contains(cfg[2, 2]));
            Assert.IsTrue(outData[cfg[3]].Contains(cfg[2, 3]));
            Assert.IsTrue(outData[cfg[3]].Contains(cfg[3, 0]));
            Assert.IsTrue(outData[cfg[3]].Contains(cfg[3, 1]));
        }

        [TestMethod]
        public void Execute_IfWithLoopAndNestedIf()
        {
            /* 
             * if (a < 5)
             *     for (j = 1 to 10)
             *         a = a + 1;
             * else
             *     if (a > 10)
             *         a = a - 10;
             * p = 2;
             */

            var tacContainer = new ThreeAddressCode();

            // basic block #0
            Utils.AddAssignmentNode(tacContainer, null, "t1", "a", "<", "5");
            Utils.AddIfGotoNode(tacContainer, null, "L1", "t1");
            // basic block #1
            Utils.AddAssignmentNode(tacContainer, null, "t2", "a", ">", "10");
            Utils.AddIfGotoNode(tacContainer, null, "L3", "t2");
            // basic block #2
            Utils.AddGotoNode(tacContainer, null, "L4");
            // basic block #3
            Utils.AddAssignmentNode(tacContainer, "L3", "t3", "a", "-", "10");
            Utils.AddAssignmentNode(tacContainer, null, "a", "t3");
            // basic block #4
            Utils.AddGotoNode(tacContainer, "L4", "L2");
            // basic block #5
            Utils.AddAssignmentNode(tacContainer, "L1", "j", "1");
            // basic block #6
            Utils.AddAssignmentNode(tacContainer, "L5", "t4", "a", "+", "1");
            Utils.AddAssignmentNode(tacContainer, null, "a", "t4");
            Utils.AddAssignmentNode(tacContainer, null, "j", "j", "+", "1");
            Utils.AddAssignmentNode(tacContainer, null, "t5", "j", "<", "10");
            Utils.AddIfGotoNode(tacContainer, null, "L5", "t5");
            // basic block #7
            Utils.AddAssignmentNode(tacContainer, "L2", "p", "2");

            var cfg = new ControlFlowGraph(tacContainer);

            var genKillVisitor = new GenKillVisitor();
            var ita = new ReachingDefinitionsITA(cfg,
                genKillVisitor.GenerateReachingDefinitionForBlocks(cfg.SourceBasicBlocks));
            var inData = ita.InOut.In;
            var outData = ita.InOut.Out;

            Assert.AreEqual(inData[cfg[0]].Count, 0);
            Assert.AreEqual(outData[cfg[0]].Count, 1);
            Assert.AreEqual(inData[cfg[1]].Count, 1);
            Assert.AreEqual(outData[cfg[1]].Count, 2);
            Assert.AreEqual(inData[cfg[2]].Count, 2);
            Assert.AreEqual(outData[cfg[2]].Count, 2);
            Assert.AreEqual(inData[cfg[3]].Count, 2);
            Assert.AreEqual(outData[cfg[3]].Count, 4);
            Assert.AreEqual(inData[cfg[4]].Count, 4);
            Assert.AreEqual(outData[cfg[4]].Count, 4);
            Assert.AreEqual(inData[cfg[5]].Count, 1);
            Assert.AreEqual(outData[cfg[5]].Count, 2);
            Assert.AreEqual(inData[cfg[6]].Count, 6);
            Assert.AreEqual(outData[cfg[6]].Count, 5);
            Assert.AreEqual(inData[cfg[7]].Count, 8);
            Assert.AreEqual(outData[cfg[7]].Count, 9);

            Assert.IsTrue(outData[cfg[0]].Contains(cfg[0, 0]));

            Assert.IsTrue(inData[cfg[1]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[1]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[1]].Contains(cfg[1, 0]));

            Assert.IsTrue(inData[cfg[2]].Contains(cfg[0, 0]));
            Assert.IsTrue(inData[cfg[2]].Contains(cfg[1, 0]));
            Assert.IsTrue(outData[cfg[2]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[2]].Contains(cfg[1, 0]));

            Assert.IsTrue(inData[cfg[3]].Contains(cfg[0, 0]));
            Assert.IsTrue(inData[cfg[3]].Contains(cfg[1, 0]));
            Assert.IsTrue(outData[cfg[3]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[3]].Contains(cfg[1, 0]));
            Assert.IsTrue(outData[cfg[3]].Contains(cfg[3, 0]));
            Assert.IsTrue(outData[cfg[3]].Contains(cfg[3, 1]));

            Assert.IsTrue(inData[cfg[4]].Contains(cfg[0, 0]));
            Assert.IsTrue(inData[cfg[4]].Contains(cfg[1, 0]));
            Assert.IsTrue(inData[cfg[4]].Contains(cfg[3, 0]));
            Assert.IsTrue(inData[cfg[4]].Contains(cfg[3, 1]));
            Assert.IsTrue(outData[cfg[4]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[4]].Contains(cfg[1, 0]));
            Assert.IsTrue(outData[cfg[4]].Contains(cfg[3, 0]));
            Assert.IsTrue(outData[cfg[4]].Contains(cfg[3, 1]));

            Assert.IsTrue(inData[cfg[5]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[5]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[5]].Contains(cfg[5, 0]));

            Assert.IsTrue(inData[cfg[6]].Contains(cfg[0, 0]));
            Assert.IsTrue(inData[cfg[6]].Contains(cfg[5, 0]));
            Assert.IsTrue(inData[cfg[6]].Contains(cfg[6, 0]));
            Assert.IsTrue(inData[cfg[6]].Contains(cfg[6, 1]));
            Assert.IsTrue(inData[cfg[6]].Contains(cfg[6, 2]));
            Assert.IsTrue(inData[cfg[6]].Contains(cfg[6, 3]));
            Assert.IsTrue(outData[cfg[6]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[6]].Contains(cfg[6, 0]));
            Assert.IsTrue(outData[cfg[6]].Contains(cfg[6, 1]));
            Assert.IsTrue(outData[cfg[6]].Contains(cfg[6, 2]));
            Assert.IsTrue(outData[cfg[6]].Contains(cfg[6, 3]));

            Assert.IsTrue(inData[cfg[7]].Contains(cfg[0, 0]));
            Assert.IsTrue(inData[cfg[7]].Contains(cfg[1, 0]));
            Assert.IsTrue(inData[cfg[7]].Contains(cfg[3, 0]));
            Assert.IsTrue(inData[cfg[7]].Contains(cfg[3, 1]));
            Assert.IsTrue(inData[cfg[7]].Contains(cfg[6, 0]));
            Assert.IsTrue(inData[cfg[7]].Contains(cfg[6, 1]));
            Assert.IsTrue(inData[cfg[7]].Contains(cfg[6, 2]));
            Assert.IsTrue(inData[cfg[7]].Contains(cfg[6, 3]));
            Assert.IsTrue(outData[cfg[7]].Contains(cfg[0, 0]));
            Assert.IsTrue(outData[cfg[7]].Contains(cfg[1, 0]));
            Assert.IsTrue(outData[cfg[7]].Contains(cfg[3, 0]));
            Assert.IsTrue(outData[cfg[7]].Contains(cfg[3, 1]));
            Assert.IsTrue(outData[cfg[7]].Contains(cfg[6, 0]));
            Assert.IsTrue(outData[cfg[7]].Contains(cfg[6, 1]));
            Assert.IsTrue(outData[cfg[7]].Contains(cfg[6, 2]));
            Assert.IsTrue(outData[cfg[7]].Contains(cfg[6, 3]));
            Assert.IsTrue(outData[cfg[7]].Contains(cfg[7, 0]));
        }
    }
}
