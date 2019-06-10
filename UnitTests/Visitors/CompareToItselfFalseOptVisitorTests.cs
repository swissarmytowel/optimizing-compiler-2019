using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;

namespace UnitTests.Visitors
{
    [TestClass]
    public class CompareToItselfFalseOptVisitorTests
    {
        [TestMethod]
        public void Visit_GreaterComparision()
        {
            var source = "c = 9;\na = c > c;\n";
            /*
             * c = 9;
             * a = c > c;
             */

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var zeroMulVisitor = new CompareToItselfFalseOptVisitor();
            parser.root.Visit(zeroMulVisitor);

            var expected = "c = 9;\na = False;\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }

        [TestMethod]
        public void Visit_NonEqualityComparision()
        {
            var source = "c = 9;\na = c != c;\n";
            /*
             * c = 9;
             * a = c != c;
             */

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var zeroMulVisitor = new CompareToItselfFalseOptVisitor();
            parser.root.Visit(zeroMulVisitor);

            var expected = "c = 9;\na = False;\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }

        [TestMethod]
        public void Visit_NotCorrectOperator()
        {
            var source = "c = 9;\na = c == c;\n";
            /*
             * c = 9;
             * a = c == c;
             */

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var zeroMulVisitor = new CompareToItselfFalseOptVisitor();
            parser.root.Visit(zeroMulVisitor);

            var expected = "c = 9;\na = (c==c);\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }
    }
}
