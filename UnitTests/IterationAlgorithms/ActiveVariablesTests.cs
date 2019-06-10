using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.TACode;
using SimpleLang.CFG;
using SimpleLang.IterationAlgorithms;
using SimpleLang.DefUse;

namespace UnitTests.IterationAlgorithms
{
    [TestClass]
    public class ActiveVariablesTests
    {

        public List<TacNodeVarDecorator> generateTacNodes(params string[] param)
        {
            return param.Select(e => new TacNodeVarDecorator { VarName = e }).ToList();
        }

        [TestMethod]
        public void Constructor_IfElse()
        {
            /*         
            Code:
            a = 1;
            if(b)
            {
	            a = 4;
            } else 
            {
	            c = 5;
            }
            a = c;
            c = 1;

            Tac:
            a = 1
            t1 = b
            if t1 goto L1   
            c = 5       
            goto L2    
            L1: a = 4   
            L2: a = 5 
            c = 1
            */

            var tacContainer = new ThreeAddressCode();

            //1
            Utils.AddAssignmentNode(tacContainer, null, "a", "1");
            Utils.AddAssignmentNode(tacContainer, null, "t1", "b");
            Utils.AddIfGotoNode(tacContainer, null, "L1", "t1");
            //2
            Utils.AddAssignmentNode(tacContainer, null, "c", "5");
            Utils.AddGotoNode(tacContainer, null, "L2");
            //3
            Utils.AddAssignmentNode(tacContainer, "L1", "a", "4");
            //4
            Utils.AddAssignmentNode(tacContainer, "L2", "a", "c");
            Utils.AddAssignmentNode(tacContainer, null, "c", "1");

            var cfg = new ControlFlowGraph(tacContainer);

            var defUseContainers = DefUseForBlocksGenerator.Execute(cfg.SourceBasicBlocks);
            var ita = new ActiveVariablesITA(cfg, defUseContainers);

            var inData = ita.InOut.In;
            var outData = ita.InOut.Out;

            var expectedIn0 = generateTacNodes("c");
            var expectedIn1 = generateTacNodes("c");
            var expectedIn2 = generateTacNodes();
            var expectedIn3 = generateTacNodes("c", "b");

            var expectedOut0 = generateTacNodes();
            var expectedOut1 = generateTacNodes("c");
            var expectedOut2 = generateTacNodes("c");
            var expectedOut3 = generateTacNodes("c");

            Assert.IsTrue(expectedIn3.All(e => inData[cfg[0]].Contains(e)));
            Assert.IsTrue(expectedIn2.All(e => inData[cfg[1]].Contains(e)));
            Assert.IsTrue(expectedIn1.All(e => inData[cfg[2]].Contains(e)));
            Assert.IsTrue(expectedIn0.All(e => inData[cfg[3]].Contains(e)));

            Assert.IsTrue(expectedOut3.All(e => outData[cfg[0]].Contains(e)));
            Assert.IsTrue(expectedOut2.All(e => outData[cfg[1]].Contains(e)));
            Assert.IsTrue(expectedOut1.All(e => outData[cfg[2]].Contains(e)));
            Assert.IsTrue(expectedOut0.All(e => outData[cfg[3]].Contains(e)));

        }


