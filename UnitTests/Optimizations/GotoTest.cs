using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.Visitors;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class GotoTest
    {
        [TestMethod]
        public void Optimize_RightOptimized()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "t1", "m" , ">", "2");
            Utils.AddIfGotoNode(tacContainer, null, "L1", "t1");
            Utils.AddGotoNode(tacContainer, null, "L2");
            Utils.AddAssignmentNode(tacContainer, "L1", "c", "3");
            Utils.AddAssignmentNode(tacContainer, "L2", null, null);

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, null, "t1", "m", ">", "2");
            Utils.AddAssignmentNode(expectedResult, null, "t5", null, "!", "t1");
            Utils.AddIfGotoNode(expectedResult, null, "L2", "t5");
            Utils.AddAssignmentNode(expectedResult, "L1", "c", "3");
            Utils.AddAssignmentNode(expectedResult, "L2", null, null);

            var optimization = new GotoOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
            Assert.IsTrue(isOptimized);
        }
       
    }
}
