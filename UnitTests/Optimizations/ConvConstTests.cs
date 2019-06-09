using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Optimizations;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Visitors;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class ConvConstTests
    {

        [TestMethod]
        public void Optimize_PlusInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "2",
                    Operation = "+",
                    SecondOperand = "3"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                LeftPartIdentifier = "t1",
                FirstOperand = "5"
            };

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(optimizedTac.ToString(), res.ToString());
        }

        [TestMethod]
        public void Optimize_MinusInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "2",
                    Operation = "-",
                    SecondOperand = "3"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                LeftPartIdentifier = "t1",
                FirstOperand = "-1"
            };

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(optimizedTac.ToString(), res.ToString());
        }

        [TestMethod]
        public void Optimize_MultInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "2",
                    Operation = "*",
                    SecondOperand = "3"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                LeftPartIdentifier = "t1",
                FirstOperand = "6"
            };

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(optimizedTac.ToString(), res.ToString());
        }

        [TestMethod]
        public void Optimize_DivisionInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "/",
                    SecondOperand = "2"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                LeftPartIdentifier = "t1",
                FirstOperand = "2"
            };

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(optimizedTac.ToString(), res.ToString());
        }

        [TestMethod]
        public void Optimize_GreaterInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = ">",
                    SecondOperand = "2"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                LeftPartIdentifier = "t1",
                FirstOperand = "True"
            };

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(optimizedTac.ToString(), res.ToString());
        }

        [TestMethod]
        public void Optimize_LessInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "<",
                    SecondOperand = "2"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                LeftPartIdentifier = "t1",
                FirstOperand = "False"
            };

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(optimizedTac.ToString(), res.ToString());
        }

        [TestMethod]
        public void Optimize_GreaterOrEqualInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = ">=",
                    SecondOperand = "4"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                LeftPartIdentifier = "t1",
                FirstOperand = "True"
            };

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(optimizedTac.ToString(), res.ToString());
        }

        [TestMethod]
        public void Optimize_LessOrEqualInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "<=",
                    SecondOperand = "4"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                LeftPartIdentifier = "t1",
                FirstOperand = "True"
            };

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(optimizedTac.ToString(), res.ToString());
        }

        [TestMethod]
        public void Optimize_EqualInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "==",
                    SecondOperand = "4"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                LeftPartIdentifier = "t1",
                FirstOperand = "True"
            };

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(optimizedTac.ToString(), res.ToString());
        }

        [TestMethod]
        public void Optimize_NotEqualInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "!=",
                    SecondOperand = "4"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                LeftPartIdentifier = "t1",
                FirstOperand = "False"
            };

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(optimizedTac.ToString(), res.ToString());
        }

        [TestMethod]
        public void Optimize_IsOptimized()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "!=",
                    SecondOperand = "4"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);
            Assert.IsTrue(isOptimized);
        }

        [TestMethod]
        public void Optimize_IsNotOptimized()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "!=",
                    SecondOperand = "a"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);
            Assert.IsFalse(isOptimized);
        }
        
    }
}
