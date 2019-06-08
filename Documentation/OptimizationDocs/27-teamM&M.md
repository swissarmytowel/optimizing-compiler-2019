
# Очистка от пустых операторов, устранение переходов через переходы.

## Постановка задачи
Очистка от пустых операторов и устранение переходов через переходы в трехадресном коде.

## Команда — исполнитель
M&M

## Зависимости
Зависит от  Трехадресного кода (AW)

## Теория
#### Очистка от пустых операторов
До:
```csharp
L1: <пусто>
	x = a + b
```
После:
```csharp
L1: x = a + b
```
####  Устранение переходов через переходы
До:
```csharp
	if (усл) goto L1
		goto L2
L1: оператор
L2:
```
После:
```csharp
	if (!усл) goto L2
	оператор
L2:
```
## Реализация

Для решения поставленной задачи был реализованы EmptyNodeOptimization и GotoOptimization.

```csharp
class  EmptyNodeOptimization : IOptimizer
{
	public bool Optimize(ThreeAddressCode tac)
	{
		bool isUsed = false;
		var node = tac.TACodeLines.First;
		while (true)
		{
			var next = node.Next;
			if (next == null)
			break;
			var val = node.Value;
			var label = val.Label;
			if (val is TacEmptyNode && val.Label == null)
			{
				isUsed = true;
				if (next != null)
				{
				next.Value.Label = label;
				}
				tac.TACodeLines.Remove(node);
			}
			node = next;
		}
		return isUsed;
	}
}
```

```csharp
class  GotoOptimization : IOptimizer
{
	public bool Optimize(ThreeAddressCode tac)
	{
		bool isUsed = false;
		var node = tac.TACodeLines.First;
		while (node != null)
		{
			var next = node.Next;
			var val = node.Value;
			var label = val.Label;
			if (next != null)
			{
				var nextVal = next.Value;
				if (val is TacIfGotoNode && nextVal is TacGotoNode)
				{
					isUsed = true;
					var ifVal = val as TacIfGotoNode;
					string tempVar = TmpNameManager.Instance.GenerateTmpVariableName();
					TacNode tempAssign = new TacAssignmentNode()
					{
						LeftPartIdentifier = tempVar,
						Operation = "!",
						SecondOperand = ifVal.Condition
					};
					tac.TACodeLines.AddBefore(tac.TACodeLines.Find(val), tempAssign);
					ifVal.Condition = tempVar;
					ifVal.TargetLabel = (nextVal as TacGotoNode).TargetLabel;
					var remove = next;
					next = node;
					tac.TACodeLines.Remove(remove);
				}
			}
			node = next;
		}
		return isUsed;
	}
}
```
## Тесты
Узнать как должны выглядить тесты в докуметации.

## Вывод
Используя методы, описанные выше, мы получили оптимизации: очистка от пустых операторов и устранение переходов через переходы.
