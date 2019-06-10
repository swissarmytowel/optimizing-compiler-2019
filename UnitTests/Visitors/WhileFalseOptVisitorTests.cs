using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;


namespace UnitTests.Visitors
{
    [TestClass]
    public class WhileFalseOptVisitorTest
    {
        [TestMethod]
        public void VisitorTest()
        {
            var source = "a = 777; while(false)\n a = 42; \n";
            /*
             * a = 777
             * while(false)
             *    a = 42;
             */

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var WhileFalseVisitor = new WhileFalseOptVisitor();
            parser.root.Visit(WhileFalseVisitor);

            var expected = "a = 777;\n;\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }
    }
}
