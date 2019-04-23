using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Visitors;
using SimpleLang.Optimizations;
using SimpleLang.TACode.TacNodes;

namespace UnitTests.OptimizationTests
{
    [TestClass]
    public class ConstsConvolutionTests
    {
        [TestMethod]
        public void Test_PlusInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "2",
                    Operation = "+",
                    SecondOperand = "3"
                });

            new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "5"
            };
            Assert.AreEqual(optimizedTac, res);
        }

        [TestMethod]
        public void Test_MinusInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "2",
                    Operation = "-",
                    SecondOperand = "3"
                });

            new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "-1"
            };
            Assert.AreEqual(optimizedTac, res);
        }

        [TestMethod]
        public void Test_MultInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "2",
                    Operation = "*",
                    SecondOperand = "3"
                });

            new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "6"
            };
            Assert.AreEqual(optimizedTac, res);
        }

        [TestMethod]
        public void Test_DivisionInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "/",
                    SecondOperand = "2"
                });

            new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "2"
            };
            Assert.AreEqual(optimizedTac, res);
        }

        [TestMethod]
        public void Test_GreaterInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = ">",
                    SecondOperand = "2"
                });

            new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "True"
            };
            Assert.AreEqual(optimizedTac, res);
        }

        [TestMethod]
        public void Test_LessInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "<",
                    SecondOperand = "2"
                });

            new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "False"
            };
            Assert.AreEqual(optimizedTac, res);
        }

        [TestMethod]
        public void Test_GreaterOrEqualInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = ">=",
                    SecondOperand = "4"
                });

            new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "True"
            };
            Assert.AreEqual(optimizedTac, res);
        }

        [TestMethod]
        public void Test_LessOrEqualInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "<=",
                    SecondOperand = "4"
                });

            new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "True"
            };
            Assert.AreEqual(optimizedTac, res);
        }

        [TestMethod]
        public void Test_EqualInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "==",
                    SecondOperand = "4"
                });

            new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "True"
            };
            Assert.AreEqual(optimizedTac, res);
        }

        [TestMethod]
        public void Test_NotEqualInts()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "!=",
                    SecondOperand = "4"
                });

            new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            var optimizedTac = threeAddressCodeVisitor.TACodeContainer.TACodeLines.First.Value as TacAssignmentNode;
            var res = new TacAssignmentNode()
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "False"
            };
            Assert.AreEqual(optimizedTac, res);
        }

        [TestMethod]
        public void Test_IsOptimized()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
                    LeftPartIdentifier = "t1",
                    FirstOperand = "4",
                    Operation = "!=",
                    SecondOperand = "4"
                });

            var isOptimized = new ConvConstOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);
            Assert.IsTrue(isOptimized);
        }

        [TestMethod]
        public void Test_IsNotOptimized()
        {
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(
                new TacAssignmentNode()
                {
                    Label = "L1",
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
