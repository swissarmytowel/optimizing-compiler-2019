# Определение всех естественных циклов в CFG с информацией об их вложенности.

## Постановка задачи
Определить все естественные циклы в CFG и информацию об их вложенности.

## Команда — исполнитель
M&M

## Зависимости
Зависит от  
- Базовых блоков  
- Трехадресного кода  
- Графа потока управления 
- Классификация ребер в CFG
- Определение глубины CFG

## Теория

Циклы могут определяться как с помощью структурных инструкций
(while, for), так и с помощью инструкций goto.

С точки зрения анализа программ:
- не имеет значения внешний вид циклов
- имеет значение то, допускают ли они простую оптимизацию.

Важнейшей характеристикой циклов для анализа программ является
наличие у них **единственной точки входа.**

**Определение.** Пусть граф потока управления приводим. Для данного
обратного ребра n → d определим естественный цикл ребра как d плюс
множество узлов, которые могут достичь n не проходя через d.

Узел d называется заголовком цикла.
Он доминирует над всеми узлами цикла.

**Алгоритм** построения естественного цикла обратного ребра n → d.

*loopSet* ≔ {n, d}
Пометим d как посещённый
Рассмотрим обратный граф потока управления (все стрелочки – в
обратную сторону)
Выполним поиск в глубину на обратном
графе потока, начиная с n. Внесём все
посещённые узлы в *loopSet*.

## Реализация

Для решения поставленной задачи были реализованы следующие методы:

```csharp
...
// Естественные циклы
public CFGNaturalCycles (ControlFlowGraph cfg)
{
	var edgeClassifierService = new EdgeClassifierService(cfg);
	// Для данного обратного ребра n → d определим естественный цикл ребра как d плюс
	// множество узлов, которые могут достичь n не проходя через d.
	var retreatingEdges = edgeClassifierService.RetreatingEdges;
	Cycles = new HashSet<CFGNaturalCycle>();
	foreach (var edge in retreatingEdges)
	{
		// Запуск алгоритма построения естественного цикла
		var cycle = new CFGNaturalCycle(cfg, edge);
		Cycles.Add(cycle);
	}
	// Объединение циклов с общим заголовком в один
	MergeLoopsWithCommonEntryBlock();
	// Заполнение информации о вложенности циклов
	DefinitionNestingForCycles();
}
```
```csharp
...
//Алгоритм построения естественного цикла обратного ребра n → d.
public CFGNaturalCycle(ControlFlowGraph cfg, Edge<ThreeAddressCode> loopEdge)
{
	//loopSet ≔ {n, d}
	var visitedVertex = new HashSet<ThreeAddressCode>();
	// Пометим d как посещённый
	visitedVertex.Add(loopEdge.Target);
	EntryBlock = loopEdge.Target;
	Graph.AddVertex(EntryBlock);
	var targetVertex = new Queue<ThreeAddressCode>();
	targetVertex.Enqueue(loopEdge.Source);
	ExitBlocks.Add(loopEdge.Source);
	Graph.AddVertex(loopEdge.Source);
	Graph.AddEdge(new Edge<ThreeAddressCode>(loopEdge.Source, EntryBlock));
	//Выполним поиск в глубину на обратном графе потока, начиная с n. Внесём все
	//посещённые узлы в loopSet.
	while (targetVertex.Count() > 0)
	{
		var curVertex = targetVertex.Dequeue();
		var containVertex = cfg.ContainsVertex(curVertex);
		// Рассмотрим обратный граф потока управления (все стрелочки – в обратную сторону)
		var incomingEdges = containVertex ? cfg.InEdges(curVertex) : new List<Edge<ThreeAddressCode>>();
		foreach (var edge in incomingEdges)
		{
			if (!visitedVertex.Contains(edge.Source))
			targetVertex.Enqueue(edge.Source);
			if (!Graph.Vertices.Contains(edge.Source))
			Graph.AddVertex(edge.Source);
			Graph.AddEdge(new Edge<ThreeAddressCode>(edge.Source, edge.Target));
		}
		visitedVertex.Add(curVertex);
	}
}
...
```
## Тесты
```csharp
l 7:
a1 = 4 * i; 
if (b) { 
	a3 = 4 * i;
	goto l 7:
}
while (cond) {
	sum = sum + 1;
}
a2 = 4 * i;
goto l 7:
```
Для данной программы сначала образуется 3 цикла, потом из-за объединения с общим заголовком остаётся 2. 
```csharp
NaturalCycles:
Loops Count = 2
VERTICES
#0:
L3: t4 = cond
if t4 goto L5

#1:
L5: t5 = sum + 1
sum = t5
goto L3

EDGES
0 -> [ 1 ]
1 -> [ 0 ]
VERTICES
#0:
l7: t1 = 4 * i
a1 = t1
t2 = b
if t2 goto L1

#1:
L1: t3 = 4 * i
a3 = t3
goto l7

#2:
L4: t6 = 4 * i
a2 = t6
goto l7

#3:
goto L4

#4:
L3: t4 = cond
if t4 goto L5

#5:
L2:

#6:
L5: t5 = sum + 1
sum = t5
goto L3

#7:
goto L2
EDGES
0 -> [ 7 1 ]
1 -> [ 0 ]
2 -> [ 0 ]
3 -> [ 2 ]
4 -> [ 6 3 ]
5 -> [ 4 ]
6 -> [ 4 ]
7 -> [ 5 ]
```

## Вывод
Используя методы, описанные выше, мы определили, все естественные циклы в CFG и информацию об их вложенности.

