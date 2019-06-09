# Построение дерева доминаторов 

## Постановка задачи
Для Control-flow graph построить дерево доминаторов на основе итерационного алгоритма.

## Команда — исполнитель
AW

## Зависимости
Зависит от:
- Трехадресный код (AW)
- Разбиение на базовые блоки (Enterprise)
- Обобщенный ИТА (ЗГ)
- Хранение IN-OUT (AW)

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
Узнать как должны выглядить тесты в докуметации.

## Вывод
Используя метод, описанные выше, было выполнено построение дерева доминаторов (всех и непосредственных) для Control-flow graph.

