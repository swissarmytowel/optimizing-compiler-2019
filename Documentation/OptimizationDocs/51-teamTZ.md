# Классификация ребер в CFG.

## Постановка задачи
Дан ControlFlowGraph. Все его рёбра необходимо классифицировать на три группы:

- Наступающие (coming) рёбра идут от узла к его истинному потомку.
- Отступающие (retreating) рёбра идут от узла к его предку.
- Поперечные (coming) - все остальные рёбра.

## Команда — исполнитель
TZ

## Зависимости
Зависит от:
- Построение ControlFlowGraph
- Построение глубинного остовного дерева


## Теория
![](../images/50-TZ.png)
Пример классификации:

Рёбра 0 -> 1 и 1 -> 2 являются наступающими, 0 -> 1 - отступающие, 3 -> 1 -поперечное.

Для решения данной задачи строится глубинное остовное дерево - обход "поиск в глубину" вершин ControlFlowGraph, начиная с первой вершины. Отметим, что в процессе обхода графа вершину нумеруются согласно Те рёбра, которые попали в данный граф, являются наступающими.

Ребро из вершины x в вершину y будет являться отступающим в том случае, если вершина y является предком вершины x (или номер вершины x больше номера вершины y).

## Реализация
Вначале был создан класс-перечисление, в котором определяются классы рёбер:
```csharp
public enum EdgeType
{
    Coming = 1,
    Retreating = 2,
    Cross = 3
}
```
Добавлено поле EdgeTypes являющееся словарем, где ключ - ребро ControlFlowGraph, а значение - тип ребра
```csharp
public Dictionary<Edge<ThreeAddressCode>, EdgeType> EdgeTypes { get; set; }
```

Для проверки существования обратного пути по DST была создна функция FindBackwardPath
```csharp
private bool FindBackwardPath(ThreeAddressCode source, ThreeAddressCode target)
        {
            var result = false;
            var containVertex = DSTree.ContainsVertex(source);
            var incomingEdges = containVertex ? DSTree.InEdges(source) : new List<Edge<ThreeAddressCode>>();
            while (incomingEdges.Any())
            {
                var edge = incomingEdges.First();
                if (edge.Source.Equals(target))
                {
                    result = true;
                    break;
                }

                incomingEdges = DSTree.InEdges(edge.Source);
            }

            return result;
        }
```

Наконец приведём алгоритм работы данной задачи:
```csharp
public void ClassificateEdges(ControlFlowGraph cfg)
        {
            DSTree = new DepthSpanningTree(cfg);
            
            foreach (var edge in cfg.Edges)
            {
                if (DSTree.Edges.Any(e => e.Target.Equals(edge.Target) && e.Source.Equals(edge.Source)))
                {
                    EdgeTypes.Add(edge, EdgeType.Coming);
                }
                else if (FindBackwardPath( edge.Source, edge.Target))
                {
                    EdgeTypes.Add(edge, EdgeType.Retreating);
                }
                else
                {
                    EdgeTypes.Add(edge, EdgeType.Cross);
                }
            }
        }
```
## Вывод
Используя метод, описанные выше, мы смогли классифицировать ребра в глубинном остовном дереве. 
