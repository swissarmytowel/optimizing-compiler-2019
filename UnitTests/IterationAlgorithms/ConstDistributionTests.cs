using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.CFG;
using SimpleLang.ConstDistrib;
using SimpleLang.IterationAlgorithms;
using SimpleLang.TACode;

namespace UnitTests.IterationAlgorithms
{
    [TestClass]
    public class ConstDistributionTests
    {
        [TestMethod]
        public void Constructor_GotoWithPossibleConstDistribution()
        {
            /*
             * a = 15;
             * c = a + d;
             * goto l 1:
             * l 1: b = a
             */

            var tacContainer = new ThreeAddressCode();

            // basic block #0
            Utils.AddAssignmentNode(tacContainer, null, "a", "15");
            Utils.AddAssignmentNode(tacContainer, null, "c", "a", "+", "d");
            Utils.AddGotoNode(tacContainer, null, "l1");
            // basic block #1
            Utils.AddAssignmentNode(tacContainer, "l1", "b", "a");

            var cfg = new ControlFlowGraph(tacContainer);

            var ita = new ConstDistributionITA(cfg);
            var inData = ita.InOut.In;
            var outData = ita.InOut.Out;

            Assert.AreEqual(inData[cfg[0]].Count, 0);
            Assert.AreEqual(outData[cfg[0]].Count, 2);
            Assert.AreEqual(inData[cfg[1]].Count, 2);
            Assert.AreEqual(outData[cfg[1]].Count, 3);

            Assert.IsTrue(outData[cfg[0]].Any(value => value.VarName == "a" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(outData[cfg[0]].Any(value => value.VarName == "c" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.UNDEF));

            Assert.IsTrue(inData[cfg[1]].Any(value => value.VarName == "a" && 
                                                      value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(inData[cfg[1]].Any(value => value.VarName == "c" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.UNDEF));
            Assert.IsTrue(outData[cfg[1]].Any(value => value.VarName == "a" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(outData[cfg[1]].Any(value => value.VarName == "c" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.UNDEF));
            Assert.IsTrue(outData[cfg[1]].Any(value => value.VarName == "b" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.CONST));
        }

        [TestMethod]
        public void Constructor_IfWithoutPossibleConstDistribution()
        {
            /*
             * a = 9;
             * if (a < b)
             *     a = 10;
             * b = a;
             */

            var tacContainer = new ThreeAddressCode();

            // basic block #0
            Utils.AddAssignmentNode(tacContainer, null, "a", "9");
            Utils.AddAssignmentNode(tacContainer, null, "t1", "a", "<", "b");
            Utils.AddIfGotoNode(tacContainer, null, "L1", "t1");
            // basic block #1
            Utils.AddGotoNode(tacContainer, null, "L2");
            // basic block #2
            Utils.AddAssignmentNode(tacContainer, "L1", "a", "10");
            // basic block #3
            Utils.AddAssignmentNode(tacContainer, "L2", "b", "a");

            var cfg = new ControlFlowGraph(tacContainer);

            var ita = new ConstDistributionITA(cfg);
            var inData = ita.InOut.In;
            var outData = ita.InOut.Out;


            Assert.AreEqual(inData[cfg[0]].Count, 0);
            Assert.AreEqual(outData[cfg[0]].Count, 2);
            Assert.AreEqual(inData[cfg[1]].Count, 2);
            Assert.AreEqual(outData[cfg[1]].Count, 2);
            Assert.AreEqual(inData[cfg[2]].Count, 2);
            Assert.AreEqual(outData[cfg[2]].Count, 2);
            Assert.AreEqual(inData[cfg[3]].Count, 2);
            Assert.AreEqual(outData[cfg[3]].Count, 3);

            Assert.IsTrue(outData[cfg[0]].Any(value => value.VarName == "a" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(outData[cfg[0]].Any(value => value.VarName == "t1" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.UNDEF));

            Assert.IsTrue(inData[cfg[1]].Any(value => value.VarName == "a" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(inData[cfg[1]].Any(value => value.VarName == "t1" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.UNDEF));
            Assert.IsTrue(outData[cfg[1]].Any(value => value.VarName == "a" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(outData[cfg[1]].Any(value => value.VarName == "t1" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.UNDEF));

            Assert.IsTrue(inData[cfg[2]].Any(value => value.VarName == "a" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(inData[cfg[2]].Any(value => value.VarName == "t1" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.UNDEF));
            Assert.IsTrue(outData[cfg[2]].Any(value => value.VarName == "a" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.NAC));
            Assert.IsTrue(outData[cfg[2]].Any(value => value.VarName == "t1" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.UNDEF));

            Assert.IsTrue(inData[cfg[3]].Any(value => value.VarName == "a" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.NAC));
            Assert.IsTrue(inData[cfg[3]].Any(value => value.VarName == "t1" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.UNDEF));
            Assert.IsTrue(outData[cfg[3]].Any(value => value.VarName == "a" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.NAC));
            Assert.IsTrue(outData[cfg[3]].Any(value => value.VarName == "t1" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.UNDEF));
            Assert.IsTrue(outData[cfg[3]].Any(value => value.VarName == "b" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.NAC));
        }

        [TestMethod]
        public void Constructor_IfWithPossibleConstDistribution()
        {
            /*
             * a = 9;
             * if (a < b)
             *     a = 9;
             * b = a;
             */

            var tacContainer = new ThreeAddressCode();

            // basic block #0
            Utils.AddAssignmentNode(tacContainer, null, "a", "9");
            Utils.AddAssignmentNode(tacContainer, null, "t1", "a", "<", "b");
            Utils.AddIfGotoNode(tacContainer, null, "L1", "t1");
            // basic block #1
            Utils.AddGotoNode(tacContainer, null, "L2");
            // basic block #2
            Utils.AddAssignmentNode(tacContainer, "L1", "a", "9");
            // basic block #3
            Utils.AddAssignmentNode(tacContainer, "L2", "b", "a");

            var cfg = new ControlFlowGraph(tacContainer);

            var ita = new ConstDistributionITA(cfg);
            var inData = ita.InOut.In;
            var outData = ita.InOut.Out;

            Assert.AreEqual(inData[cfg[0]].Count, 0);
            Assert.AreEqual(outData[cfg[0]].Count, 2);
            Assert.AreEqual(inData[cfg[1]].Count, 2);
            Assert.AreEqual(outData[cfg[1]].Count, 2);
            Assert.AreEqual(inData[cfg[2]].Count, 2);
            Assert.AreEqual(outData[cfg[2]].Count, 2);
            Assert.AreEqual(inData[cfg[3]].Count, 2);
            Assert.AreEqual(outData[cfg[3]].Count, 3);

            Assert.IsTrue(outData[cfg[0]].Any(value => value.VarName == "a" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(outData[cfg[0]].Any(value => value.VarName == "t1" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.UNDEF));

            Assert.IsTrue(inData[cfg[1]].Any(value => value.VarName == "a" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(inData[cfg[1]].Any(value => value.VarName == "t1" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.UNDEF));
            Assert.IsTrue(outData[cfg[1]].Any(value => value.VarName == "a" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(outData[cfg[1]].Any(value => value.VarName == "t1" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.UNDEF));

            Assert.IsTrue(inData[cfg[2]].Any(value => value.VarName == "a" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(inData[cfg[2]].Any(value => value.VarName == "t1" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.UNDEF));
            Assert.IsTrue(outData[cfg[2]].Any(value => value.VarName == "a" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(outData[cfg[2]].Any(value => value.VarName == "t1" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.UNDEF));

            Assert.IsTrue(inData[cfg[3]].Any(value => value.VarName == "a" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(inData[cfg[3]].Any(value => value.VarName == "t1" &&
                                                      value.Value.TypeValue == SemilatticeValueEnum.UNDEF));
            Assert.IsTrue(outData[cfg[3]].Any(value => value.VarName == "a" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.CONST));
            Assert.IsTrue(outData[cfg[3]].Any(value => value.VarName == "t1" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.UNDEF));
            Assert.IsTrue(outData[cfg[3]].Any(value => value.VarName == "b" &&
                                                       value.Value.TypeValue == SemilatticeValueEnum.CONST));
        }
    }
}
