# Замена выражения вида 1 * expr, expr * 1, expr / 1 на expr.

## Постановка задачи
Заменить выражения вида `1 * expr`, `expr * 1`, `expr / 1` на `expr` с помощью визитора.

## Команда — исполнитель
TZ

## Зависимости
Зависит от:
- Базовые визиторы
- PrettyPrinter

## Теория
&mdash;

## Реализация
Для решения поставленной задачи был реализован визитор, наследуемый от ChangeVisitor.

```csharp
class SimplifyMultDivisionByOneVisitor : ChangeVisitor
    {
        public override void VisitBinOpNode(BinOpNode binop)
        {
            base.VisitBinOpNode(binop);
            if (binop.Op == "*")
            {
                ExprNode expr1 = binop.Left;
                ExprNode expr2 = binop.Right;
                if (expr1 is IntNumNode expr && expr.Num == 1)
                {
                    ReplaceExpr(expr1.Parent as ExprNode, expr2);
                }
                else if (expr2 is IntNumNode exp && exp.Num == 1)
                {
                    ReplaceExpr(expr2.Parent as ExprNode, expr1);
                }
            }
            if (binop.Op == "/")
            {
                ExprNode expr1 = binop.Left;
                ExprNode expr2 = binop.Right;
                if (expr2 is IntNumNode exp && exp.Num == 1)
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
Используя метод, описанный выше, мы получили визитор, заменяющий выражения вида `1 * expr`, `expr * 1`, `expr / 1` на `expr`.
