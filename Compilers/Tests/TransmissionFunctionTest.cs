using NUnit.Framework;
using System;
using System.Collections.Generic;

using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleLang.GenKill.Implementations;
using SimpleLang.TacBasicBlocks;

namespace SimpleLang.Tests
{
    public class TFByCompositionTest
    {

        [Test]
        public void Calculate_EmptyIn()
        {
            var bblock_1 = new ThreeAddressCode();
            bblock_1.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "i",
                FirstOperand = "m",
                Operation = "-",
                SecondOperand = "1"
            });
            bblock_1.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "j",
                FirstOperand = "n",
            });
            bblock_1.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "a",
                FirstOperand = "u1"
            });

            var bblock_2 = new ThreeAddressCode();
            bblock_2.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "i",
                FirstOperand = "i",
                Operation = "+",
                SecondOperand = "1"
            });
            bblock_2.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "j",
                FirstOperand = "j",
                Operation = "-",
                SecondOperand = "1"
            });

            var bblock_3 = new ThreeAddressCode();
            bblock_3.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "a",
                FirstOperand = "u2",
            });

            var bblock_4 = new ThreeAddressCode();
            bblock_4.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "i",
                FirstOperand = "u3",
            });

            var basicBlocks = new BasicBlocks
            {
                BasicBlockItems = new List<ThreeAddressCode> { bblock_1, bblock_2, bblock_3, bblock_4 }
            };

            var genKillVisitor = new GenKillVisitor();
            var genKill = genKillVisitor.GenerateReachingDefinitionForBlocks(basicBlocks);

            var tfFunction = new TFByComposition(genKill);
            var output = tfFunction.Calculate(new HashSet<TacNode>(), basicBlocks.BasicBlockItems[0]);
            var expected = new HashSet<TacNode>();

            foreach (var item in bblock_1)
                expected.Add(item);
            
            Assert.IsTrue(expected.SetEquals(output));
        }

        [Test]
        public void Calculate_NotEmpyInAndOut()
        {
            var bblock_1 = new ThreeAddressCode();
            bblock_1.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "i",
                FirstOperand = "m",
                Operation = "-",
                SecondOperand = "1"
            });
            bblock_1.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "j",
                FirstOperand = "n",
            });
            bblock_1.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "a",
                FirstOperand = "u1"
            });

            var bblock_2 = new ThreeAddressCode();
            bblock_2.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "i",
                FirstOperand = "i",
                Operation = "+",
                SecondOperand = "1"
            });
            bblock_2.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "j",
                FirstOperand = "j",
                Operation = "-",
                SecondOperand = "1"
            });

            var bblock_3 = new ThreeAddressCode();
            bblock_3.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "a",
                FirstOperand = "u2",
            });

            var bblock_4 = new ThreeAddressCode();
            bblock_4.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "i",
                FirstOperand = "u3",
            });

            var basicBlocks = new BasicBlocks
            {
                BasicBlockItems = new List<ThreeAddressCode> { bblock_1, bblock_2, bblock_3, bblock_4 }
            };

            var genKillVisitor = new GenKillVisitor();
            var genKill = genKillVisitor.GenerateReachingDefinitionForBlocks(basicBlocks);

            var expected = new HashSet<TacNode>
            {
                bblock_1.TACodeLines.First.Next.Next.Value,
                bblock_2.TACodeLines.First.Value,
                bblock_2.TACodeLines.First.Next.Value
            };

            var _in = new HashSet<TacNode>();
            foreach (var line in bblock_1)
                _in.Add(line);

            var tfFunction = new TFByComposition(genKill);
            var output = tfFunction.Calculate(_in, bblock_2);

            Assert.IsTrue(expected.SetEquals(output));
        }

        [Test]
        public void Calculate_lastBlock()
        {
            var bblock_1 = new ThreeAddressCode();
            bblock_1.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "i",
                FirstOperand = "m",
                Operation = "-",
                SecondOperand = "1"
            });
            bblock_1.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "j",
                FirstOperand = "n",
            });
            bblock_1.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "a",
                FirstOperand = "u1"
            });

            var bblock_2 = new ThreeAddressCode();
            bblock_2.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "i",
                FirstOperand = "i",
                Operation = "+",
                SecondOperand = "1"
            });
            bblock_2.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "j",
                FirstOperand = "j",
                Operation = "-",
                SecondOperand = "1"
            });

            var bblock_3 = new ThreeAddressCode();
            bblock_3.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "a",
                FirstOperand = "u2",
            });

            var bblock_4 = new ThreeAddressCode();
            bblock_4.PushNode(new TacAssignmentNode
            {
                LeftPartIdentifier = "i",
                FirstOperand = "u3",
            });

            var basicBlocks = new BasicBlocks
            {
                BasicBlockItems = new List<ThreeAddressCode> { bblock_1, bblock_2, bblock_3, bblock_4 }
            };

            var genKillVisitor = new GenKillVisitor();
            var genKill = genKillVisitor.GenerateReachingDefinitionForBlocks(basicBlocks);

            var expected = new HashSet<TacNode>
            {
                bblock_2.First.Next.Value,
                bblock_3.First.Value,
                bblock_4.First.Value
            };

            var _in = new HashSet<TacNode>
            {
                bblock_2.First.Value,
                bblock_2.First.Next.Value,
                bblock_3.First.Value
            };

            var tfFunction = new TFByComposition(genKill);
            var output = tfFunction.Calculate(_in, bblock_4);

            Assert.IsTrue(expected.SetEquals(output));
        }
    }
}
