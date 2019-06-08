using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.CFG;
using SimpleLang.GenKill.Implementations;
using SimpleLang.IterationAlgorithms;

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
    }
}
