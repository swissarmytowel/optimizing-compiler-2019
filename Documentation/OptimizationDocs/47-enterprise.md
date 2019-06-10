# Поиск решения методом MOP.

## Постановка задачи
Задача состояла в реализации поиска решения методом MOP

## Команда — исполнитель
Enterprise

## Зависимости
Данная задача зависит от задачи реализации оператора сбора. 

## Теория
Из-за того, что поиск всех возможных путей выполнения - задача неразрешимая, следовательно требуется поиск приближенного решения.
Решение сбором по путям (Meet Over Paths, MOP) – это решение, получаемое применением оператора сбора по всем путям, ведущим ко входу в блок B. Основная форма метода MOP: 
![MOP](../images/46-enterprise.jpg)

Рассматриваемые в решении MOP пути представляют собой надмножетсво всех путей, которые могут быть выполнены, следовательно MOP[B] включает больше путей, чем IDEAL[B], в том числе заведомо невыполнимые пути, поэтому `MOP[B] ≤ IDEAL[B]`. 

Решение максимальной фиксированной точки (MFP) – это решение IN[B] ,
полученное итерационным алгоритмом: `MOP[B] ≤ IDEAL[B]`.

## Реализация
В процессе решения задачи был создан класс, реализующий интерфейс _IIterationAlgorithm_. В нем присутствует главный метод _Compute_, в котором происходит обход по всему графу потока управления:
```csharp
    while (isChanged)
            {
                var predecessors = new Stack<ThreeAddressCode>();
                var outBefore = new Dictionary<ThreeAddressCode, HashSet<TacNode>>(InOut.Out);
                if (IsForwardDirection)
                    DepthFirstSearch(entryBlock, visited, predecessors, ControlFlowGraph.IsOutEdgesEmpty, ControlFlowGraph.OutEdges);
                else DepthFirstSearch(entryBlock, visited, predecessors, ControlFlowGraph.IsInEdgesEmpty, ControlFlowGraph.InEdges);

                isChanged = false;
                foreach (var _out in InOut.Out)
                {
                    var key = _out.Key;
                    isChanged = isChanged || !_out.Value.SequenceEqual(outBefore[key]);
                }
            }
```
В этом участке кода вызывается метод _DepthFirstSearch_, который обходит граф потока управления в глубину:
```csharp
visited[currentBlock] = true;

            var collectionOperatorResult = new HashSet<TacNode>();
            if (predecessors.Count > 0)
                collectionOperatorResult = predecessors
                .Select(e => InOut.Out[e])
                .Aggregate((a, b) => CollectionOperator.Collect(a, b));

            InOut.In[currentBlock] = CollectionOperator.Collect(collectionOperatorResult, InOut.In[currentBlock]);
            var outBefore = InOut.Out[currentBlock];
            InOut.Out[currentBlock] = TransmissionFunction.Calculate(InOut.In[currentBlock], currentBlock);

```

## Тесты
Граф потока управления:
```
VERTICES
#0:
c = 123  
t4 = 1 + c
t5 = t4 + y
m = t5  
t6 = m > 2
if t6 goto L3

#1:
c = 456  
goto L4

#2:
L3: c = 3  

#3:
L4: a = 11  

EDGES
0 -> [ 1 2 ]
1 -> [ 3 ]
2 -> [ 3 ]
3 -> [ ]

```
IN и OUT для достигающий определений последнего базового блока:
```
IN:
    c=456
    t1=1+c
    t2=t1+y
    m=t2
    t3=m>2
    c=123
    L1: c=3
OUT:
    L2: a=11
    c=456
    t1=1+c
    t2=t1+y
    m=t2
    t3=m>2
    c=123
    L1: c=3
```

IN и OUT для активных переменных для первого базового блока:
```
IN: y
OUT: 
```

## Вывод
В результате работы был написан класс, который реализует поиск решения методом MOP. Его работоспособность была успешно протестирована.
