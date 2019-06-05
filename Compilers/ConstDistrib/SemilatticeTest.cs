using System.Diagnostics;


namespace SimpleLang.ConstDistrib
{
    static class SemilatticeTest
    {
        static public void TestForValueOperator ()
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
        
    }
}
