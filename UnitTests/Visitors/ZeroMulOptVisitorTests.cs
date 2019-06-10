using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;

namespace UnitTests.Visitors
{
    [TestClass]
    public class ZeroMulOptVisitorTests
    {
        [TestMethod]
        public void Visit_LeftOperatorZero()
        {
            var source = "c = 9;\na = 0 * c;\n";
            /*
             * c = 9;
             * a = 0 * c;
             */

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var zeroMulVisitor = new ZeroMulOptVisitor();
            parser.root.Visit(zeroMulVisitor);

            var expected = "c = 9;\na = 0;\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }

        [TestMethod]
        public void Visit_RightOperatorZero()
        {
            var source = "c = 9;\na = c * 0;\n";
            /*
             * c = 9;
             * a = c * 0;
             */

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var zeroMulVisitor = new ZeroMulOptVisitor();
            parser.root.Visit(zeroMulVisitor);

            var expected = "c = 9;\na = 0;\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }

        [TestMethod]
        public void Visit_NoZeroOperators()
        {
            var source = "c = 9;\na = c * 123;\n";
            /*
             * c = 9;
             * a = c * 123;
             */

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var zeroMulVisitor = new ZeroMulOptVisitor();
            parser.root.Visit(zeroMulVisitor);

            var expected = "c = 9;\na = (c*123);\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }
    }
}
