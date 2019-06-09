# Оптимизация по дереву №12: замена выражений вида `if (false) st1; else st2;` на `st2`

## Постановка задачи
Заменить выражения вида `if (false) st1; else st2;` на `st2` с помощью визитора.

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
class AlwaysElseVisitor: ChangeVisitor
{
    public override void VisitIfNode(IfNode inode)
    {
        base.VisitIfNode(inode);
        if (inode.Expr is BoolNode expr && expr.Value == false)
        {
             ReplaceStatement(inode, inode.Stat2 ?? new EmptyNode());
        }
        else
        {
            base.VisitIfNode(inode);
        }
    }
}
```

## Тесты
&mdash;

## Вывод
Используя метод, описанный выше, реализован визитор, заменяющий выражения вида `if (false) st1; else st2;` на `st2`.
