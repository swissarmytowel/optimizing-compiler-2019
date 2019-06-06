using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;

namespace UnitTests.Visitors
{
    [TestClass]
    public class ThreeAddressCodeTests
    {
        [TestMethod]
        public void Visit_NestedLoopWithCounter()
        {
            const string source = "a = 1;\n" +
                                  "for (i = 1 to 10)\n" +
                                  " for (j = 1 to 10)\n" +
                                  "  a = a + 1;";
            var scanner = new Scanner();
            scanner.SetSource(source, 0);

            /*
             * a = 1;
             * for (i = 1 to 10)
             *     for (j = 1 to 10)
             *         a = a + 1;
             */

            var expectedResult = Utils.GetNestedLoopWithCounterInTAC();

            var parser = new Parser(scanner);
            parser.Parse();
            var root = parser.root;
            var tacVisitor = new ThreeAddressCodeVisitor();
            root.Visit(tacVisitor);

            Assert.AreEqual(tacVisitor.TACodeContainer.ToString(), expectedResult.ToString());
        }
    }
}