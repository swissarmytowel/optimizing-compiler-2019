using System.Collections.Generic;
using System.Linq;

namespace SimpleLang.ConstDistrib
{
    public struct SemilatticeStreamValue
    {
        public string VarName;
        public SemilatticeValue Value;

        public SemilatticeStreamValue(string varName, SemilatticeValue value)
        {
            VarName = varName;
            Value = value;
        }

        public override string ToString()
        {
            return $"var={VarName} => {Value}";
        }
    }

    public class DataStreamValue
    {
        public HashSet<SemilatticeStreamValue> Stream;

        public DataStreamValue()
        {
            Stream = new HashSet<SemilatticeStreamValue>();
        }

        public DataStreamValue(HashSet<SemilatticeStreamValue> stream)
        {
            if (stream == null)
                Stream = new HashSet<SemilatticeStreamValue>();
            else Stream = stream; 
        }

        public void ChangeStreamValue(string varName, SemilatticeValue value)
        {
            var table = CreateTableByStream(Stream);
            if (table.Keys.Contains(varName))
            {
                table[varName] = value;
                Stream = CreateStreamByTable(table);
            }
            else
            {
                Stream.Add(new SemilatticeStreamValue(varName, value));
            }
        }

        public static Dictionary<string, SemilatticeValue> CreateTableByStream(HashSet<SemilatticeStreamValue> stream)
        {
            var result = new Dictionary<string, SemilatticeValue>();
            foreach (var streamValue in stream)
                result.Add(streamValue.VarName, streamValue.Value);
            return result;
        }

        public static HashSet<SemilatticeStreamValue> CreateStreamByTable(Dictionary<string, SemilatticeValue> table)
        {
            var result = new HashSet<SemilatticeStreamValue>();
            foreach (var key in table.Keys)
                result.Add(new SemilatticeStreamValue(key, table[key]));
            return result;
        }

        public static DataStreamValue operator ^(DataStreamValue t1, DataStreamValue t2)
        {
            var result = new DataStreamValue();
            var table1 = CreateTableByStream(t1.Stream);
            var table2 = CreateTableByStream(t2.Stream);
            var varNames = table1.Keys.Union(table2.Keys);
            var table3 = new Dictionary<string, SemilatticeValue>();
            foreach (var varName in varNames)
            {
                var semilatticeValue = new SemilatticeValue(SemilatticeValueEnum.UNDEF);
                var isFirstVar = table1.Keys.Contains(varName);
                var isSecondVar = table2.Keys.Contains(varName);
                if (isFirstVar && !isSecondVar)
                    semilatticeValue = table1[varName] ^ semilatticeValue;
                else if (!isFirstVar && isSecondVar)
                    semilatticeValue = semilatticeValue ^ table2[varName];
                else if (isFirstVar && isSecondVar)
                    semilatticeValue = table1[varName] ^ table2[varName];
                table3.Add(varName, semilatticeValue);
            }
            result.Stream = CreateStreamByTable(table3);
            return result;
        }
    }
}
