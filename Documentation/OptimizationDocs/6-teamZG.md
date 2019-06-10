# Замена выражений вида `2 < 3` на `true (false)`

## Постановка задачи
Заменить выражения вида `2 < 3` на `true (false)` с помощью визитора.

## Команда — исполнитель
ZG

## Зависимости
Зависит от:

-   Базовые визиторы
-   PrettyPrinter

## Теория
&mdash;

## Реализация
Для решения поставленной задачи был реализован визитор, наследуемый от ChangeVisitor.

```csharp
class CheckTruthVisitor: ChangeVisitor
{
    public override void VisitBinOpNode(BinOpNode binop)
    {
        base.VisitBinOpNode(binop);
        if (binop.Op == "<")
        {
            ExprNode expr1 = binop.Left;
            ExprNode expr2 = binop.Right;

            if (expr1 is IntNumNode ex && expr2 is IntNumNode ex2)
            {
                if (ex.Num < ex2.Num)
                {
                    ReplaceExpr(expr1.Parent as ExprNode, new BoolNode(true));
                }
            }
        }
    }
}
```

## Тесты
&mdash;

## Вывод
Используя метод, описанный выше, реализован визитор, заменяющий выражения вида `2 < 3` на `true (false)`.
