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
Для решения поставленной задачи был реализован визитор "DelOfDeadConditionsVisitor", наследуемый от ChangeVisitor.

```csharp
  public class DelOfDeadConditionsVisitor : ChangeVisitor
    {
        public override void VisitBlockNode(BlockNode bl)
        {
            for (int i = 0; i < bl.StList.Count; i++)
                if (bl.StList[i] is IfNode ifn)
                {
                    var stlist1 = ifn.Stat1 as BlockNode;
                    var stlist2 = ifn.Stat2 as BlockNode;
                    bool null1, null2;
                    null1 = null2 = false;
                    if (stlist1.StList.Count == 1 & stlist1.StList[0] is EmptyNode)
                        null1 = true;
                    if (stlist2.StList.Count == 1 & stlist2.StList[0] is EmptyNode)
                        null2 = true;

                    if (null1 && null2)
                        bl.StList[i] = new EmptyNode();
                    else
                        base.VisitIfNode(ifn);
                }
        }
    }
```

## Тесты

#### INPUT:
```
if(a == b)
	{;}
else
	{;}
```

#### OUTPUT:
```
null
```

## Вывод
Используя метод, описанный выше, мы получили визитор, производящий удаление мертвых ветвлений.
