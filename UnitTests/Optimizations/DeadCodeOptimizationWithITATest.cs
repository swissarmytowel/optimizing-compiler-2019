using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Visitors;
using SimpleLang.CFG;
using SimpleParser;
using SimpleScanner;
using SimpleLang.IterationAlgorithms;
using SimpleLang.DefUse;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class DeadCodeOptimizationWithITATests
    {
        [TestMethod]
        public void Optimize_SimpleBlock()
        {
            TmpNameManager.Instance.Drop();
            /*
             *  
	            x = a;  To be removed
                x = b;
                y = x + 1;
            */

            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "x", "a");
            Utils.AddAssignmentNode(tacContainer, null, "x", "b");
            Utils.AddAssignmentNode(tacContainer, null, "y", "x", "+", "1");
            Utils.AddAssignmentNode(tacContainer, null, "e", "d", "*", "a");

            var expectedResult = new ThreeAddressCode();
            Utils.AddAssignmentNode(expectedResult, null, "x", "b");
            Utils.AddAssignmentNode(expectedResult, null, "y", "x", "+", "1");
            Utils.AddAssignmentNode(expectedResult, null, "e", "d", "*", "a");

            var optimization = new DeadCodeOptimization();

            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
            isOptimized = optimization.Optimize(tacContainer);
            Assert.IsFalse(isOptimized);

        }

        [TestMethod]
        public void Optimize_CombinationOfBlocks()
        {
            /*
            x = b;
            x = a; --> Should be deleted despite of it's latest operation in block
            if (1==1)
            {
            x = b;
            y = x;
            }
            x = 1;
            v = x;
             */
            TmpNameManager.Instance.Drop();
            var tacVisitor = new ThreeAddressCodeVisitor();

            var expectedResult = "t1 = 1 == 1\nif t1 goto L1\n";
            var source = "x = b;\nx = a;\nif (1==1){\nx = b;\n y = x;\n}\n x = 1;\n v = x;\n";

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();
            var root = parser.root;

            root.Visit(tacVisitor);
            tacVisitor.Postprocess();

            var cfg = new ControlFlowGraph(tacVisitor.TACodeContainer);
            var defUseContainers = DefUseForBlocksGenerator.Execute(cfg.SourceBasicBlocks);
            var activeVariablesITA = new ActiveVariablesITA(cfg, defUseContainers);
            DeadCodeOptimizationWithITA optimization = new DeadCodeOptimizationWithITA();
            var isOptimized = optimization.Optimize(activeVariablesITA);
            Assert.AreEqual(expectedResult, activeVariablesITA.controlFlowGraph.SourceBasicBlocks.BasicBlockItems[0].ToString());

        }
    }
}
