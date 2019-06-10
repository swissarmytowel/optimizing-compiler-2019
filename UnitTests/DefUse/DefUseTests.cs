using SimpleLang.CFG;
using SimpleLang.TACode;
using SimpleLang.DefUse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.TACode.TacNodes;

namespace UnitTests.DefUse
{
    [TestClass]
    public class DefUseTests
    {
        [TestMethod]
        public void DefUse_Test1()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "a", "b", "+", "c");
            Utils.AddAssignmentNode(tacContainer, null, "b", "a", "-", "d");
            Utils.AddAssignmentNode(tacContainer, null, "v", "l", "+", "c");
            Utils.AddAssignmentNode(tacContainer, null, "d", "a", "-", "d");
            var cfg = new ControlFlowGraph(tacContainer);
            var defUseContainers = DefUseForBlocksGenerator.Execute(cfg.SourceBasicBlocks);
            var container = defUseContainers.First().Value;
            Assert.IsTrue(
                container.GetSecondSet().SetEquals(new HashSet<TacNode>() {
                new TacNodeVarDecorator { VarName = "a" },
                new TacNodeVarDecorator { VarName = "v" }
                })
            );
            Assert.IsTrue(
                container.GetFirstSet().SetEquals(new HashSet<TacNode>() {
                    new TacNodeVarDecorator { VarName = "b" },
                    new TacNodeVarDecorator { VarName = "c" },
                    new TacNodeVarDecorator { VarName = "d" },
                    new TacNodeVarDecorator { VarName = "l" }
                })
            );
        }

        [TestMethod]
        public void DefUse_Test2()
        {
            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "t1", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a1", "t1");
            Utils.AddAssignmentNode(tacContainer, null, "t2", "b");
            Utils.AddAssignmentNode(tacContainer, null, "i", "1");
            Utils.AddIfGotoNode(tacContainer, null, "L1", "t2");
            Utils.AddGotoNode(tacContainer, null, "L2");
            Utils.AddAssignmentNode(tacContainer, "L1", "t3", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, "L2", "a3", "t3");
            Utils.AddAssignmentNode(tacContainer, null, "t4", "4", "*", "i");
            Utils.AddAssignmentNode(tacContainer, null, "a2", "t4");
            var cfg = new ControlFlowGraph(tacContainer);
            var defUseContainers = DefUseForBlocksGenerator.Execute(cfg.SourceBasicBlocks);
            var containers = defUseContainers.Select(e => e.Value).ToList();
            Assert.IsTrue(
                containers[0].GetSecondSet().SetEquals(new HashSet<TacNode>() {
                    new TacNodeVarDecorator { VarName = "a1" },
                    new TacNodeVarDecorator { VarName = "t1" },
                    new TacNodeVarDecorator { VarName = "t2" }
                })
            );
            Assert.IsTrue(
                containers[0].GetFirstSet().SetEquals(new HashSet<TacNode>() {
                    new TacNodeVarDecorator { VarName = "b" },
                    new TacNodeVarDecorator { VarName = "i" }
                })
            );
            Assert.IsTrue(containers[1].GetSecondSet().Count == 0);
            Assert.IsTrue(containers[1].GetFirstSet().Count == 0);
            Assert.IsTrue(
                containers[2].GetSecondSet().SetEquals(new HashSet<TacNode>() {
                    new TacNodeVarDecorator { VarName = "t3" }
                })
            );
            Assert.IsTrue(
                containers[2].GetFirstSet().SetEquals(new HashSet<TacNode>() {
                    new TacNodeVarDecorator { VarName = "i" }
                })
            );
            Assert.IsTrue(
                containers[3].GetSecondSet().SetEquals(new HashSet<TacNode>() {
                    new TacNodeVarDecorator { VarName = "a2" },
                    new TacNodeVarDecorator { VarName = "a3" },
                    new TacNodeVarDecorator { VarName = "t4" }
                })
            );
            Assert.IsTrue(
                containers[3].GetFirstSet().SetEquals(new HashSet<TacNode>() {
                    new TacNodeVarDecorator { VarName = "i" },
                    new TacNodeVarDecorator { VarName = "t3" }
                })
            );
        }
    }
}
