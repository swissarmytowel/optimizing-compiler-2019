using SimpleLang.IterationAlgorithms.CollectionOperators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.ConstDistrib
{
    public class ConstDistribOperator : ICollectionOperator<SemilatticeStreamValue>
    {
        // Использует итерационный алгоритм для задачи распространения констант IN[B] = ^P-предшB OUT[P]
        // Принимает два потока firstSet и secondSet, создаёт на их основе DataStreamValue - Поток данных(реализация на множествах для ITA)
        // Далее применяет оператор сбора ^ для Потока данных и возвращает его поток
        public HashSet<SemilatticeStreamValue> Collect(HashSet<SemilatticeStreamValue> firstSet, HashSet<SemilatticeStreamValue> secondSet)
        {
            var dataStreamValue1 = new DataStreamValue(firstSet);
            var dataStreamValue2 = new DataStreamValue(secondSet);
            var dataStreamValue3 = dataStreamValue1 ^ dataStreamValue2;
            return dataStreamValue3.Stream;
        }
    }
}
