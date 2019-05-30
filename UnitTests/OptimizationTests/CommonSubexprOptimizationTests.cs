using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Optimizations;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Visitors;

namespace UnitTests.OptimizationTests
{
    [TestClass]
    public class CommonSubexprOptimizationTests
    {
        [TestMethod]
        public void TrivialOptimization()
        {
            var tacVisitor = new ThreeAddressCodeVisitor();
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t2",
                FirstOperand = "2"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });

            var result = new ThreeAddressCodeVisitor();
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t2",
                FirstOperand = "2"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "t1"
            });
            
            var optimization = new CommonSubexprOptimization();
            optimization.Optimize(tacVisitor.TACodeContainer);
            Assert.AreEqual(tacVisitor.ToString(), result.ToString());
        }

        [TestMethod]
        public void OptimizationOfRepeatingExpression()
        {
            var tacVisitor = new ThreeAddressCodeVisitor();
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t2",
                FirstOperand = "2"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t4",
                FirstOperand = "10"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t5",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });

            var result = new ThreeAddressCodeVisitor();
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t2",
                FirstOperand = "2"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "t1"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t4",
                FirstOperand = "10"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t5",
                FirstOperand = "t1"
            });

            var optimization = new CommonSubexprOptimization();
            optimization.Optimize(tacVisitor.TACodeContainer);
            Assert.AreEqual(tacVisitor.ToString(), result.ToString());
        }

        [TestMethod]
        public void OptimizationWhenChangingVariableFromExpression()
        {
            var tacVisitor = new ThreeAddressCodeVisitor();
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "a",
                FirstOperand = "2"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });

            var result = new ThreeAddressCodeVisitor();
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "a",
                FirstOperand = "2"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });

            var optimization = new CommonSubexprOptimization();
            optimization.Optimize(tacVisitor.TACodeContainer);
            Assert.AreEqual(tacVisitor.ToString(), result.ToString());

            tacVisitor = new ThreeAddressCodeVisitor();
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "b",
                FirstOperand = "2"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });

            result = new ThreeAddressCodeVisitor();
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "b",
                FirstOperand = "2"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });

            optimization.Optimize(tacVisitor.TACodeContainer);
            Assert.AreEqual(tacVisitor.ToString(), result.ToString());

            tacVisitor = new ThreeAddressCodeVisitor();
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "b",
                FirstOperand = "2"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t4",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });

            result = new ThreeAddressCodeVisitor();
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "b",
                FirstOperand = "2"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t4",
                FirstOperand = "t3"
            });

            optimization.Optimize(tacVisitor.TACodeContainer);
            Assert.AreEqual(tacVisitor.ToString(), result.ToString());
        }

        [TestMethod]
        public void OptimizationOfMultipleExpressions()
        {
            var tacVisitor = new ThreeAddressCodeVisitor();
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t2",
                FirstOperand = "c",
                Operation = "+",
                SecondOperand = "d"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "c",
                Operation = "+",
                SecondOperand = "d"
            });
            tacVisitor.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t4",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });

            var result = new ThreeAddressCodeVisitor();
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                Label = "L1",
                LeftPartIdentifier = "t1",
                FirstOperand = "a",
                Operation = "+",
                SecondOperand = "b"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t2",
                FirstOperand = "c",
                Operation = "+",
                SecondOperand = "d"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t3",
                FirstOperand = "t2"
            });
            result.TACodeContainer.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "t4",
                FirstOperand = "t1"
            });

            var optimization = new CommonSubexprOptimization();
            optimization.Optimize(tacVisitor.TACodeContainer);
            Assert.AreEqual(tacVisitor.ToString(), result.ToString());
        }
    }
}
