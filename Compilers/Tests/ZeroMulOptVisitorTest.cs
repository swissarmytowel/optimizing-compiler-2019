using NUnit.Framework;
using System;
using SimpleParser;
using SimpleScanner;
using SimpleLang.Visitors;

namespace SimpleLang.Tests
{
    public class ZeroMulOptVisitorTest
    {
        [Test]
        public void LeftOperatorZero()
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
            var root = parser.root;

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var zeroMulVisitor = new ZeroMulOptVisitor();
            parser.root.Visit(zeroMulVisitor);

            var expected = "c = 9;\na = 0;\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }

        [Test]
        public void RightOperatorZero()
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
            var root = parser.root;

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var zeroMulVisitor = new ZeroMulOptVisitor();
            parser.root.Visit(zeroMulVisitor);

            var expected = "c = 9;\na = 0;\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }

        [Test]
        public void NoZeroOperators()
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
            var root = parser.root;

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var zeroMulVisitor = new ZeroMulOptVisitor();
            parser.root.Visit(zeroMulVisitor);

            var expected = "c = 9;\na = (c*123);\n";
            Assert.AreEqual(expected, parser.root.ToString());
        }
    }
}
