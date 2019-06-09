using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class DefUseBBlocksTests
    {
        [TestMethod]
        public void DefUseBBlocks_ConstPropagationTest()
        {
            TmpNameManager.Instance.Drop();
            var tacVisitor = new ThreeAddressCodeVisitor();

            var expectedResult = "a = 42  \nt1 = 42 + 10\nx = t1  \nz = 1  \nt2 = x + y\nb = t2  \nt3 = 42 * 1\nt4 = t3 / x\nt5 = 100 + t4\nv = t5  \nvar1 = 1  \n";
            var source = "a = 42;\nx = a + 10;\nz = 1;\nb = x + y;\nv = 100 + (a * z) / x;\nvar1 = z;\n";

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();
            var root = parser.root;

            root.Visit(tacVisitor);
            tacVisitor.Postprocess();

            var defUseConstPropagation = new DefUseConstPropagation();
            var result = defUseConstPropagation.Optimize(tacVisitor.TACodeContainer);
            while(result)
            {
                result = defUseConstPropagation.Optimize(tacVisitor.TACodeContainer);
            }

            Assert.AreEqual(expectedResult, tacVisitor.TACodeContainer.ToString());
        }

        [TestMethod]
        public void DefUseBBlocks_CopyPropagationTest()
        {
            TmpNameManager.Instance.Drop();
            var tacVisitor = new ThreeAddressCodeVisitor();

            var expectedResult = "a = 42  \nt1 = a + 10\nx = t1  \nz = 1  \nt2 = t1 + y\nb = t2  \nt3 = a * z\nt4 = t3 / t1\nt5 = 100 + t4\nv = t5  \nt6 = t1 + t2\nt7 = z + t6\nvar1 = t7  \n";
            var source = "a = 42;\nx = a + 10;\nz = 1;\nb = x + y;\nv = 100 + (a * z) / x;\nvar1 = z + (x + b);\n";

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();
            var root = parser.root;

            root.Visit(tacVisitor);
            tacVisitor.Postprocess();

            var defUseConstPropagation = new DefUseCopyPropagation();
            var result = defUseConstPropagation.Optimize(tacVisitor.TACodeContainer);
            while (result)
            {
                result = defUseConstPropagation.Optimize(tacVisitor.TACodeContainer);
            }

            Assert.AreEqual(expectedResult, tacVisitor.TACodeContainer.ToString());
        }
    }
}
