using System;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.Visitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.CFG;
using SimpleLang.E_GenKill.Implementations;
using SimpleLang.IterationAlgorithms;
using System.Linq;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class AvailableExprTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "t1", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a1", "t1");
            Utils.AddIfGotoNode(tacContainer, null, "L2", "t4");
            Utils.AddGotoNode(tacContainer, null, "L2");
            Utils.AddAssignmentNode(tacContainer, "L1", "t2", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a3", "t2");
            Utils.AddAssignmentNode(tacContainer, "L2", "t3", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a2", "t3");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, null, "t5", "4", "*", "i");
            Utils.AddAssignmentNode(expectedResult, null, "t1", "t5");
            Utils.AddAssignmentNode(expectedResult, null, "a1", "t1");
            Utils.AddIfGotoNode(tacContainer, null, "L2", "t4");
            Utils.AddGotoNode(tacContainer, null, "L2");
            Utils.AddAssignmentNode(tacContainer, "L1", "t2", "t5");
            Utils.AddAssignmentNode(tacContainer, null, "a3", "t2");
            Utils.AddAssignmentNode(tacContainer, "L2", "t3", "t5");
            Utils.AddAssignmentNode(tacContainer, null, "a2", "t3");

            var cfg = new ControlFlowGraph(tacContainer);

            E_GenKillVisitor availExprVisitor = new E_GenKillVisitor();
            var availExprContainers = availExprVisitor.GenerateAvailableExpressionForBlocks(cfg.SourceBasicBlocks);

            var availableExpressionsITA = new AvailableExpressionsITA(cfg, availExprContainers);

            var availableExprOptimization = new AvailableExprOptimization();
            bool isOptimized = availableExprOptimization.Optimize(availableExpressionsITA);
            var basicBlockItems = cfg.SourceBasicBlocks.BasicBlockItems;
            var codeText = cfg.SourceBasicBlocks.BasicBlockItems
                .Select(bl => bl.ToString()).Aggregate((b1, b2) => b1 + b2);

            Assert.IsFalse(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }
    }
}
