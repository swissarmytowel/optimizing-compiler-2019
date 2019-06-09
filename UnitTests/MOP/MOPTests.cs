using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.CFG;
using SimpleLang.DefUse;
using SimpleLang.GenKill.Implementations;
using SimpleLang.IterationAlgorithms.CollectionOperators;
using SimpleLang.MOP;
using SimpleLang.TacBasicBlocks;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;

namespace UnitTests.MOP
{
    [TestClass]
    public class MOPTests
    {
        // Causes an error when comparing temporary variable names

        //[TestMethod]
        //public void Compute_CheckLastBlockReachingDefinitions()
        //{
        //    var source = "c = 123;\nm = 1 + c + y;\nif (m > 2) {\n    c = 3;\n} else {\n    c = 456;\n}\na = 11;";

        //    /*
        //     * c = 123;
        //     * m = 1 + c + y;       
        //     * if (m > 2) {
        //     *   c = 3;
        //     * } else {
        //     *   c = 456;
        //     * }
        //     * a = 11;            
        //    */

        //    var scanner = new Scanner();
        //    scanner.SetSource(source, 0);
        //    var parser = new Parser(scanner);
        //    parser.Parse();

        //    var parentv = new FillParentVisitor();
        //    parser.root.Visit(parentv);

        //    var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
        //    parser.root.Visit(threeAddressCodeVisitor);
        //    threeAddressCodeVisitor.Postprocess();

        //    var cfg = new ControlFlowGraph(threeAddressCodeVisitor.TACodeContainer);
        //    var genkillVisitor = new GenKillVisitor();

        //    var basicBlocks = new BasicBlocks();
        //    basicBlocks.SplitTACode(cfg.SourceCode);

        //    var genKill = genkillVisitor.GenerateReachingDefinitionForBlocks(cfg.SourceBasicBlocks);
        //    var tfFunction = new TFByComposition(genKill);

        //    var collectionOperator = new UnionCollectionOperator<TacNode>();

        //    var mop = new MeetOverPaths(cfg, tfFunction, collectionOperator, new HashSet<TacNode>());
        //    mop.Compute();
        //    var ita = new ReachingDefinitionsITA(cfg, genKill);

        //    var expectedIn = new List<string>
        //    {
        //        "c = 456  ",
        //        "t1 = 1 + c",
        //        "t2 = t1 + y",
        //        "m = t2  ",
        //        "t3 = m > 2",
        //        "c = 123  ",
        //        "L1: c = 3  "
        //    };

        //    var outputIn = mop.InOut.In[cfg.SourceBasicBlocks.BasicBlockItems[3]]
        //        .Select(x => x.ToString()).ToList();

        //    Assert.IsTrue(expectedIn.SequenceEqual(outputIn));

        //    var expectedOut = new List<string>
        //    {
        //        "L2: a = 11  ",
        //        "c = 456  ",
        //        "t1 = 1 + c",
        //        "t2 = t1 + y",
        //        "m = t2  ",
        //        "t3 = m > 2",
        //        "c = 123  ",
        //        "L1: c = 3  "
        //    };
        //    var outputOut = mop.InOut.Out[cfg.SourceBasicBlocks.BasicBlockItems[3]]
        //        .Select(x => x.ToString()).ToList();

        //    Assert.AreEqual(expectedOut, outputOut); 
        //}

        [TestMethod]
        public void Compute_CheckFirstBlockActiveVariables()
        {
            var source = "c = 123;\nm = 1 + c + y;\nif (m > 2) {\n    c = 3;\n} else {\n    c = 456;\n}\na = 11;";

            /*
             * c = 123;
             * m = 1 + c + y;       
             * if (m > 2) {
             *   c = 3;
             * } else {
             *   c = 456;
             * }
             * a = 11;            
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

            var cfg = new ControlFlowGraph(threeAddressCodeVisitor.TACodeContainer);

            var basicBlocks = new BasicBlocks();
            basicBlocks.SplitTACode(cfg.SourceCode);

            var defUseContainers = DefUseForBlocksGenerator.Execute(cfg.SourceBasicBlocks);
            DefUseForBlocksPrinter.Execute(defUseContainers);

            var tfFunction = new TFByComposition(defUseContainers);

            var collectionOperator = new UnionCollectionOperator<TacNode>();

            var mop = new MeetOverPaths(cfg, tfFunction, collectionOperator, new HashSet<TacNode>(), false);
            mop.Compute();

            var expectedIn = new List<string> { "y" };
            var outputIn = mop.InOut.In[cfg.SourceBasicBlocks.BasicBlockItems[0]]
                .Select(x => x.ToString()).ToList();

            Assert.IsTrue(expectedIn.SequenceEqual(outputIn));

            var outputOut = mop.InOut.Out[cfg.SourceBasicBlocks.BasicBlockItems[0]]
                .Select(x => x.ToString()).ToList();
            Assert.AreEqual(0, outputOut.Count);
        }
    }
}
