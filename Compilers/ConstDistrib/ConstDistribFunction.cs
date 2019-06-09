using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.ConstDistrib
{
    public class ConstDistribFunction : ITransmissionFunction<SemilatticeStreamValue>
    {
        private ThreeAddressCode basicBlock;

        private bool IsSimpleAssignNode(TacAssignmentNode assign)
        {
            return assign.Operation == null && assign.SecondOperand == null;
        }

        // Использует итерационный алгоритм для задачи распространения констант OUT[B] = fB(IN[B])
        // На вход получает _in Stream из которого создается DataStreamValue - Поток данных(реализация на множествах для ITA)
        // Выход новый Stream после изменения значений для присваиваний из tACodeLines
        // где SemilatticeStreamValue - обёртка над именем переменной и её значением из полурешётки констант SemilatticeValue
        // где DataStreamTable - Поток данных(реализация на словаре для реализации алгоритма и более удобной работы)
        public HashSet<SemilatticeStreamValue> Calculate(HashSet<SemilatticeStreamValue> _in, ThreeAddressCode tACodeLines)
        {
            basicBlock = tACodeLines;
            var result = new HashSet<SemilatticeStreamValue>();
            var dataStreamValue1 = new DataStreamValue(_in);
            var dataStreamValue2 = new DataStreamValue();
            foreach (TacNode node in tACodeLines)
            {
                if (node is TacAssignmentNode assign)
                {
                    var idVar = assign.LeftPartIdentifier;
                    var value1 = assign.FirstOperand;
                    var operation = assign.Operation;
                    var value2 = assign.SecondOperand;
                    var table = new DataStreamTable(dataStreamValue1.Stream, dataStreamValue2.Stream);
                    if (IsSimpleAssignNode(assign))
                        dataStreamValue2.ChangeStreamValue(idVar, table.GetValue(value1));
                    else
                    {
                        var semVal1 = table.GetValue(value1);
                        var semVal2 = table.GetValue(value2);
                        if (semVal1.TypeValue == SemilatticeValueEnum.CONST && semVal2.TypeValue == SemilatticeValueEnum.CONST)
                        {
                            double val1 = double.Parse(semVal1.ConstValue);
                            double val2 = double.Parse(semVal2.ConstValue);
                            double val3 = 0;
                            switch (operation)
                            {
                                case "+":
                                    val3 = val1 + val2;
                                    break;
                                case "-":
                                    val3 = val1 - val2;
                                    break;
                                case "/":
                                    val3 = val1 / val2;
                                    break;
                                case "*":
                                    val3 = val1 * val2;
                                    break;
                            }
                            dataStreamValue2.ChangeStreamValue(idVar, new SemilatticeValue(SemilatticeValueEnum.CONST, val3.ToString()));
                        }
                        else if (semVal1.TypeValue == SemilatticeValueEnum.NAC || semVal2.TypeValue == SemilatticeValueEnum.NAC)
                        {
                            dataStreamValue2.ChangeStreamValue(idVar, new SemilatticeValue(SemilatticeValueEnum.NAC));
                        }
                        else
                        {
                            dataStreamValue2.ChangeStreamValue(idVar, new SemilatticeValue(SemilatticeValueEnum.UNDEF));
                        }
                    }
                }
            }
            var dataStreamValue3 = dataStreamValue1 ^ dataStreamValue2;
            return dataStreamValue3.Stream;
        }

        public ThreeAddressCode GetBasicBlock()
        {
            return basicBlock;
        }

        public HashSet<SemilatticeStreamValue> GetLineGen(SemilatticeStreamValue tacNode)
        {
            throw new NotImplementedException();
        }

        public HashSet<SemilatticeStreamValue> GetLineKill(SemilatticeStreamValue tacNode)
        {
            throw new NotImplementedException();
        }
    }
}
