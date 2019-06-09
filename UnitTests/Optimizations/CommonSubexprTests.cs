using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.Visitors;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class CommonSubexprTests
    {
        [TestMethod]
        public void Optimize_ThreeTimesRepeatedExpression()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacContainer, null, "t2", "2");
            Utils.AddAssignmentNode(tacContainer, null, "t3", "a", "+", "b");
            Utils.AddAssignmentNode(tacContainer, null, "t4", "10");
            Utils.AddAssignmentNode(tacContainer, null, "t5", "a", "+", "b");

            var expectedResult = new ThreeAddressCode();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "2");
            Utils.AddAssignmentNode(expectedResult, null, "t3", "t1");
            Utils.AddAssignmentNode(expectedResult, null, "t4", "10");
            Utils.AddAssignmentNode(expectedResult, null, "t5", "t1");

            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }

        [TestMethod]
        public void Optimize_ChangedFirstOperand()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacContainer, null, "a", "2");
            Utils.AddAssignmentNode(tacContainer, null, "t2", "a", "+", "b");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "a", "2");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "a", "+", "b");

            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsFalse(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }

        [TestMethod]
        public void Optimize_ChangedSecondOperand()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacContainer, null, "b", "2");
            Utils.AddAssignmentNode(tacContainer, null, "t2", "a", "+", "b");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "b", "2");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "a", "+", "b");

            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsFalse(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }

        [TestMethod]
        public void Optimize_ChangedOperandThenTwiceRepeatedExpression()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacContainer, null, "b", "2");
            Utils.AddAssignmentNode(tacContainer, null, "t2", "a", "+", "b");
            Utils.AddAssignmentNode(tacContainer, null, "t3", "a", "+", "b");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "b", "2");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "t3", "t2");

            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }

        [TestMethod]
        public void Optimize_MultipleExpressions()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacContainer, null, "t2", "c", "+", "d");
            Utils.AddAssignmentNode(tacContainer, null, "t3", "c", "+", "d");
            Utils.AddAssignmentNode(tacContainer, null, "t4", "a", "+", "b");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "c", "+", "d");
            Utils.AddAssignmentNode(expectedResult, null, "t3", "t2");
            Utils.AddAssignmentNode(expectedResult, null, "t4", "t1");

            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }
    }
}