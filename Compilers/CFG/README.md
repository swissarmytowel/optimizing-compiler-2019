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
digraph G {
0 [label="Basic block:
L1: c = False  
L2: t1 = 8 + 90
L3: k = t1  
L4: d = 0  
L5: bm = False  
L6: p = True  
"];
1 [label="Basic block:
L7:
L8: m = 3  
"];
0 -> 1 [];
}
```

### Класс `BidirectionalGraph<TVertex, TEdge>`

Ссылка на документацию класса `BidirectionalGraph<TVertex, TEdge>`: [yaccconstructor.github.io](http://yaccconstructor.github.io/QuickGraph/reference/quickgraph-bidirectionalgraph-2.html).

Примеры методов:
1. `IsInEdgesEmpty(TVertex vertex)` и `IsOutEdgesEmpty(TVertex vertex)`;
2. `InEdges(TVertex vertex)` и `OutEdges(TVertex vertex)`;
3. `OutDegree(TVertex vertex)` и `InDegree(TVertex vertex)`. 

Ссылка на полную документацию библиотеки: [yaccconstructor.github.io](http://yaccconstructor.github.io/QuickGraph/reference/).
