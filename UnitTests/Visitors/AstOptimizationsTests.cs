using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleParser;
using SimpleScanner;
using SimpleLang.Visitors;


namespace UnitTests.Visitors
{
    [TestClass]
    public class AstOptimizationsTests

    {
        [TestMethod]
        public void Visit_PlusZeroExprTest()
        {
            var source = "a = 1;\n" +
                "b = 1 + 0;\n" +
                "var1 = 0 + b;";

            var trueSource = "a = 1;\n" +
                "b = 1;\n" +
                "var1 = b;\n";

            var scanner1 = new Scanner();
            scanner1.SetSource(source, 0);

            var sourceParser = new Parser(scanner1);
            sourceParser.Parse();

            var fillParent = new FillParentVisitor();
            sourceParser.root.Visit(fillParent);

            var scanner2 = new Scanner();
            scanner2.SetSource(trueSource, 0);
            var trueSourceParser = new Parser(scanner2);
            trueSourceParser.Parse();

            var plusZeroExprVisitor = new PlusZeroExprVisitor();

            sourceParser.root.Visit(plusZeroExprVisitor);

            var printer1 = new PrettyPrintVisitor(true);
            var printer2 = new PrettyPrintVisitor(true);

            sourceParser.root.Visit(printer1);
            trueSourceParser.root.Visit(printer2);

            Console.WriteLine(printer1.Text);
            Console.WriteLine(printer2.Text);

            Assert.AreEqual(printer1.Text, printer2.Text);
        }
        [TestMethod]
        public void Visit_IfNodeWithBoolExprTest()
        {
            var source = "if(a == 1){\n" +
                "a = 3;\n" +
                "}else{\n" +
                "a = 4;\n" +
                "}\n" +
                "if(true){\n" +
                "a = 42;\n" +
                "}else{\n" +
                "a = 24;\n" +
                "}\n";

            var trueSource = "if(a == 1){\n" +
                "a = 3;\n" +
                "}else{\n" +
                "a = 4;\n" +
                "}\n" +
                "{a = 42;}\n";
                

            var scanner1 = new Scanner();
            scanner1.SetSource(source, 0);

            var sourceParser = new Parser(scanner1);
            sourceParser.Parse();

            var fillParent = new FillParentVisitor();
            sourceParser.root.Visit(fillParent);

            var scanner2 = new Scanner();
            scanner2.SetSource(trueSource, 0);
            var trueSourceParser = new Parser(scanner2);
            trueSourceParser.Parse();

            var plusZeroExprVisitor = new IfNodeWithBoolExprVisitor();

            sourceParser.root.Visit(plusZeroExprVisitor);

            var printer1 = new PrettyPrintVisitor(true);
            var printer2 = new PrettyPrintVisitor(true);

            sourceParser.root.Visit(printer1);
            trueSourceParser.root.Visit(printer2);

            Console.WriteLine(printer1.Text);
            Console.WriteLine(printer2.Text);

            Assert.AreEqual(printer1.Text, printer2.Text);
        }

        /// <summary>
        /// Test №3
        /// </summary>
        [TestMethod]
        public void Visit_ConstFoldingTests()
        {
            var source = "x = 50 * 10;";

            var trueSource = "x = 500;";

            var scanner1 = new Scanner();
            scanner1.SetSource(source, 0);

            var sourceParser = new Parser(scanner1);
            sourceParser.Parse();

            var fillParent = new FillParentVisitor();
            sourceParser.root.Visit(fillParent);

            var scanner2 = new Scanner();
            scanner2.SetSource(trueSource, 0);
            var trueSourceParser = new Parser(scanner2);
            trueSourceParser.Parse();

            var cfv = new ConstFoldingVisitor();

            sourceParser.root.Visit(cfv);

            var printer1 = new PrettyPrintVisitor(true);
            var printer2 = new PrettyPrintVisitor(true);

            sourceParser.root.Visit(printer1);
            trueSourceParser.root.Visit(printer2);

            Console.WriteLine(printer1.Text);
            Console.WriteLine(printer2.Text);

            Assert.AreEqual(printer1.Text, printer2.Text);
        }

        /// <summary>
        /// Test №13
        /// </summary>
        [TestMethod]
        public void Visit_DelOfDeadConditionsTests()
        {
            var source = "if(a == b){;}else{;}";

            var trueSource = ";";

            var scanner1 = new Scanner();
            scanner1.SetSource(source, 0);

            var sourceParser = new Parser(scanner1);
            sourceParser.Parse();

            var fillParent = new FillParentVisitor();
            sourceParser.root.Visit(fillParent);

            var scanner2 = new Scanner();
            scanner2.SetSource(trueSource, 0);
            var trueSourceParser = new Parser(scanner2);
            trueSourceParser.Parse();

            var dodcv = new DelOfDeadConditionsVisitor();

            sourceParser.root.Visit(dodcv);

            var printer1 = new PrettyPrintVisitor(true);
            var printer2 = new PrettyPrintVisitor(true);

            sourceParser.root.Visit(printer1);
            trueSourceParser.root.Visit(printer2);

            Console.WriteLine(printer1.Text);
            Console.WriteLine(printer2.Text);

            Assert.AreEqual(printer1.Text, printer2.Text);
        }
    }
}
