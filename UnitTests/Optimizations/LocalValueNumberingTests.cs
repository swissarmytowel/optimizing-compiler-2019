using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleLang.Visitors;
using SimpleLang.Optimizations;
using SimpleLang.TACode;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class LocalValueNumberingTests
    {
        //TODO: update methods
       
        [TestMethod]
        public void Optimize_RightOptimized1()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "a", "b", "+", "c");
            Utils.AddAssignmentNode(tacContainer, null, "b", "a", "-", "d");
            Utils.AddAssignmentNode(tacContainer, null, "c", "b", "+", "c");
            Utils.AddAssignmentNode(tacContainer, null, "d", "a", "-", "d");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, null, "a", "b", "+", "c");
            Utils.AddAssignmentNode(expectedResult, null, "b", "a", "-", "d");
            Utils.AddAssignmentNode(expectedResult, null, "c", "b", "+", "c");
            Utils.AddAssignmentNode(expectedResult, null, "d", "b");

            var optimization = new LocalValueNumberingOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }

        [TestMethod]
        public void Optimize_IsNotOptimized()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "a", "b", "+", "c");
            Utils.AddAssignmentNode(tacContainer, null, "b", "a", "-", "d");
            Utils.AddAssignmentNode(tacContainer, null, "c", "b", "+", "c");

            var optimization = new LocalValueNumberingOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsFalse(isOptimized);
        }

        [TestMethod]
        public void Optimize_RightOptimized2()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "a", "b", "*", "c");
            Utils.AddAssignmentNode(tacContainer, null, "d", "b");
            Utils.AddAssignmentNode(tacContainer, null, "e", "d", "*", "c");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, null, "a", "b", "*", "c");
            Utils.AddAssignmentNode(expectedResult, null, "d", "b");
            Utils.AddAssignmentNode(expectedResult, null, "e", "a");

            var optimization = new LocalValueNumberingOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }

        [TestMethod]
        public void Optimize_ReversedExpressions()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "a", "1", "+", "2");
            Utils.AddAssignmentNode(tacContainer, null, "b", "2", "+", "1");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, null, "a", "1", "+", "2");
            Utils.AddAssignmentNode(expectedResult, null, "b", "a");

            var optimization = new LocalValueNumberingOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }


        [TestMethod]
        public void Optimize_NoParamsWithDefinedType()
        {
            TmpNameManager.Instance.Drop();

            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "a", "x", "+", "y");
            Utils.AddAssignmentNode(tacContainer, null, "b", "x", "+", "y");
            Utils.AddAssignmentNode(tacContainer, null, "a", "17");
            Utils.AddAssignmentNode(tacContainer, null, "b", "18");
            Utils.AddAssignmentNode(tacContainer, null, "c", "x", "+", "y");
            
            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, null, "a", "x", "+", "y");
            Utils.AddAssignmentNode(expectedResult, null, "b", "a");
            Utils.AddAssignmentNode(expectedResult, null, "t1", "a");
            Utils.AddAssignmentNode(expectedResult, null, "a", "17");
            Utils.AddAssignmentNode(expectedResult, null, "b", "18");
            Utils.AddAssignmentNode(expectedResult, null, "c", "t1");

            var optimization = new LocalValueNumberingOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }
        
    }
}
