# Оптимизация по дереву вида a > a, a != a

## Постановка задачи
Задача состояла в реализации оптимизаций AST-дерева вида `a > a` и `a != a`

## Команда — исполнитель
Enterprise

## Зависимости
Данная задача не зависит от других задач и не порождает новых зависимостей.

## Теория
При возникновении логического выражения вида `a > a` или `a != a` его необходимо заменить на `a`.

## Реализация
Для решения поставленной задачи был реализован визитор, наследуемый от ChangeVisitor.

```csharp
class CompareToItselfFalseOptVisitor : ChangeVisitor
    {
        public override void VisitBinOpNode(BinOpNode binop)
        {
            if (binop.Left is IdNode left && binop.Right is IdNode right)
            {
                var isNamesEqual = string.Equals(left.Name, right.Name);
                if (string.Equals(binop.Op, ">") || string.Equals(binop.Op, "!=") && isNamesEqual)
                    ReplaceExpr(binop, new BoolNode(false));
                else base.VisitBinOpNode(binop);
            }
        }
    }
```

## Тесты
Узнать как должны выглядить тесты в докуметации.

## Вывод
В результате работы был написан визитор, который посещает все узлы AST-дерева и применяет оптимизации вида `a > a` и `a != a`.