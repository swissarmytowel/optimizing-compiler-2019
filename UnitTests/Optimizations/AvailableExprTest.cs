using System;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.Visitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            //var optimization = new AvailableExprOptimization();
            //var isOptimized = optimization.Optimize(tacContainer);

            //Assert.IsFalse(isOptimized);
            //Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }
    }
}
