# Построение дерева доминаторов 

## Постановка задачи
Для Control-flow graph построить дерево доминаторов на основе итерационного алгоритма.

## Команда — исполнитель
AW

## Зависимости
Зависит от:
- Трехадресный код 
- Разбиение на базовые блоки
- Обобщенный ИТА
- Хранение IN-OUT

## Теория
Пусть d, n &mdash; вершины Control-flow graph. Будем говорить, что d dom n
(d ***доминирует*** над n) если любой путь от входного узла к n проходит
через d.

Среди всех доминаторов узла будем выделять ***непосредственный
доминатор***: m idom n, обладающий следующими свойствами:
m dom n, m ≠ n и если d dom n, d ≠ n, то d dom m.

***Дерево доминаторов*** &mdash; вспомогательная структура данных, содержащая информацию об отношениях доминирования. 
При этом дуга от узла M к узлу N идет тогда и только тогда, когда M является непосредственным доминатором N.

![](../images/46-teamAW-1.PNG)

Примечание: здесь p - непосредственно предшествующие блоки.

## Реализация

Для решения данной задачи были созданы два множества доминаторов: 
1. Множество всех доминаторов для каждого базового блока;

```csharp
/// <summary>
/// Все доминаторы
/// Ключ - блок
/// Значение - Все доминаторы текущего блока
/// </summary>
public Dictionary<ThreeAddressCode, HashSet<ThreeAddressCode>> Dominators =
    new Dictionary<ThreeAddressCode, HashSet<ThreeAddressCode>>();
```

2. Множество непосредственных доминаторов для каждого базового блока.

```csharp
/// <summary>
/// Непосредственные доминаторы
/// Ключ - блок
/// Значение - непосредственный доминатор
/// </summary>
public Dictionary<ThreeAddressCode, ThreeAddressCode> ImmediateDominators =
    new Dictionary<ThreeAddressCode, ThreeAddressCode>();
```
---
Оператор сбора имеет вид:
```csharp
/// <summary>
/// Оператор сбора
/// </summary>
private ICollectionOperator<ThreeAddressCode> _collectionOperator =
    new IntersectCollectionOperator<ThreeAddressCode>();
```

Метод для построения дерева доминаторов:
```csharp
// entryPoint - входной базовый блок
// threeAddressCodeHashSet - все базовые блоки, кроме входного
// outWasChanged - переменная, которая показывает, были ли произведены 
// какие-либо измения с множеством Out какого-либо базового блока 

public DominatorsFinder(ControlFlowGraph cfg)
{
    ...

    // Для входного узла In - пустой, Out - сам базовый блок
    InOut.In[entryPoint] = new HashSet<ThreeAddressCode>();
    InOut.Out.Add(entryPoint, new HashSet<ThreeAddressCode>() {entryPoint});
    ImmediateDominators[entryPoint] = entryPoint;

    // Для всех ББл, кроме входного, Out явл. множеством всех ББл, кроме входного
    foreach (var basicBlock in cfg.SourceBasicBlocks)
    {
        if (basicBlock == entryPoint) continue;
        InOut.Out[basicBlock] = threeAddressCodeHashSet; 
    }

    ...

    // Цикл: пока вносятся изменения в Out хотя бы одного базового блока
    while (outWasChanged)
    {
        outWasChanged = false;
        for (var i = 1; i < vertices.Count; ++i)
        {
            var curBlock = vertices[i];

            // Предки текущего узла
            var ancestors = cfg.Edges.Where(edge => edge.Target == curBlock).Select(e => e.Source).ToList();

#region All Dominators 
            InOut.In[curBlock] = new HashSet<ThreeAddressCode>();
            InOut.In[curBlock] = InOut.Out[ancestors[0]];

            // Если несколько непосредственных предков
            if (ancestors.Count > 1) { 
                for (int ind = 1; ind < ancestors.Count; ++ind) {
                    if (curBlock == ancestors[ind]) continue;
                    InOut.In[curBlock] = _collectionOperator.Collect(InOut.In[curBlock], InOut.Out[ancestors[ind]]);
                }
            }

            InOut.Out[curBlock] = new HashSet<ThreeAddressCode>(InOut.In[curBlock].Union(new HashSet<ThreeAddressCode>(){curBlock}));
#endregion
            ...

#region Immediate Dominators
            // Находим непосредственных доминаоров, т.е. доминаторов не являющихся:
            // 1) рассматриваемым узлом;
            // 2) доминатором над каким-либо из доминаторов данного узла
            var immediateDomsCurBlock = InOut.Out[curBlock].Where(block => block != curBlock).Select(p => p).ToList();
            var needToDeleteElems = new List<ThreeAddressCode>();
            foreach (var dom in immediateDomsCurBlock) {
                if (immediateDomsCurBlock.Contains(ImmediateDominators[dom]) && ImmediateDominators[dom] != dom) {
                    needToDeleteElems.Add(ImmediateDominators[dom]);
                }
            }
            needToDeleteElems.Distinct();
            ImmediateDominators[curBlock] = immediateDomsCurBlock
                .Where(d => !needToDeleteElems.Contains(d))
                .FirstOrDefault();
#endregion
            ...
        }
    }
    ImmediateDominators[entryPoint] = null;
    Dominators = InOut.Out;
}
```

