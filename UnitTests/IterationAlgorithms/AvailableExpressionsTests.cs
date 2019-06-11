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
using SimpleLang.E_GenKill.Implementations;
using SimpleLang.TACode.TacNodes;

namespace UnitTests.IterationAlgorithms
{   
    [TestClass]
    public class AvailableExpressionsTests
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
           L2: a = с 
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

            E_GenKillVisitor availExprVisitor = new E_GenKillVisitor();
            var availExprContainers = availExprVisitor.GenerateAvailableExpressionForBlocks(cfg.SourceBasicBlocks);

            var ita = new AvailableExpressionsITA(cfg, availExprContainers);

            var inData = ita.InOut.In;
            var outData = ita.InOut.Out;

            var expectedIn0 = new TacAssignmentNode();

            var expectedIn1 = new TacAssignmentNode();
            expectedIn1.LeftPartIdentifier = "t1";
            expectedIn1.FirstOperand = "b";

            var expectedOut0 = new TacAssignmentNode();
            expectedOut0.LeftPartIdentifier = "t1";
            expectedOut0.FirstOperand = "b";

            Assert.AreEqual(expectedIn1.ToString(), inData[cfg[1]].First().ToString());
            Assert.AreEqual(expectedIn1.ToString(), inData[cfg[2]].First().ToString());
            Assert.AreEqual(expectedIn1.ToString(), inData[cfg[3]].First().ToString());

            Assert.AreEqual(expectedOut0.ToString(), outData[cfg[1]].First().ToString());
            Assert.AreEqual(expectedOut0.ToString(), outData[cfg[2]].First().ToString());
            Assert.AreEqual(expectedOut0.ToString(), outData[cfg[3]].First().ToString());
        }

    }
}
