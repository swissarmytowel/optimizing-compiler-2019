using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Visitors;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class CopyPropagationTests
    {
        [TestMethod]
        public void Optimize_OneCopyPropagation()
        {
            /*
	            a = b
	            c = b - a     -----> c = b - b
	            a = 1
	            e = d * a     -----> e = d * a
            */

            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, "L1", "a", "b");
            Utils.AddAssignmentNode(tacContainer, null, "c", "b", "-", "a");
            Utils.AddAssignmentNode(tacContainer, null, "a", "1");
            Utils.AddAssignmentNode(tacContainer, null, "e", "d", "*", "a");

            var expectedResult = new ThreeAddressCode();
            Utils.AddAssignmentNode(expectedResult, "L1", "a", "b");
            Utils.AddAssignmentNode(expectedResult, null, "c", "b", "-", "b");
            Utils.AddAssignmentNode(expectedResult, null, "a", "1");
            Utils.AddAssignmentNode(expectedResult, null, "e", "d", "*", "a");

            var optimization = new CopyPropagationOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());

        }


        [TestMethod]
        public void Optimize_ThreeCopyPropagation()
        {
            /*
	            a = b
	            c = b - a     -----> c = b - b
	            a = f
	            e = d * a     -----> e = d * f
                a = x
                y = a + a     -----> y = x + x
            */

            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, "L1", "a", "b");
            Utils.AddAssignmentNode(tacContainer, null, "c", "b", "-", "a");
            Utils.AddAssignmentNode(tacContainer, null, "a", "f");
            Utils.AddAssignmentNode(tacContainer, null, "e", "d", "*", "a");
            Utils.AddAssignmentNode(tacContainer, null, "a", "x");
            Utils.AddAssignmentNode(tacContainer, null, "y", "a", "+", "a");


            var expectedResult = new ThreeAddressCode();
            Utils.AddAssignmentNode(expectedResult, "L1", "a", "b");
            Utils.AddAssignmentNode(expectedResult, null, "c", "b", "-", "b");
            Utils.AddAssignmentNode(expectedResult, null, "a", "f");
            Utils.AddAssignmentNode(expectedResult, null, "e", "d", "*", "f");
            Utils.AddAssignmentNode(expectedResult, null, "a", "x");
            Utils.AddAssignmentNode(expectedResult, null, "y", "x", "+", "x");

            var optimization = new CopyPropagationOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());

        }

        [TestMethod]
        public void Optimize_SimpleMultipleCopyPropagation()
        {
            /*
	            a = b
	            c = b - a     -----> c = b - b
	            a = 5
	            e = d * a     -----> e = d * a
                a = x
                y = a + a     -----> y = x + x
            */

            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, "L1", "a", "b");
            Utils.AddAssignmentNode(tacContainer, null, "c", "b", "-", "a");
            Utils.AddAssignmentNode(tacContainer, null, "a", "5");
            Utils.AddAssignmentNode(tacContainer, null, "e", "d", "*", "a");
            Utils.AddAssignmentNode(tacContainer, null, "a", "x");
            Utils.AddAssignmentNode(tacContainer, null, "y", "a", "+", "a");


            var expectedResult = new ThreeAddressCode();
            Utils.AddAssignmentNode(expectedResult, "L1", "a", "b");
            Utils.AddAssignmentNode(expectedResult, null, "c", "b", "-", "b");
            Utils.AddAssignmentNode(expectedResult, null, "a", "5");
            Utils.AddAssignmentNode(expectedResult, null, "e", "d", "*", "a");
            Utils.AddAssignmentNode(expectedResult, null, "a", "x");
            Utils.AddAssignmentNode(expectedResult, null, "y", "x", "+", "x");

            var optimization = new CopyPropagationOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());

        }

        [TestMethod]
        public void Optimize_MultipleCopyPropagation()
        {
            /*
	            a = b
	            c = b - a     -----> c = b - b
	            d = c + 1
	            e = d * a     -----> e = d * b
	            a = 35 - f
	            k = c + a     -----> k = c + a
            */

            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, "L1", "a", "b");
            Utils.AddAssignmentNode(tacContainer, null, "c", "b", "-", "a");
            Utils.AddAssignmentNode(tacContainer, null, "d", "c", "+", "1");
            Utils.AddAssignmentNode(tacContainer, null, "e", "d", "*", "a");
            Utils.AddAssignmentNode(tacContainer, null, "a", "35", "-", "f");
            Utils.AddAssignmentNode(tacContainer, null, "k", "c", "+", "a");

            var expectedResult = new ThreeAddressCode();
            Utils.AddAssignmentNode(expectedResult, "L1", "a", "b");
            Utils.AddAssignmentNode(expectedResult, null, "c", "b", "-", "b");
            Utils.AddAssignmentNode(expectedResult, null, "d", "c", "+", "1");
            Utils.AddAssignmentNode(expectedResult, null, "e", "d", "*", "b");
            Utils.AddAssignmentNode(expectedResult, null, "a", "35", "-", "f");
            Utils.AddAssignmentNode(expectedResult, null, "k", "c", "+", "a");

            var optimization = new CopyPropagationOptimization();
            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
        }

    }
}
