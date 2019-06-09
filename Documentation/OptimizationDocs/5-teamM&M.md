# Оптимизация по дереву №5: замена `a - a` на `0`.

## Постановка задачи
Заменить выражения вида `a - a` на `0` с помощью визитора.

## Команда — исполнитель
M&M

## Зависимости
Зависит от:

-   Базовые визиторы
-   PrettyPrinter

## Теория
--
## Реализация

Для решения поставленной задачи был реализован визитор, наследуемый от ChangeVisitor.

```csharp
class SameMinusOptVisitor : ChangeVisitor
{
	public override void VisitBinOpNode(BinOpNode binop)
	{
		base.VisitBinOpNode(binop);
		if (binop.Op == "-")
		{
			ExprNode expr1 = binop.Left;
			ExprNode expr2 = binop.Right;
			if (expr1 is IdNode && expr2 is IdNode)
			{
				if ((expr1 as IdNode).Name == (expr2 as IdNode).Name)
				{
					ReplaceExpr(expr1.Parent as ExprNode, new IntNumNode(0));
				}
			}
		}
	}
}
```

## Тесты
--

## Вывод
Используя метод, описанный выше, мы получили визитор, заменяющий выражения вида `a - a` на `0`.
