# Определение глубины графа потока данных

## Постановка задачи
Задача состояла в реализации поиска глубины CFG

## Команда — исполнитель
Enterprise

## Зависимости
Зависит от: 
- Построения графа потока данных 
- Классификации рёбер

## Теория
Глубиной графа потока управления CFG называется наибольшее количество отступающих рёбер по всем ациклическим путям.

Если граф приводим, то глубина не зависит от глубинного остовного дерева, так как отступающие рёбра равны обратным. 

## Реализация
Для вычисления глубины графа потока управления была написана функция, которая принимает на вход классифицированные ребра графа:
```csharp
public int GetDepth(Dictionary<Edge<ThreeAddressCode>, EdgeType> EdgeTypes)
        {
            var visitedEdges = new HashSet<Edge<ThreeAddressCode>>();
            return CalcDepth(EntryBlock, visitedEdges, EdgeTypes);
        }
```
Само вычисление глубины графа осуществлялось поиском в глубину. Ниже приведена рекурсивная функция:
```csharp
private int CalcDepth(ThreeAddressCode currentBlock, HashSet<Edge<ThreeAddressCode>> visitedEdges, 
                              Dictionary<Edge<ThreeAddressCode>, EdgeType> EdgeTypes)
        {
            var childrenDepths = new List<int>();

            foreach (var edge in OutEdges(currentBlock))
            {
                if (!visitedEdges.Contains(edge))
                {
                    visitedEdges.Add(edge);
                    if (EdgeTypes[edge] == EdgeType.Retreating)
                        childrenDepths.Add(1 + CalcDepth(edge.Target, visitedEdges, EdgeTypes));
                    else childrenDepths.Add(CalcDepth(edge.Target, visitedEdges, EdgeTypes));
                }
                visitedEdges.Remove(edge);
            }

            return childrenDepths.Count > 0 ? childrenDepths.Max() : 0;
        }
```
## Тесты

Исходный граф:
```
EDGES
0 -> [ 1 ]
1 -> [ 2 1 ]
2 -> [ 3 4 ]
3 -> [ 8 ]
4 -> [ 5 6 ]
5 -> [ 7 ]
6 -> [ 2 ]
7 -> [ 8 ]
8 -> [ ]
```
Полученная глубина: 2

Исходный граф:
```
EDGES
0 -> [ 2 ]
1 -> [ 2 ]
2 -> [ 3 4 ]
3 -> [ 5 ]
4 -> [ 5 ]
5 -> [ ]
```
Полученная глубина: 0

Исходный граф:
```
EDGES
0 -> [ 1 2 ]
1 -> [ 3 ]
2 -> [ 3 ]
3 -> [ 4 ]
4 -> [ 5 ]
5 -> [ 6 5 ]
6 -> [ 7 4 ]
7 -> [ 8 ]
8 -> [ 8 ]
```
Полученная глубина: 1

Исходный граф:
```
EDGES
0 -> [ 1 2 ]
1 -> [ 3 ]
2 -> [ 3 ]
3 -> [ 4 ]
4 -> [ 5 ]
5 -> [ 6 ]
6 -> [ 7 6 ]
7 -> [ 8 5 ]
8 -> [ 9 4 ]
9 -> [ 10 ]
10 -> [ 10 ]
```
Полученная глубина: 2

## Вывод
В результате работы был написан класс, который реализует вычисление глубины графа потока управления. Его работоспособность была успешно протестирована.
