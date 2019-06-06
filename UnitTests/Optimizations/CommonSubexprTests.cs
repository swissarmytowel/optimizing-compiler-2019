using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Optimizations;
using SimpleLang.Visitors;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class CommonSubexprTests
    {
        [TestMethod]
        public void Optimize_ThreeTimesRepeatedExpression()
        {
            var tacVisitor = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(tacVisitor, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacVisitor, null, "t2", "2");
            Utils.AddAssignmentNode(tacVisitor, null, "t3", "a", "+", "b");
            Utils.AddAssignmentNode(tacVisitor, null, "t4", "10");
            Utils.AddAssignmentNode(tacVisitor, null, "t5", "a", "+", "b");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "2");
            Utils.AddAssignmentNode(expectedResult, null, "t3", "t1");
            Utils.AddAssignmentNode(expectedResult, null, "t4", "10");
            Utils.AddAssignmentNode(expectedResult, null, "t5", "t1");

            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacVisitor.TACodeContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacVisitor.ToString(), expectedResult.ToString());
        }

        [TestMethod]
        public void Optimize_ChangedFirstOperand()
        {
            var tacVisitor = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(tacVisitor, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacVisitor, null, "a", "2");
            Utils.AddAssignmentNode(tacVisitor, null, "t2", "a", "+", "b");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "a", "2");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "a", "+", "b");

            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacVisitor.TACodeContainer);

            Assert.IsFalse(isOptimized);
            Assert.AreEqual(tacVisitor.ToString(), expectedResult.ToString());
        }

        [TestMethod]
        public void Optimize_ChangedSecondOperand()
        {
            var tacVisitor = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(tacVisitor, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacVisitor, null, "b", "2");
            Utils.AddAssignmentNode(tacVisitor, null, "t2", "a", "+", "b");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "b", "2");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "a", "+", "b");

            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacVisitor.TACodeContainer);

            Assert.IsFalse(isOptimized);
            Assert.AreEqual(tacVisitor.ToString(), expectedResult.ToString());
        }

        [TestMethod]
        public void Optimize_ChangedOperandThenTwiceRepeatedExpression()
        {
            var tacVisitor = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(tacVisitor, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacVisitor, null, "b", "2");
            Utils.AddAssignmentNode(tacVisitor, null, "t2", "a", "+", "b");
            Utils.AddAssignmentNode(tacVisitor, null, "t3", "a", "+", "b");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "b", "2");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "t3", "t2");

            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacVisitor.TACodeContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacVisitor.ToString(), expectedResult.ToString());
        }

        [TestMethod]
        public void Optimize_MultipleExpressions()
        {
            var tacVisitor = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(tacVisitor, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacVisitor, null, "t2", "c", "+", "d");
            Utils.AddAssignmentNode(tacVisitor, null, "t3", "c", "+", "d");
            Utils.AddAssignmentNode(tacVisitor, null, "t4", "a", "+", "b");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "c", "+", "d");
            Utils.AddAssignmentNode(expectedResult, null, "t3", "t2");
            Utils.AddAssignmentNode(expectedResult, null, "t4", "t1");

            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacVisitor.TACodeContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacVisitor.ToString(), expectedResult.ToString());
        }
    }
}