## Тесты
#### INPUT: 
```csharp
c = 2 > 3;
a = 120;
tmp1 = a;
tmp = 101;
if (c) { 
  b = 3; 
}
i = 0;
for ( i = 0 to 3) { 
  c = c + 1;
  b = c + tmp;
  d = a + 1; 
}
b = a + c;
a = 2;
c = 0; 
a = 11;
```

#### Three address code:
```csharp
t1 = 2 > 3
c = t1
a = 120
tmp1 = a
tmp = 101
t2 = c
if t2 goto L1
goto L2
L1: b = 3
L2: i = 0
i = 0
L3: t3 = c + 1
c = t3
t4 = c + tmp
b = t4
t5 = a + 1
d = t5
i = i + 1
t6 = i < 3
if t6 goto L3
t7 = a + c
b = t7
a = 2
c = 0
a = 11
```

### Control-flow graph
Вершины Control-flow graph являются базовыми блоками программы. 
```csharp
VERTICES
#0:
t1 = 2 > 3
c = t1
a = 120
tmp1 = a
tmp = 101
t2 = c
if t2 goto L1

#1:
goto L2

#2:
L1: b = 3

#3:
L2: i = 0
i = 0

#4:
L3: t3 = c + 1
c = t3
t4 = c + tmp
b = t4
t5 = a + 1
d = t5
i = i + 1
t6 = i < 3
if t6 goto L3

#5:
t7 = a + c
b = t7
a = 2
c = 0
a = 11

EDGES
0 -> [ 1 2 ]
1 -> [ 3 ]
2 -> [ 3 ]
3 -> [ 4 ]
4 -> [ 5 4 ]
5 -> [ ]
```

#### OUTPUT:

Все доминаторы для каждого базового блока:
```csharp
Block 0:
Dominators:
t1 = 2 > 3
c = t1
a = 120
tmp1 = 120
tmp = 101
t2 = c
if t2 goto L1
```
```csharp
Block 1:
Dominators:
t1 = 2 > 3
c = t1
a = 120
tmp1 = 120
tmp = 101
t2 = c
if t2 goto L1

goto L2
```
```csharp
Block 2:
Dominators:
t1 = 2 > 3
c = t1
a = 120
tmp1 = 120
tmp = 101
t2 = c
if t2 goto L1

L1: b = 3
```
```csharp
Block 3:
Dominators:
t1 = 2 > 3
c = t1
a = 120
tmp1 = 120
tmp = 101
t2 = c
if t2 goto L1

L2: i = 0
i = 0
```
```csharp
Block 4:
Dominators:
t1 = 2 > 3
c = t1
a = 120
tmp1 = 120
tmp = 101
t2 = c
if t2 goto L1

L2: i = 0
i = 0

L3: t3 = c + 1
c = t3
t4 = c + 101
b = t4
t5 = 120 + 1
d = t5
i = i + 1
t6 = i < 3
if t6 goto L3
```
```csharp
Block 5:
Dominators:
t1 = 2 > 3
c = t1
a = 120
tmp1 = 120
tmp = 101
t2 = c
if t2 goto L1

L2: i = 0
i = 0

L3: t3 = c + 1
c = t3
t4 = c + 101
b = t4
t5 = 120 + 1
d = t5
i = i + 1
t6 = i < 3
if t6 goto L3

t7 = 120 + c
b = t7
a = 2
c = 0
a = 11
```
---
Непосредственные доминаторы для каждого базового блока:
```csharp
Block 0:
Immediate dominator:
null
```
```csharp
Block 1:
Immediate dominator:
t1 = 2 > 3
c = t1
a = 120
tmp1 = 120
tmp = 101
t2 = c
if t2 goto L1
```
```csharp
Block 2:
Immediate dominator:
t1 = 2 > 3
c = t1
a = 120
tmp1 = 120
tmp = 101
t2 = c
if t2 goto L1
```
```csharp
Block 3:
Immediate dominator:
t1 = 2 > 3
c = t1
a = 120
tmp1 = 120
tmp = 101
t2 = c
if t2 goto L1
```
```csharp
Block 4:
Immediate dominator:
L2: i = 0
i = 0
```
```csharp
Block 5:
Immediate dominator:
L3: t3 = c + 1
c = t3
t4 = c + 101
b = t4
t5 = 120 + 1
d = t5
i = i + 1
t6 = i < 3
if t6 goto L3
```

#### Упрощенный вывод доминаторов:
```csharp
blockInd0:  Dominators: 0        ImmediateDominator: null
blockInd1:  Dominators: 0 1      ImmediateDominator: 0
blockInd2:  Dominators: 0 2      ImmediateDominator: 0
blockInd3:  Dominators: 0 3      ImmediateDominator: 0
blockInd4:  Dominators: 0 3 4    ImmediateDominator: 3
blockInd5:  Dominators: 0 3 4 5  ImmediateDominator: 4
```

## Вывод
Используя метод, описанные выше, было выполнено построение дерева доминаторов (всех и непосредственных) для Control-flow graph.

