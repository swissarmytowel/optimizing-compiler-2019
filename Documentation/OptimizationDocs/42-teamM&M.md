# Провести оптимизации на основе анализа доступных выражений.

## Постановка задачи
Реализовать оптимизации на основе анализа доступных выражений.
## Команда — исполнитель
M&M

## Зависимости
Зависит от  
- Базовых блоков  
- Трехадресного кода  
- Графа потока управления


## Теория
&mdash;

**Определение.** x+y доступно в точке p если любой путь от входа к p
вычисляет x+y и после последнего вычисления до достижения p нет
присваиваний x и y.

Оптимизации на основе доступных выражений:
![](../images/41-teamM&M.png)

## Реализация

Для решения поставленной задачи был реализован  AvailableExprOptimization:

```csharp
...
// проводим оптимизацию
for (int blockInd = 0; blockInd < bb.BasicBlockItems.Count(); blockInd++)
	{
	var block = bb.BasicBlockItems[blockInd];
	var codeLine = block.TACodeLines.First;
	foreach (var _expr in varsExprChange.Keys.ToArray())
		varsExprChange[_expr] = !IN_EXPR[block].Contains(_expr);
	while (codeLine != null)
	{
		var node = codeLine.Value;
		if (node is TacAssignmentNode assign)
		{
			string assignId = assign.LeftPartIdentifier;
			TacExpr expr = new TacExpr(assign);
			// если выражений больше 1 делаем оптимизацию
			if (tacExprCount.Keys.Contains(expr) && tacExprCount[expr] > 1)
			{
				isUsed = true;
				if (!varsExprChange.Keys.Contains(expr))
					varsExprChange.Add(expr, !IN_EXPR[block].Contains(expr));
			// если это первая замена общего выражения
			if (!idsForExprDic.Keys.Contains(expr))
			{
			// создаём переменную для общего выражения
				string idName = TmpNameManager.Instance.GenerateTmpVariableName();
				idsForExprDic.Add(expr, idName);
				block.TACodeLines.AddBefore(block.TACodeLines.Find(node), expr.CreateAssignNode(idName));
				AssignRightPartVarReplace(assign, idName);
				varsExprChange[expr] = false;
			} else
			{
				string idName = idsForExprDic[expr];
				// если это не замена общего выражения
				if (assignId != idName)
					AssignRightPartVarReplace(assign, idName);
				// если выражение недоступно на входе
				if (varsExprChange[expr])
					{
						block.TACodeLines.AddBefore(block.TACodeLines.Find(node), expr.CreateAssignNode(idName));
						varsExprChange[expr] = false;
					}
				}
			}
			// для всех оптимизируемых выражений
			foreach (var _expr in varsExprChange.Keys.ToArray())
			{
			// если выражение недоступно на выходе и присваивание его изменяет
				if (_expr.FirstOperand == assignId || _expr.SecondOperand == assignId)
				{
					varsExprChange[_expr] = true;
				}
			}
		}
		codeLine = codeLine.Next;
	}
}
...
```

## Тесты

Source Code:
```
a1 = 4 * i; 
if (b) { 
  a3 = 4 * i;
} 
a2 = 4 * i;         
```
Before AvailableExprOptimization

```
BLOCK0:
t1 = 4 * i
a1 = t1
if t4 goto L2
BLOCK1:
goto L2
BLOCK2:
L1: t2 = 4 * i
a3 = t2
BLOCK3:
L2: t3 = 4 * i
a2 = t3
```
After AvailableExprOptimization

```
BLOCK0:
t5 = 4 * i
t1 = t5
a1 = t1
if t4 goto L2
BLOCK1:
goto L2
BLOCK2:
L1: t2 = t5
a3 = t2
BLOCK3:
L2: t3 = t5
a2 = t3
```
## Вывод
Используя методы, описанные выше, мы получили оптимизации на основе анализа доступных выражений
