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
    public class AvailableExprTests
    {
        [TestMethod]
        public void Optimize_RightOptimized1()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "t1", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a1", "t1");
            Utils.AddAssignmentNode(tacContainer, null, "t2", "b");
            Utils.AddIfGotoNode(tacContainer, null, "L1", "t2");
            Utils.AddGotoNode(tacContainer, null, "L2");
            Utils.AddAssignmentNode(tacContainer, "L1", "t3", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a3", "t3");
            Utils.AddAssignmentNode(tacContainer, "L2", "t4", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a2", "t4");

            var expectedResult = new ThreeAddressCode();
            Utils.AddAssignmentNode(expectedResult, null, "t2", "4", "*", "i");
            Utils.AddAssignmentNode(expectedResult, null, "t1", "t2");
            Utils.AddAssignmentNode(expectedResult, null, "a1", "t1");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "b");
            Utils.AddIfGotoNode(expectedResult, null, "L1", "t2");
            Utils.AddGotoNode(expectedResult, null, "L2");
            Utils.AddAssignmentNode(expectedResult, "L1", "t3", "t2");
            Utils.AddAssignmentNode(expectedResult, null, "a3", "t3");
            Utils.AddAssignmentNode(expectedResult, "L2", "t4", "t2");
            Utils.AddAssignmentNode(expectedResult, null, "a2", "t4");

            var cfg = new ControlFlowGraph(tacContainer);
            var expectedResultcfg = new ControlFlowGraph(expectedResult);
            E_GenKillVisitor availExprVisitor = new E_GenKillVisitor();
            var availExprContainers = availExprVisitor.GenerateAvailableExpressionForBlocks(cfg.SourceBasicBlocks);

            var availableExpressionsITA = new AvailableExpressionsITA(cfg, availExprContainers);

            var availableExprOptimization = new AvailableExprOptimization();
            bool isOptimized = availableExprOptimization.Optimize(availableExpressionsITA);
            var basicBlockItems = cfg.SourceBasicBlocks.BasicBlockItems;
            var codeText = cfg.SourceBasicBlocks.BasicBlockItems
                .Select(bl => bl.ToString()).Aggregate((b1, b2) => b1 + b2);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(cfg.ToString(), expectedResultcfg.ToString());
        }

        [TestMethod]
        public void Optimize_RightOptimized2()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "t1", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a1", "t1");
            Utils.AddAssignmentNode(tacContainer, null, "t2", "b");
            Utils.AddAssignmentNode(tacContainer, null, "i", "1");
            Utils.AddIfGotoNode(tacContainer, null, "L1", "t2");
            Utils.AddGotoNode(tacContainer, null, "L2");
            Utils.AddAssignmentNode(tacContainer, "L1", "t3", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, "L2", "a3", "t3");
            Utils.AddAssignmentNode(tacContainer, null, "t4", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a2", "t4");

            var expectedResult = new ThreeAddressCode();
            Utils.AddAssignmentNode(expectedResult, null, "t3", "4", "*", "i");
            Utils.AddAssignmentNode(expectedResult, null, "t1", "t3");
            Utils.AddAssignmentNode(expectedResult, null, "a1", "t1");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "b");
            Utils.AddAssignmentNode(expectedResult, null, "i", "1");
            Utils.AddIfGotoNode(expectedResult, null, "L1", "t2");
            Utils.AddGotoNode(expectedResult, null, "L2");
            Utils.AddAssignmentNode(expectedResult, "L1", "t3", "4", "*", "i");
            Utils.AddAssignmentNode(expectedResult, "L2", "a3", "t3");
            Utils.AddAssignmentNode(expectedResult, null, "t3", "4", "*", "i");
            Utils.AddAssignmentNode(expectedResult, null, "t4", "t3");
            Utils.AddAssignmentNode(expectedResult, null, "a2", "t4");

            var cfg = new ControlFlowGraph(tacContainer);
            var expectedResultcfg = new ControlFlowGraph(expectedResult);

            E_GenKillVisitor availExprVisitor = new E_GenKillVisitor();
            var availExprContainers = availExprVisitor.GenerateAvailableExpressionForBlocks(cfg.SourceBasicBlocks);

            var availableExpressionsITA = new AvailableExpressionsITA(cfg, availExprContainers);

            var availableExprOptimization = new AvailableExprOptimization();
            bool isOptimized = availableExprOptimization.Optimize(availableExpressionsITA);
            var basicBlockItems = cfg.SourceBasicBlocks.BasicBlockItems;
            var codeText = cfg.SourceBasicBlocks.BasicBlockItems
                .Select(bl => bl.ToString()).Aggregate((b1, b2) => b1 + b2);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(cfg.ToString(), expectedResultcfg.ToString());
        }

        [TestMethod]
        public void Optimize_RightOptimized3()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "t1", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a1", "t1");
            Utils.AddAssignmentNode(tacContainer, null, "t2", "b");
            Utils.AddIfGotoNode(tacContainer, null, "L1", "t2");
            Utils.AddGotoNode(tacContainer, null, "L2");
            Utils.AddAssignmentNode(tacContainer, "L1", "t3", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a3", "t3");
            Utils.AddAssignmentNode(tacContainer, "L2", null, null);

            var expectedResult = new ThreeAddressCode();
            Utils.AddAssignmentNode(expectedResult, null, "t4", "4", "*", "i");
            Utils.AddAssignmentNode(expectedResult, null, "t1", "t4");
            Utils.AddAssignmentNode(expectedResult, null, "a1", "t1");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "b");
            Utils.AddIfGotoNode(expectedResult, null, "L1", "t2");
            Utils.AddGotoNode(expectedResult, null, "L2");
            Utils.AddAssignmentNode(expectedResult, "L1", "t3", "t4");
            Utils.AddAssignmentNode(expectedResult, null, "a3", "t3");
            Utils.AddAssignmentNode(expectedResult, "L2", null , null);

            var cfg = new ControlFlowGraph(tacContainer);

            var expectedResultcfg = new ControlFlowGraph(expectedResult);

            E_GenKillVisitor availExprVisitor = new E_GenKillVisitor();
            var availExprContainers = availExprVisitor.GenerateAvailableExpressionForBlocks(cfg.SourceBasicBlocks);

            var availableExpressionsITA = new AvailableExpressionsITA(cfg, availExprContainers);

            var availableExprOptimization = new AvailableExprOptimization();
            bool isOptimized = availableExprOptimization.Optimize(availableExpressionsITA);
            var basicBlockItems = cfg.SourceBasicBlocks.BasicBlockItems;
            var codeText = cfg.SourceBasicBlocks.BasicBlockItems
                .Select(bl => bl.ToString()).Aggregate((b1, b2) => b1 + b2);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(cfg.ToString(), expectedResultcfg.ToString());
        }
    }
}
