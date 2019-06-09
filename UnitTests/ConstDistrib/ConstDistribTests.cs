using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLang.ConstDistrib;
using SimpleLang.TACode;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UnitTests.Optimizations
{
    [TestClass]
    public class ConstDistribTests
    {
        [TestMethod]
        static public void TestForOperator()
        {
            var constDistribOperator = new ConstDistribOperator();
            var set1 = new HashSet<SemilatticeStreamValue>()
            {
                new SemilatticeStreamValue("a", new SemilatticeValue(SemilatticeValueEnum.CONST, "12")),
                new SemilatticeStreamValue("b", new SemilatticeValue(SemilatticeValueEnum.CONST, "13")),
                new SemilatticeStreamValue("c", new SemilatticeValue(SemilatticeValueEnum.CONST, "14"))
            };
            var set2 = new HashSet<SemilatticeStreamValue>()
            {
                new SemilatticeStreamValue("a", new SemilatticeValue(SemilatticeValueEnum.CONST, "14")),
                new SemilatticeStreamValue("b", new SemilatticeValue(SemilatticeValueEnum.CONST, "13")),
                new SemilatticeStreamValue("c", new SemilatticeValue(SemilatticeValueEnum.UNDEF))
            };
            var set3 = constDistribOperator.Collect(set1, set2);
            var table3 = DataStreamValue.CreateTableByStream(set3);
            Debug.Assert(table3["a"] == new SemilatticeValue(SemilatticeValueEnum.NAC));
            Debug.Assert(table3["b"] == new SemilatticeValue(SemilatticeValueEnum.CONST, "13"));
            Debug.Assert(table3["c"] == new SemilatticeValue(SemilatticeValueEnum.CONST, "14"));
        }

        private ThreeAddressCode GetCodeLinesByText(string text)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(text, 0);
            Parser parser = new Parser(scanner);
            var b = parser.Parse();
            var r = parser.root;
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            r.Visit(threeAddressCodeVisitor);
            return threeAddressCodeVisitor.TACodeContainer;
        }

        [TestMethod]
        public void TestForFunction()
        {
            var constDistribFun = new ConstDistribFunction();
            var emptySet = new HashSet<SemilatticeStreamValue>();
            var f1 = constDistribFun.Calculate(emptySet, GetCodeLinesByText("x=2;y=3;"));
            var f1Table = new DataStreamTable(f1);
            Debug.Assert(f1Table.GetValue("x") == new SemilatticeValue(SemilatticeValueEnum.CONST, "2"));
            Debug.Assert(f1Table.GetValue("y") == new SemilatticeValue(SemilatticeValueEnum.CONST, "3"));
            Debug.Assert(f1Table.GetValue("z") == new SemilatticeValue(SemilatticeValueEnum.UNDEF));
            var f2 = constDistribFun.Calculate(emptySet, GetCodeLinesByText("x=3;y=2;"));
            var f2Table = new DataStreamTable(f2);
            Debug.Assert(f2Table.GetValue("x") == new SemilatticeValue(SemilatticeValueEnum.CONST, "3"));
            Debug.Assert(f2Table.GetValue("y") == new SemilatticeValue(SemilatticeValueEnum.CONST, "2"));
            Debug.Assert(f2Table.GetValue("z") == new SemilatticeValue(SemilatticeValueEnum.UNDEF));
            var f1f2 = constDistribFun.Calculate(f1, GetCodeLinesByText("x=3;y=2;"));
            var f1f2Table = f1Table ^ f2Table;
            Debug.Assert(f1f2Table.GetValue("x") == new SemilatticeValue(SemilatticeValueEnum.NAC));
            Debug.Assert(f1f2Table.GetValue("y") == new SemilatticeValue(SemilatticeValueEnum.NAC));
            Debug.Assert(f1f2Table.GetValue("z") == new SemilatticeValue(SemilatticeValueEnum.UNDEF));
            var f3f1f2 = constDistribFun.Calculate(f1f2, GetCodeLinesByText("z=x+y;"));
            var f3f1f2Table = new DataStreamTable(f3f1f2);
            Debug.Assert(f3f1f2Table.GetValue("x") == new SemilatticeValue(SemilatticeValueEnum.NAC));
            Debug.Assert(f3f1f2Table.GetValue("y") == new SemilatticeValue(SemilatticeValueEnum.NAC));
            Debug.Assert(f3f1f2Table.GetValue("z") == new SemilatticeValue(SemilatticeValueEnum.NAC));
            var f3f1 = constDistribFun.Calculate(f1, GetCodeLinesByText("z=x+y;"));
            var f3f1Table = new DataStreamTable(f3f1);
            Debug.Assert(f3f1Table.GetValue("x") == new SemilatticeValue(SemilatticeValueEnum.CONST, "2"));
            Debug.Assert(f3f1Table.GetValue("y") == new SemilatticeValue(SemilatticeValueEnum.CONST, "3"));
            Debug.Assert(f3f1Table.GetValue("z") == new SemilatticeValue(SemilatticeValueEnum.CONST, "5"));
            var f3f2 = constDistribFun.Calculate(f2, GetCodeLinesByText("z=x+y;"));
            var f3f2Table = new DataStreamTable(f3f2);
            Debug.Assert(f3f2Table.GetValue("x") == new SemilatticeValue(SemilatticeValueEnum.CONST, "3"));
            Debug.Assert(f3f2Table.GetValue("y") == new SemilatticeValue(SemilatticeValueEnum.CONST, "2"));
            Debug.Assert(f3f2Table.GetValue("z") == new SemilatticeValue(SemilatticeValueEnum.CONST, "5"));
            var f3f1ANDf3f2Table = f3f1Table ^ f3f2Table;
            Debug.Assert(f3f1ANDf3f2Table.GetValue("x") == new SemilatticeValue(SemilatticeValueEnum.NAC));
            Debug.Assert(f3f1ANDf3f2Table.GetValue("y") == new SemilatticeValue(SemilatticeValueEnum.NAC));
            Debug.Assert(f3f1ANDf3f2Table.GetValue("z") == new SemilatticeValue(SemilatticeValueEnum.CONST, "5"));
        }
    }
}
