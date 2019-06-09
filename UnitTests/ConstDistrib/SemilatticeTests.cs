using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleLang.ConstDistrib;

namespace UnitTests.ConstDistrib
{
    [TestClass]
    public class SemilatticeTests
    {
        [TestMethod]
        public void TestForValueOperator ()
        {
            var undef = new SemilatticeValue("");
            Debug.Assert(undef.TypeValue == SemilatticeValueEnum.UNDEF && undef.ConstValue == null);
            var c1 = new SemilatticeValue("123");
            Debug.Assert(c1.TypeValue == SemilatticeValueEnum.CONST && c1.ConstValue == "123");
            var c2 = new SemilatticeValue("456.45");
            Debug.Assert(c2.TypeValue == SemilatticeValueEnum.CONST && c2.ConstValue == "456.45");
            var c3 = new SemilatticeValue("-123");
            Debug.Assert(c3.TypeValue == SemilatticeValueEnum.CONST && c3.ConstValue == "-123");
            var c4 = new SemilatticeValue("123");
            Debug.Assert(c4.TypeValue == SemilatticeValueEnum.CONST && c4.ConstValue == "123");
            var nac = new SemilatticeValue("fun()");
            Debug.Assert(nac.TypeValue == SemilatticeValueEnum.NAC && nac.ConstValue == null);

            var t = c1 ^ undef;
            Debug.Assert(t.TypeValue == SemilatticeValueEnum.CONST && t.ConstValue == "123");
            t = undef ^ c1;
            Debug.Assert(t.TypeValue == SemilatticeValueEnum.CONST && t.ConstValue == "123");
            t = undef ^ nac;
            Debug.Assert(t.TypeValue == SemilatticeValueEnum.NAC && t.ConstValue == null);
            t = nac ^ undef;
            Debug.Assert(t.TypeValue == SemilatticeValueEnum.NAC && t.ConstValue == null);
            t = nac ^ nac;
            Debug.Assert(t.TypeValue == SemilatticeValueEnum.NAC && t.ConstValue == null);
            t = undef ^ undef;
            Debug.Assert(t.TypeValue == SemilatticeValueEnum.UNDEF && t.ConstValue == null);
            t = c1 ^ c2;
            Debug.Assert(t.TypeValue == SemilatticeValueEnum.NAC && t.ConstValue == null);
            t = c1 ^ c4;
            Debug.Assert(t.TypeValue == SemilatticeValueEnum.CONST && t.ConstValue == "123");
            t = c1 ^ nac;
            Debug.Assert(t.TypeValue == SemilatticeValueEnum.NAC && t.ConstValue == null);
            t = nac ^ c2;
            Debug.Assert(t.TypeValue == SemilatticeValueEnum.NAC && t.ConstValue == null);
        }

        [TestMethod]
        public void TestForStreamValueOperator()
        {
            var set1 = new HashSet<SemilatticeStreamValue>()
            {
                new SemilatticeStreamValue("a", new SemilatticeValue(SemilatticeValueEnum.CONST, "12")),
                new SemilatticeStreamValue("b", new SemilatticeValue(SemilatticeValueEnum.CONST, "13")),
                new SemilatticeStreamValue("c", new SemilatticeValue(SemilatticeValueEnum.CONST, "14"))
            };
            var dataStreamValue1 = new DataStreamValue(set1);
            var set2 = new HashSet<SemilatticeStreamValue>()
            {
                new SemilatticeStreamValue("a", new SemilatticeValue(SemilatticeValueEnum.CONST, "12")),
                new SemilatticeStreamValue("b", new SemilatticeValue(SemilatticeValueEnum.CONST, "13")),
                new SemilatticeStreamValue("c", new SemilatticeValue(SemilatticeValueEnum.CONST, "14"))
            };
            var dataStreamValue2 = new DataStreamValue(set2);
            var dataStreamValue3 = dataStreamValue1 ^ dataStreamValue2;
            var table3 = DataStreamValue.CreateTableByStream(dataStreamValue3.Stream);
            Debug.Assert(table3["a"] == new SemilatticeValue(SemilatticeValueEnum.CONST, "12"));
            Debug.Assert(table3["b"] == new SemilatticeValue(SemilatticeValueEnum.CONST, "13"));
            Debug.Assert(table3["c"] == new SemilatticeValue(SemilatticeValueEnum.CONST, "14"));

            set1 = new HashSet<SemilatticeStreamValue>()
            {
                new SemilatticeStreamValue("a", new SemilatticeValue(SemilatticeValueEnum.CONST, "12")),
                new SemilatticeStreamValue("b", new SemilatticeValue(SemilatticeValueEnum.CONST, "13")),
                new SemilatticeStreamValue("c", new SemilatticeValue(SemilatticeValueEnum.CONST, "14"))
            };
            dataStreamValue1 = new DataStreamValue(set1);
            set2 = new HashSet<SemilatticeStreamValue>()
            {
                new SemilatticeStreamValue("a", new SemilatticeValue(SemilatticeValueEnum.CONST, "14")),
                new SemilatticeStreamValue("b", new SemilatticeValue(SemilatticeValueEnum.CONST, "13")),
                new SemilatticeStreamValue("c", new SemilatticeValue(SemilatticeValueEnum.UNDEF))
            };
            dataStreamValue2 = new DataStreamValue(set2);
            dataStreamValue3 = dataStreamValue1 ^ dataStreamValue2;
            table3 = DataStreamValue.CreateTableByStream(dataStreamValue3.Stream);
            Debug.Assert(table3["a"] == new SemilatticeValue(SemilatticeValueEnum.NAC));
            Debug.Assert(table3["b"] == new SemilatticeValue(SemilatticeValueEnum.CONST, "13"));
            Debug.Assert(table3["c"] == new SemilatticeValue(SemilatticeValueEnum.CONST, "14"));
        }

    }
}
