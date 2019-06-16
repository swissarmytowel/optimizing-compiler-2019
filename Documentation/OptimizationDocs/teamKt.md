
# Алгоритм нахождения итерационного фронта доминирования.

## Постановка задачи
Реализовать алгоритм нахождения итерационного фронта доминирования.

## Команда — исполнитель
Kt
## Зависимости
Зависит от  
- Базовых блоков  
- Трехадресного кода  
- Графа потока управления 

## Теория

**Опр.** Для узла *n* множество узлов *m*:
1. *n* является доминатором предшественника *m*:
	n ∊ Dom(p), где p ∊ Pred(m)
2.  *n* не является строгим доминатором  *m* (т. е. либо не доминирует, либо совпадает).

Алгоритм постраения DF(x) для всех x.
```
foreach n do 
	DF[n] = ∅
foreach n do
	if |Pred(n)| > 1 then
		foreach p in Pred(n) do
		{
			r = p
			while r ≠ IDom(n) do
			{
				DF[r] += n
				r = IDom(r)
			}
		}
```
## Реализация

Для решения поставленной задачи был реализован следующий метод:

```csharp
...
public static HashSet<ThreeAddressCode> Execute(ControlFlowGraph cfg, ThreeAddressCode n)
{
	var res = new HashSet<ThreeAddressCode>();
	var dominatorsFinder = new DominatorsFinder(cfg);
	var Pred = cfg.InEdges(n).Select(edge => edge.Source);
	var iDomN = dominatorsFinder.ImmediateDominators[n];
	if (Pred.Count() > 1)
	{
		foreach(var p in Pred)
		{
			var r = p;
			while(r != iDomN)
			{
				res.Add(r);
				r = dominatorsFinder.ImmediateDominators[r];
			}
		}
	}
	return res;
}
...
```
## Тесты

#### INPUT
```csharp
CFG:
VERTICES
#0:
a = 9
t1 = 9 > 5
if t1 goto L1

#1:
goto L2

#2:
L1: goto l1

#3:
L2: b = 8
t2 = 9 + 8
c = 17

#4:
l1: a = 15

EDGES
0 -> [ 1 2 ]
1 -> [ 3 ]
2 -> [ 4 ]
3 -> [ 4 ]
4 -> [ ]
```
#### OUTPUT
```csharp
DominationBorder:
vertex:
a = 9
t1 = 9 > 5
if t1 goto L1

borderSet:
Empty

vertex:
goto L2

borderSet:
Empty

vertex:
L1: goto l1

borderSet:
Empty

vertex:
L2: b = 8
t2 = 9 + 8
c = 17

borderSet:
Empty

vertex:
l1: a = 15

borderSet:
L1: goto l1
L2: b = 8
t2 = 9 + 8
c = 17
goto L2
```

## Вывод
Используя методы, описанные выше, мы реализовали алгоритм нахождения итерационного фронта доминирования.


