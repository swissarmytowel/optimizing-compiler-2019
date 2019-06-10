using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;

namespace UnitTests.Visitors
{
    [TestClass]
    public class SameMinusOptVisitorTest
    {
        [TestMethod]
        public void VisitorTest()
        {
            var source = "c = a - a; \na = b - b;\n";
            /*
             * c = a - a;
             * a = b - b;
             */

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var SameMinusVisitor = new SameMinusOptVisitor();
            parser.root.Visit(SameMinusVisitor);

            var expected = "c = 0;\na = 0;\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }
    }
}
