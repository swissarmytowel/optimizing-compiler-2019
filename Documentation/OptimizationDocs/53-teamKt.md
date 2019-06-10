# Определение того, является ли ребро обратимым и является ли CFG приводимым.

## Постановка задачи
Определить, является ли ребро обратимым и является ли CFG приводимым.
## Команда — исполнитель
Kt

## Зависимости
Зависит от  
- Базовых блоков  
- Трехадресного кода  
- Графа потока управления 

## Теория

**Определение.** Обратным в графе CFG называется ребро a → b, у
которого b доминирует над a.
**Утверждение.** Не любое отступающее ребро является обратным.
**Определение.** Граф потока управления называется приводимым если
все его отступающие рёбра являются обратными.
## Реализация

Для решения поставленной задачи был реализован следующий метод :

```csharp
...
public static bool IsReducibility(ControlFlowGraph cfg)
{
	var edgeClassifierService = new EdgeClassifierService(cfg);
	var retreatingEdges = edgeClassifierService.RetreatingEdges;
	var backEdges = edgeClassifierService.BackEdges;
	return retreatingEdges.SetEquals(backEdges);
}
...
```

## Тесты

#### Input

```
BasicBlocks:

BLOCK0:
a = 9
t1 = a > 5
if t1 goto L1

BLOCK1:
goto L2

BLOCK2:
L1: goto l1

BLOCK3:
L2: b = 8
t2 = 9 + b
c = t2

BLOCK4:
l1: a = 15
```
#### Output

```
DominatorService:
blockInd0:
Dominator: 0  ImmediateDominator: -1
blockInd1:
Dominator: 0 1  ImmediateDominator: 0
blockInd2:
Dominator: 0 2  ImmediateDominator: 0
blockInd3:
Dominator: 0 1 3  ImmediateDominator: 1
blockInd4:
Dominator: 0 4  ImmediateDominator: 0

BackEdges:

ComingEdges:
EDGE: block0 -> block2
EDGE: block0 -> block1
EDGE: block1 -> block3
EDGE: block2 -> block4

RetreatingEdges:

CrossEdges:
EDGE: block3 -> block4

IsReducibility: True
```
## Вывод
Используя методы, описанные выше, мы определили, является ли ребро обратимым и является ли CFG приводимым.
