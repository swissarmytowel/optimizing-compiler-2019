# Название задачи
Оптимизация по дереву №4: замена выражения вида `0 + expr` (`expr + 0`) на `expr`.

## Постановка задачи
Заменить выражения вида `0 + expr` (`expr + 0`) на `expr` с помощью визитора.

## Команда — исполнитель
AW

## Зависимости
Зависит от:
- Базовые визиторы
- PrettyPrinter

## Теория
&mdash;

## Реализация
Для решения поставленной задачи был реализован визитор, наследуемый от ChangeVisitor.

```csharp
class PlusZeroExprVisitor : ChangeVisitor
{
    public override void VisitBinOpNode(BinOpNode binop)
    {
        base.VisitBinOpNode(binop);
        if (binop.Op == "+")
        {
            ExprNode expr1 = binop.Left;
            ExprNode expr2 = binop.Right;
            if (expr1 is IntNumNode expr && expr.Num == 0)
            {
                ReplaceExpr(expr1.Parent as ExprNode, expr2);
            }
            else if (expr2 is IntNumNode exp && exp.Num == 0)
            {
                ReplaceExpr(expr2.Parent as ExprNode, expr1);
            }
        }
    }
}
```

## Тесты
Узнать как должны выглядить тесты в докуметации.

## Вывод
Используя метод, описанный выше, мы получили визитор, заменяющий выражения вида `0 + expr` (`expr + 0`) на `expr`.