        [TestMethod]
        public void Constructor_IfWithNestedLoopAndNestedIf()
        {
            /* 
             * if (a < 5)
             *     for (j = 1 to 10)
             *         a = a + 1;
             * else
             *     if (a > 10)
             *         a = a - 10;
             * p = 2;
             */

            var tacContainer = new ThreeAddressCode();

            // basic block #0
            Utils.AddAssignmentNode(tacContainer, null, "t1", "a", "<", "5");
            Utils.AddIfGotoNode(tacContainer, null, "L1", "t1");
            // basic block #1
            Utils.AddAssignmentNode(tacContainer, null, "t2", "a", ">", "10");
            Utils.AddIfGotoNode(tacContainer, null, "L3", "t2");
            // basic block #2
            Utils.AddGotoNode(tacContainer, null, "L4");
            // basic block #3
            Utils.AddAssignmentNode(tacContainer, "L3", "t3", "a", "-", "10");
            Utils.AddAssignmentNode(tacContainer, null, "a", "t3");
            // basic block #4
            Utils.AddGotoNode(tacContainer, "L4", "L2");
            // basic block #5
            Utils.AddAssignmentNode(tacContainer, "L1", "j", "1");
            // basic block #6
            Utils.AddAssignmentNode(tacContainer, "L5", "t4", "a", "+", "1");
            Utils.AddAssignmentNode(tacContainer, null, "a", "t4");
            Utils.AddAssignmentNode(tacContainer, null, "j", "j", "+", "1");
            Utils.AddAssignmentNode(tacContainer, null, "t5", "j", "<", "10");
            Utils.AddIfGotoNode(tacContainer, null, "L5", "t5");
            // basic block #7
            Utils.AddAssignmentNode(tacContainer, "L2", "p", "2");

            var cfg = new ControlFlowGraph(tacContainer);

            var defUseContainers = DefUseForBlocksGenerator.Execute(cfg.SourceBasicBlocks);
            var ita = new ActiveVariablesITA(cfg, defUseContainers);

            var inData = ita.InOut.In;
            var outData = ita.InOut.Out;

            var expectedIn0 = generateTacNodes();
            var expectedIn1 = generateTacNodes("a");
            var expectedIn2 = generateTacNodes("a");
            var expectedIn3 = generateTacNodes();
            var expectedIn4 = generateTacNodes("a");
            var expectedIn5 = generateTacNodes();
            var expectedIn6 = generateTacNodes("a");
            var expectedIn7 = generateTacNodes("a");

            var expectedOut0 = generateTacNodes();
            var expectedOut1 = generateTacNodes("a");
            var expectedOut2 = generateTacNodes("a");
            var expectedOut3 = generateTacNodes();
            var expectedOut4 = generateTacNodes();
            var expectedOut5 = generateTacNodes();
            var expectedOut6 = generateTacNodes("a");
            var expectedOut7 = generateTacNodes("a");

            Assert.IsTrue(expectedIn7.All(e => inData[cfg[0]].Contains(e)));
            Assert.IsTrue(expectedIn6.All(e => inData[cfg[1]].Contains(e)));
            Assert.IsTrue(expectedIn5.All(e => inData[cfg[2]].Contains(e)));
            Assert.IsTrue(expectedIn4.All(e => inData[cfg[3]].Contains(e)));
            Assert.IsTrue(expectedIn3.All(e => inData[cfg[4]].Contains(e)));
            Assert.IsTrue(expectedIn2.All(e => inData[cfg[5]].Contains(e)));
            Assert.IsTrue(expectedIn1.All(e => inData[cfg[6]].Contains(e)));
            Assert.IsTrue(expectedIn0.All(e => inData[cfg[7]].Contains(e)));

            Assert.IsTrue(expectedOut7.All(e => outData[cfg[0]].Contains(e)));
            Assert.IsTrue(expectedOut6.All(e => outData[cfg[1]].Contains(e)));
            Assert.IsTrue(expectedOut5.All(e => outData[cfg[2]].Contains(e)));
            Assert.IsTrue(expectedOut4.All(e => outData[cfg[3]].Contains(e)));
            Assert.IsTrue(expectedOut3.All(e => outData[cfg[4]].Contains(e)));
            Assert.IsTrue(expectedOut2.All(e => outData[cfg[5]].Contains(e)));
            Assert.IsTrue(expectedOut1.All(e => outData[cfg[6]].Contains(e)));
            Assert.IsTrue(expectedOut0.All(e => outData[cfg[7]].Contains(e)));

        }
    }
}
