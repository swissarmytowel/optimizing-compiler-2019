# Граф потока управления

Класс `ControlFlowGraph` является наследником класса `BidirectionalGraph<TVertex, TEdge>` из библиотеки `QuickGraph`, у которого:
1. Вершины (свойство `Vertices`) имеют тип `ThreeAddressCode` (тип элементов списка `BasicBlockItems` из класса `BasicBlocks`);
2. Дуги (свойство `Edges`) имеют тип `Edge<ThreeAddressCode>` и содержать свойства `Source` и `Target`, представляющие собой вершины, соедененные данной дугой.

### Построение графа

Построение графа осуществляется с помощью метода `Construct(...)`, который принимает программу в одном из следующих видов:
1. Трехадресный код (`ThreeAddressCode`);
2. Разбиение на базовые блоки (`BasicBlocks`).

### "Визуализация" графа

Получить своеобразную визуализацию графа можно с помощью следующих методов:
1. `ToString()`;
2. `SaveToFile(...)`.

Пример результата работы любого из методов:

```md
VERTICES
#0:
i = 1  

#1:
L1: j = 1  

#2:
L2: a = 1  
j = j + 1
t1 = j < 10
if t1 goto L2

#3:
i = i + 1
t2 = i < 10
if t2 goto L1

#4:
i = 1  

#5:
L3: a = 0  

#6:
i = i + 1
t3 = i < 10
if t3 goto L3

EDGES
0 -> [ 1 ]
1 -> [ 2 ]
2 -> [ 2 3 ]
3 -> [ 1 4 ]
4 -> [ 5 ]
5 -> [ 6 ]
6 -> [ 5 ]
```

### Класс `BidirectionalGraph<TVertex, TEdge>`

Ссылка на документацию класса `BidirectionalGraph<TVertex, TEdge>`: [yaccconstructor.github.io](http://yaccconstructor.github.io/QuickGraph/reference/quickgraph-bidirectionalgraph-2.html).

Примеры методов:
1. `IsInEdgesEmpty(TVertex vertex)` и `IsOutEdgesEmpty(TVertex vertex)`;
2. `InEdges(TVertex vertex)` и `OutEdges(TVertex vertex)`;
3. `OutDegree(TVertex vertex)` и `InDegree(TVertex vertex)`. 

Ссылка на полную документацию библиотеки: [yaccconstructor.github.io](http://yaccconstructor.github.io/QuickGraph/reference/).
