# Замена выражения вида `0 * expr`, `expr * 0` на `0`.

## Постановка задачи
Заменить выражения вида `0 * expr`, `expr * 0` на `0` с помощью визитора.

## Команда — исполнитель
Kt

## Зависимости
Зависит от:
- Базовые визиторы
- PrettyPrinter

## Теория
&mdash;

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
Узнать как должны выглядить тесты в докуметации.

## Вывод
Используя метод, описанный выше, мы получили визитор, заменяющий выражения вида `0 * expr`, `expr * 0` на `0`.
