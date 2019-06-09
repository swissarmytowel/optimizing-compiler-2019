using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.TACode;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;
using System;

namespace UnitTests.Visitors
{
    [TestClass]
    public class ThreeAddressCodeTests
    {
        [TestMethod]
        public void Visit_ifTacGeneration()
        {
            TmpNameManager.Instance.Drop();
            var tacVisitor = new ThreeAddressCodeVisitor();
            var tacTest = new ThreeAddressCode();

            var source = "a = 42;\n" +
                         "x = 13;" +
                         "b = a + (30 * (a - 1));\n" +
                         "if(b > (x + 10)){\n" +
                         "    a = 8;\n" +
                         "}else{\n" +
                         "    a = 9;\n" +
                         "    x = 100;\n" +
                         "}";
            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();
            var root = parser.root;

            root.Visit(tacVisitor);
            tacVisitor.Postprocess();

            Utils.AddAssignmentNode(tacTest, null, "a", "42");
            Utils.AddAssignmentNode(tacTest, null, "x", "13");
            Utils.AddAssignmentNode(tacTest, null, "t1", "a", "-", "1");
            Utils.AddAssignmentNode(tacTest, null, "t2", "30", "*", "t1");
            Utils.AddAssignmentNode(tacTest, null, "t3", "a", "+", "t2");
            Utils.AddAssignmentNode(tacTest, null, "b", "t3");
            Utils.AddAssignmentNode(tacTest, null, "t4", "x", "+", "10");
            Utils.AddAssignmentNode(tacTest, null, "t5", "b", ">", "t4");
            Utils.AddIfGotoNode(tacTest, null, "L1", "t5");
            Utils.AddAssignmentNode(tacTest, null, "a", "9");
            Utils.AddAssignmentNode(tacTest, null, "x", "100");
            Utils.AddGotoNode(tacTest, null, "L2");
            Utils.AddAssignmentNode(tacTest, "L1", "a", "8");
            Utils.AddEmptyNode(tacTest, "L2");

            Assert.AreEqual(tacVisitor.TACodeContainer.ToString(), tacTest.ToString());

        }
    }
}