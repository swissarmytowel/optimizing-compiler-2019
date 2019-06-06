using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class LVNTests
    {
        /*

        TODO: update methods

        int countLabel = 1;
        Regex assignFullRegex = new Regex(@"\s*(\w+)\s*=\s*(\w+)\s*([+*-])\s*(\w+)\s*");
        Regex assignPartRegex = new Regex(@"\s*(\w+)\s*=\s*(\w+)\s*");

        public TacAssignmentNode parseStringToNode(string assignStat)
        {
            Match match = assignFullRegex.Match(assignStat);
            if (match.Success)
            {
                var strs = new List<string>();
                foreach (Group item in match.Groups)
                    strs.Add(item.Value);
                return new TacAssignmentNode()
                {
                    Label = $"L{countLabel++}",
                    LeftPartIdentifier = strs[1],
                    FirstOperand = strs[2],
                    Operation = strs[3],
                    SecondOperand = strs[4]
                };
            }
            else
            {
                match = assignPartRegex.Match(assignStat);
                if (match.Success)
                {
                    var strs = new List<string>();
                    foreach (Group item in match.Groups)
                        strs.Add(item.Value);
                    return new TacAssignmentNode()
                    {
                        Label = $"L{countLabel++}",
                        LeftPartIdentifier = strs[1],
                        FirstOperand = strs[2],
                    };
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void setLabelsToNull(ThreeAddressCodeVisitor tacVisitor)
        {
            var current = tacVisitor.TACodeContainer.TACodeLines.First;
            while (current != null)
            {
                current.Value.Label = null;
                current = current.Next;
            }
        }

        [TestMethod]
        public void Test_RightOptimized1()
        {
            countLabel = 1;
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("a = b + c"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("b = a - d"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("c = b + c"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("d = a - d"));

            countLabel = 1;
            var threeAddressCodeVisitorResult = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("a = b + c"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("b = a - d"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("c = b + c"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("d = b"));

            new LocalValueNumberingOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            Assert.AreEqual(threeAddressCodeVisitor.ToString(), threeAddressCodeVisitorResult.ToString());
        }

        [TestMethod]
        public void Test_IsOptimized()
        {
            countLabel = 1;
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("a = b + c"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("b = a - d"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("c = b + c"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("d = a - d"));
            var isOptimized = new LocalValueNumberingOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            Assert.IsTrue(isOptimized);
        }

        [TestMethod]
        public void Test_IsNotOptimized()
        {
            countLabel = 1;
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("a = b + c"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("b = a - d"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("c = b + c"));
            var isOptimized = new LocalValueNumberingOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            Assert.IsFalse(isOptimized);
        }

        [TestMethod]
        public void Test_RightOptimized2()
        {
            countLabel = 1;
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("a = b * c"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("d = b"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("e = d * c"));

            countLabel = 1;
            var threeAddressCodeVisitorResult = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("a = b * c"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("d = b"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("e = a"));

            new LocalValueNumberingOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            Assert.AreEqual(threeAddressCodeVisitor.ToString(), threeAddressCodeVisitorResult.ToString());
        }

        [TestMethod]
        public void Test_RightOptimizedReversedExprs()
        {
            countLabel = 1;
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("a = 1 + 2"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("b = 2 + 1"));


            countLabel = 1;
            var threeAddressCodeVisitorResult = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("a = 1 + 2"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("b = a"));

            new LocalValueNumberingOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            Assert.AreEqual(threeAddressCodeVisitor.ToString(), threeAddressCodeVisitorResult.ToString());
        }


        [TestMethod]
        public void Test_RightOptimizedWhenNoParamWithTheTypeCase()
        {
            countLabel = 1;
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("a = x + y"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("b = x + y"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("a = 17"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("b = 18"));
            threeAddressCodeVisitor.TACodeContainer.PushNode(parseStringToNode("c = x + y"));

            countLabel = 1;
            var threeAddressCodeVisitorResult = new ThreeAddressCodeVisitor();
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("a = x + y"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("b = a"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("t1 = a"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("a = 17"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("b = 18"));
            threeAddressCodeVisitorResult.TACodeContainer.PushNode(parseStringToNode("c = t1"));

            new LocalValueNumberingOptimization().Optimize(threeAddressCodeVisitor.TACodeContainer);

            setLabelsToNull(threeAddressCodeVisitor);
            setLabelsToNull(threeAddressCodeVisitorResult);
  
            Assert.AreEqual(threeAddressCodeVisitor.ToString(), threeAddressCodeVisitorResult.ToString());
        }
        */
    }
}
