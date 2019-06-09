using NUnit.Framework;
using System;

using SimpleParser;
using SimpleScanner;
using SimpleLang.Visitors;
using SimpleLang.TacBasicBlocks;

namespace SimpleLang.Tests
{
    public class BasicBlocksTest
    {
        [Test]
        public void OneBasicBlock()
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

            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            parser.root.Visit(threeAddressCodeVisitor);
            threeAddressCodeVisitor.Postprocess();

            var bblocks = new BasicBlocks();
            bblocks.SplitTACode(threeAddressCodeVisitor.TACodeContainer);
            Assert.AreEqual(1, bblocks.BasicBlockItems.Count);
        }

        [Test]
        public void FourBasicBlocks()
        {
            var source = "a = 123;" +
            	"if (a > 444) {" +
            	"g = 34;" +
            	"}";
            /*
             * a = 123;
             * if (a > 44) {
             *   g = 34;
             * }
             */

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            parser.root.Visit(threeAddressCodeVisitor);
            threeAddressCodeVisitor.Postprocess();

            var bblocks = new BasicBlocks();
            bblocks.SplitTACode(threeAddressCodeVisitor.TACodeContainer);
            Assert.AreEqual(4, bblocks.BasicBlockItems.Count);
        }

        [Test]
        public void ThreeBasicBlocks()
        {
            var source = "goto l 7:" +
                "g = 34;" +
                "l 7:";
            /*
             * goto l 7;
             * g = 34;
             * l 7:
             */

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);

            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            parser.root.Visit(threeAddressCodeVisitor);
            threeAddressCodeVisitor.Postprocess();

            var bblocks = new BasicBlocks();
            bblocks.SplitTACode(threeAddressCodeVisitor.TACodeContainer);
            Assert.AreEqual(3, bblocks.BasicBlockItems.Count);
        }
    }
}
