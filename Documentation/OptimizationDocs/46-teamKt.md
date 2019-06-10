# Оператор сбора /\ и отображение m в задаче о распространении констант.

## Постановка задачи
Реализовать оператор сбора /\ и отображение m в задаче о распространении констант
## Команда — исполнитель
Kt

## Зависимости
Зависит от  
- Базовых блоков  
- Трехадресного кода  
- Графа потока управления 

## Теория
Для одной переменной x – значения в полурешётке V:

1. все константы данного типа
2. NAC (Not A Constant) – либо переменной было присвоена не
константа, либо по разным веткам – разные константы
3. UNDEF (неизвестно пока, является ли константой)

Оператор сбора на полурешетке V:

- UNDEF ∧ v = v (v − переменная)
- NAC ∧ v = NAC
- c ∧ c = c
- c1 ∧ c2 = NAC
## Реализация

Для решения поставленной задачи был реализован следующий метод :

```csharp
...
// Реализация оператора ^
public static SemilatticeValue operator ^(SemilatticeValue c1, SemilatticeValue c2)
{
	if (c1.TypeValue == SemilatticeValueEnum.UNDEF && c2.TypeValue == SemilatticeValueEnum.UNDEF)
		return new SemilatticeValue(SemilatticeValueEnum.UNDEF);
	if (c1.TypeValue == SemilatticeValueEnum.CONST && c2.TypeValue == SemilatticeValueEnum.UNDEF)
		return new SemilatticeValue(SemilatticeValueEnum.CONST, c1.ConstValue);
	if (c1.TypeValue == SemilatticeValueEnum.UNDEF && c2.TypeValue == SemilatticeValueEnum.CONST)
		return new SemilatticeValue(SemilatticeValueEnum.CONST, c2.ConstValue);
	if (c1.TypeValue == SemilatticeValueEnum.CONST && c2.TypeValue == SemilatticeValueEnum.CONST && c1.ConstValue == c2.ConstValue)
		return new SemilatticeValue(SemilatticeValueEnum.CONST, c2.ConstValue);
	if (c1.TypeValue == SemilatticeValueEnum.CONST && c2.TypeValue == SemilatticeValueEnum.CONST && c1.ConstValue != c2.ConstValue)
		return new SemilatticeValue(SemilatticeValueEnum.NAC);
	if (c1.TypeValue == SemilatticeValueEnum.NAC || c2.TypeValue == SemilatticeValueEnum.NAC)
		return new SemilatticeValue(SemilatticeValueEnum.NAC);
	return new SemilatticeValue(SemilatticeValueEnum.UNDEF);
}
...
```

## Тесты
```csharp
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
}
```

## Вывод
Используя методы, описанные выше, были реализованы оператор сбора /\ и отображение m в задаче о распространении констант.
