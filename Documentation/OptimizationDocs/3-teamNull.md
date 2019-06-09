# Название задачи
Оптимизация по дереву №3: свёртка констант.

## Постановка задачи
Заменить алгебраические выражения вида "2 * 3" их рассчитанным ответом "6" с помощью визитора.
## Команда — исполнитель
"Null"

## Зависимости
Зависит от:
- Базовые визиторы
- PrettyPrinter

## Теория
Данная оптимизация заключается в свёртке констант в AST-дереве.

Пример:
```
x = 2 * 3; ==> x = 6;
```
## Реализация
Для решения поставленной задачи был реализован визитор "ConstFoldingVisitor", наследуемый от ChangeVisitor.

```csharp
class ConstFoldingVisitor : ChangeVisitor
    {
        public override void VisitBinOpNode(BinOpNode binop)
        {
            base.VisitBinOpNode(binop);
            if (binop.Left is IntNumNode left && binop.Right is IntNumNode right)
            {
                var num = 0;

                switch (binop.Op)
                {
                    case "+":
                        num = left.Num + right.Num;
                        break;
                    case "-":
                        num = left.Num - right.Num;
                        break;
                    case "*":
                        num = left.Num * right.Num;
                        break;
                    case "/":
                        num = left.Num / right.Num;
                        break;
                }

                var newInt = new IntNumNode(num);
                ReplaceExpr(binop, newInt);
            }
        }
	}
```

## Тесты
#### INPUT: 
```
x = 450 + 35 + 15;
```
#### OUTPUT:
```
x = 500;
```

## Вывод
Используя метод, описанный выше, мы получили визитор, производящий свертку констант.