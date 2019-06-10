# Замена выражения вида `x = x` на `null`.

## Постановка задачи
Заменить выражения вида `x = x` на `null` с помощью визитора.

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
class RemoveSelfAssignment : ChangeVisitor
    {
        public override void VisitAssignNode(AssignNode a)
        {
            IdNode id = a.Id;
            ExprNode expr = a.Expr; 

            if (expr is IdNode idExpr)
            {
  
                if (id.Name == idExpr.Name)
                {
                    ReplaceStatement(a, new EmptyNode());
                }
            }
        }
    }
```

## Тесты
Узнать как должны выглядить тесты в докуметации.

## Вывод
Используя метод, описанный выше, мы получили визитор, заменяющий выражения вида `x = x` на `null`.