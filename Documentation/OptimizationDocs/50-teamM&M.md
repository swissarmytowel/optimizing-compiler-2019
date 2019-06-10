# Передаточная функция в задаче о распространении констант.

## Постановка задачи
Реализовать передаточную функцию для задачи распространения констант.
## Команда — исполнитель
M&M

## Зависимости
Зависит от  
- Базовых блоков  
- Трехадресного кода  
- Графа потока управления
- Оператора сбора /\ и отображение m в задаче о распространении констант 

## Теория

Каждая переменная в некоторой таблице имеет одно из значений в полурешетке - _UNDEF_ (undefigned), _const_, _NAC_ (not a const). Таблица является декартовым произведением полурешеток, и следовательно, сама полурешетка. Таким образом, элементом данных будет отображение _m_ на соответствующее значение полурешетки.

1.  Если s не является присваиванием, то f тождественна, т.е. m = m’
    
2.  Если s присваивание, то для каждого v != x: m’(v) = m(v)
    
3.  Если s присваивание константе, то m’(x) = const

5.  Если s: x=y+z, то
	   - m’(x) = m(y) + m(z), если m(y) и m(z) - const
	   -  m’(x) = NAC, если m(y) или m(z) - NAC 
	   - m’(x) = UNDEF в остальных случаях
## Реализация

Для решения поставленной задачи был реализован следующий метод :

```csharp
...
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
...
```

## Тесты
```csharp
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
```

## Вывод
Используя методы, описанные выше, мы получили передаточную функцию в задачи распространения констант.
