# Замена while (false) st; на null.

## Постановка задачи
Заменить выражения вида `while (false) st;` на `null` с помощью визитора.

## Команда — исполнитель
M&M

## Зависимости
Зависит от:

-   Базовые визиторы
-   PrettyPrinter

## Теория

Данная оптимизация заключается в том, чтобы заменять любой оператор *while* вида
``` 
while (false)
	st;
```
на выражение *null*, так как при любом *expression* в условном операторе будет получен *null*.

## Реализация

Для решения поставленной задачи был реализован визитор, наследуемый от ChangeVisitor.

```csharp
class WhileFalseOptVisitor: ChangeVisitor
{
	public override void VisitWhileNode(WhileNode c)
	{
		if (c.Expr is BoolNode bn && bn.Value == false)
		{
			ReplaceStatement(c, new EmptyNode());
		}
	}
}
```
Также был реализован метод ReplaceStatement для визитора ChangeVisitor, который позволяет заменять StatementNode:

```csharp
public void ReplaceStatement(StatementNode from, StatementNode to)
{
	var p = from.Parent;
	if (p is AssignNode || p is ExprNode)
	{
		throw new Exception("Родительский узел не содержит операторов");
	}
	to.Parent = p;
	if (p is BlockNode bln)
	{
		for (var i = 0; i < bln.StList.Count; ++i)
		{
			if (bln.StList[i] == from)
			{
				bln.StList[i] = to;
				break;
			}
		}
	}
	else if (p is IfNode ifn)
	{
		if (ifn.Stat1 == from)
		{
			ifn.Stat1 = to;
		}
		else if (ifn.Stat2 == from)
		{
			ifn.Stat2 = to;
		}
	}
	else
	{
		throw new Exception("ReplaceStatement не определен для данного типа узла");
	}
}
```
## Тесты
#### INPUT: 
```csharp
while(false)
	x = a;
x = b;
```
#### OUTPUT:
```csharp
x = b;
```

## Вывод
Используя метод, описанный выше, мы получили визитор, заменяющий выражения вида `while (false) st;` на `null`.
