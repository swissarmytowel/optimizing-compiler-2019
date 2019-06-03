# Абстрактный класс двунаправленного графа BidirectionalGraph 

Класс основан на `BidirectionalGraph<TVertex, TEdge>` из библиотеки QuickGraph: [yaccconstructor.github.io](http://yaccconstructor.github.io/QuickGraph/reference/quickgraph-bidirectionalgraph-2.html).

## Свойства

1. Вершины `Vertices` (тип `ThreeAddressCode`), которые являются базовыми блоками, полученными из класса `BasicBlocks` (элементы списка `BasicBlockItems`);
2. Дуги `Edges` (тип `Edge<ThreeAddressCode>`), каждая из которых содержит информацию о вершинах, которые она соединяет (`Source` и `Target`);
3. VertexCount (тип `int`);
4. EdgeCount (тип `int`);
5. IsVerticesEmpty (тип `bool`);
6. IsEdgesEmpty (тип `bool`).

## Методы

1. `OutEdges(vertex)` — возвращает все дуги, выходящие из вершины `vertex`;
2. `InEdges(vertex)` — возвращает все дуги, входящие в вершину `vertex`;
3. `IsOutEdgesEmpty(vertex)`;
4. `IsInEdgesEmpty(vertex)`;
5. `OutDegree(vertex)`;
6. `InDegree(vertex)`;
7. `ContainsVertex(vertex)` — проверяет, содержит ли граф вершину `vertex`;
8. `ContainsEdge(edge)` — проверяет, содержит ли граф другу `edge`;
9. `SaveToFile(string fileName)`; 
10. `ToString()`.

# Класс графа потока управления ControlFlowGraph

Класс является наследником `BidirectionalGraph`. Построение графа осуществляется при создании объекта класса, а с помощью метода `Rebuild(tac)` можно перестроить уже существующий граф.

## Дополнительные свойства

1. `EntryBlock` — входной блок, т.е. вершина, через которую управление входит в граф;
2. `ExitBlock` — выходной блок, т.е. вершина, которая завершает все пути в графе;
3. `SourceCode` — исходный трехадресный код, по которому был составлен граф.

# Класс глубинного остовного дерева DepthSpanningTree

Класс также является наследником `BidirectionalGraph`. Построение графа осуществляется при создании объекта класса, а с помощью метода `Rebuild(cfg)` можно перестроить уже существующий граф.

# Примеры использования

```c#
var cfg = new ControlFlowGraph(threeAddressCodeVisitor.TACodeContainer);
foreach (var vertex in cfg.Vertices)
{
  // ...
}
```

```c#
Build(cfg.EntryBlock);

// ...

private void Build(ThreeAddressCode block)
{
    visitedBlocks.Add(block);
    var outEdges = cfg.OutEdges(block);
    foreach (var edge in outEdges)
    {
        // ...
        
        Build(edge.Target);
    }
}

```
