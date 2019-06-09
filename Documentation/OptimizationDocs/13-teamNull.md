# Название задачи
Оптимизация по дереву №13: удаление мертвых ветвлений.

## Постановка задачи
Заменить выражения вида "if (ex) null; else null; " на "null" с помощью визитора.

## Команда — исполнитель
"Null"

## Зависимости
Зависит от:
- Базовые визиторы
- PrettyPrinter

## Теория
Данная оптимизация заключается в том, чтобы заменять любой условный оператор вида
``` 
if(expression)
	null;
else
	null;
```
на выражение *null* так как при любом *expression* в условном операторе будет получен *null*.

## Реализация
Для решения поставленной задачи был реализован визитор "ThirteenthOptimizationVisitor", наследуемый от ChangeVisitor.

```csharp
 class ThirteenthOptimizationVisitor : ChangeVisitor
    {
        public override void VisitIfNode(IfNode c)
        {
            base.VisitIfNode(c);

            if (c.Stat1 == null && c.Stat2 == null)
                ReplaceStatement(c, null);
        }
	}
```

## Тесты

#### INPUT:
```
if(a!=b)
	null;
else
	null;
```

#### OUTPUT:
```
null;
```

## Вывод
Используя метод, описанный выше, мы получили визитор, производящий удаление мертвых ветвлений.