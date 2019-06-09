using NUnit.Framework;
using System;
using SimpleParser;
using SimpleScanner;
using SimpleLang.Visitors;

namespace SimpleLang.Tests
{
    public class CompareToItselfFalseOptVisitorTest
    {
        [Test]
        public void GreaterComparasion()
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

        [Test]
        public void NonEqualityComparasion()
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

        [Test]
        public void NotCorrectOperator()
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
