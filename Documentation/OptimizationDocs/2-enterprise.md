# Оптимизация по дереву вида 0*expr

## Постановка задачи
Задача состояла в реализации оптимизации AST-дерева вида `0 * expr`

## Команда — исполнитель
Enterprise

## Зависимости
Зависит от:
- Базовые визиторы
## Теория
При возникновении присвоения, в процессе которого переменной присваивается выражение вида `0 * expr` или `0 * expr` необходимо заменить выражение на ноль. 

## Реализация
Для решения поставленной задачи был реализован визитор, наследуемый от ChangeVisitor.

```csharp
class ZeroMulOptVisitor : ChangeVisitor
    {

        public override void VisitBinOpNode(BinOpNode binop)
        {
            var isZeroLeft = binop.Left is IntNumNode && (binop.Left as IntNumNode).Num == 0;
            var isZeroRight = binop.Right is IntNumNode && (binop.Right as IntNumNode).Num == 0;

            if (string.Equals(binop.Op, "*") && (isZeroRight || isZeroLeft))
                ReplaceExpr(binop, new IntNumNode(0));
            else base.VisitBinOpNode(binop);
        }
    }
```

## Тесты
#### INPUT: 
```
x = 50 * 10;
```
#### OUTPUT:
```
x = 500;
```

## Вывод
В результате работы был написан визитор, который посещает все узлы AST-дерева и применяет оптимизацию вида `0 * expr`.
