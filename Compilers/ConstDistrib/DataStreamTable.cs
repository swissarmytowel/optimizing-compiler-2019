﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.ConstDistrib
{
    // Поток данных(реализация на словаре для реализации алгоритма и более удобной работы)
    // Использует DataStreamValue(Поток данных на множествах) в процессе своей работы
    public class DataStreamTable
    {
        private Dictionary<string, SemilatticeValue> Table;

        public DataStreamTable()
        {
            Table = new Dictionary<string, SemilatticeValue>();
        }

        // Инициализация потоком
        public DataStreamTable(HashSet<SemilatticeStreamValue> stream)
        {
            if (stream == null) Table = new Dictionary<string, SemilatticeValue>();
            else Table = DataStreamValue.CreateTableByStream(stream);
        }

        public DataStreamTable(HashSet<SemilatticeStreamValue> stream1, HashSet<SemilatticeStreamValue> stream2)
        {
            Table = new Dictionary<string, SemilatticeValue>();
            foreach (var streamValue in stream1)
                Table.Add(streamValue.VarName, streamValue.Value);
            foreach (var streamValue in stream2)
            {
                if (Table.Keys.Contains(streamValue.VarName))
                    Table[streamValue.VarName] = streamValue.Value;
                else Table.Add(streamValue.VarName, streamValue.Value);
            }
        }

        // Метод получения значение из полурешетки констант по имени переменной
        public SemilatticeValue GetValue(string varName)
        {
            bool IsVariable = Utility.Utility.IsVariable(varName);
            if (IsVariable)
            {
                if (Table.Keys.Contains(varName))
                    return Table[varName];
                else return new SemilatticeValue(null);
            }
            else return new SemilatticeValue(varName);
        }

        // Оператор сбора для Потока данных
        public static DataStreamTable operator ^(DataStreamTable t1, DataStreamTable t2)
        {
            var stream1 = new DataStreamValue(DataStreamValue.CreateStreamByTable(t1.Table));
            var stream2 = new DataStreamValue(DataStreamValue.CreateStreamByTable(t2.Table));
            var stream3 = stream1 ^ stream2;
            return new DataStreamTable(stream3.Stream);
        }
    }
}
