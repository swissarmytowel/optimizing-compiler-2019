using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SimpleLang.ConstDistrib
{
    public enum SemilatticeValueEnum { UNDEF = 0, NAC = 1, CONST = 2 }

    class SemilatticeValue
    {
        public SemilatticeValueEnum TypeValue { get; private set; }
        public string ConstValue { get; private set; }

        public SemilatticeValue(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                TypeValue = SemilatticeValueEnum.UNDEF;
                ConstValue = null;
            }
            else
            {
                try
                {
                    Double.Parse(value, CultureInfo.InvariantCulture);
                    TypeValue = SemilatticeValueEnum.CONST;
                    ConstValue = value;
                }
                catch (Exception e)
                {
                    TypeValue = SemilatticeValueEnum.NAC;
                    ConstValue = null;
                }
            }
        }

        public SemilatticeValue(SemilatticeValueEnum typeValue, string constValue = null)
        {
            TypeValue = typeValue;
            ConstValue = constValue;
        }

        public static SemilatticeValue operator ^(SemilatticeValue c1, SemilatticeValue c2)
        {
            if (c1.TypeValue == SemilatticeValueEnum.UNDEF && c2.TypeValue == SemilatticeValueEnum.UNDEF)
                return new SemilatticeValue(SemilatticeValueEnum.UNDEF);
            if (c1.TypeValue == SemilatticeValueEnum.CONST && c2.TypeValue == SemilatticeValueEnum.UNDEF)
                return new SemilatticeValue(SemilatticeValueEnum.CONST, c1.ConstValue);
            if (c1.TypeValue == SemilatticeValueEnum.UNDEF && c2.TypeValue == SemilatticeValueEnum.CONST)
                return new SemilatticeValue(SemilatticeValueEnum.CONST, c2.ConstValue);
            if (c1.TypeValue == SemilatticeValueEnum.CONST && c2.TypeValue == SemilatticeValueEnum.CONST &&
                 c1.ConstValue == c2.ConstValue)
                return new SemilatticeValue(SemilatticeValueEnum.CONST, c2.ConstValue);
            if (c1.TypeValue == SemilatticeValueEnum.CONST && c2.TypeValue == SemilatticeValueEnum.CONST &&
                  c1.ConstValue != c2.ConstValue)
                return new SemilatticeValue(SemilatticeValueEnum.NAC);
            if (c1.TypeValue == SemilatticeValueEnum.NAC || c2.TypeValue == SemilatticeValueEnum.NAC)
                return new SemilatticeValue(SemilatticeValueEnum.NAC);
            return new SemilatticeValue(SemilatticeValueEnum.UNDEF);
        }
    }
}
