# Свёртка констант. Алгебраические тождества.

## Постановка задачи
Заменить выражения вида `x := 2 * 3.14` на `x := 6.28`. Применить следующие алгебраические тождества:
```
x + 0 = x        1 * x = x
0 + x = x        x * 0 = 0
x - 0 = x        x / 1 = x
x - x = 0        x / x = 1
x * 1 = x
```

## Команда — исполнитель
ZG

## Зависимости
Зависит от:

-   Трёхадресный код (AW)
-   IOptimizer

## Теория
&mdash;

## Реализация
Для решения задачи свёртки констант был реализован класс `ConvConstOptimization`, наследуемый от `IOptimizer`.
Основная идея заключается в том, что оптимизатор проходит по строкам трёхадресного кода, находит строки в которых второй операнд не `null`,
пытается преобразовать оба операнда к типу `int (double)` и применяет следующие оптимизации:
```csharp
switch (assigned.Operation)
{
    case "+":
        assigned.FirstOperand = (int_1 + int_2).ToString();
        break;
    case "-":
        assigned.FirstOperand = (int_1 - int_2).ToString();
        break;
    case "*":
        assigned.FirstOperand = (int_1 * int_2).ToString();
        break;
    case "/":
        assigned.FirstOperand = (int_1 / int_2).ToString();
        break;
    case ">":
        assigned.FirstOperand = (int_1 > int_2).ToString();
        break;
    case "<":
        assigned.FirstOperand = (int_1 < int_2).ToString();
        break;
    case ">=":
        assigned.FirstOperand = (int_1 >= int_2).ToString();
        break;
    case "<=":
        assigned.FirstOperand = (int_1 <= int_2).ToString();
        break;
    case "==":
        assigned.FirstOperand = (int_1 == int_2).ToString();
        break;
    case "!=":
        assigned.FirstOperand = (int_1 != int_2).ToString();
        break;
}
```
Для применения алгебраических тождеств был реализован класс `AlgebraicIdentityOptimization`, наследуемый от `IOptimizer`.
Принцип работы похож на описанный выше. Применение алгебраических тождеств реализовано следующим образом:
```csharp
switch (assign.Operation)
{
    case "+": // x + 0 = x, 0 + x = x  
        if (assign.SecondOperand == "0")
        {
            assign.SecondOperand = null;
            assign.Operation = null;
        }
        else if (assign.FirstOperand == "0")
        {
            assign.FirstOperand = assign.SecondOperand;
            assign.SecondOperand = null;
            assign.Operation = null;
        }
        break;
    case "-": // x - 0 = x, x - x = 0
        if (assign.SecondOperand == "0")
        {
            assign.SecondOperand = null;
            assign.Operation = null;
        }
        else if (assign.FirstOperand == assign.SecondOperand)
        {
            assign.FirstOperand = "0";
            assign.SecondOperand = null;
            assign.Operation = null;
        }
        break;
    case "*": // x * 1 = x, 1 * x = x, x * 0 = 0
        if (assign.SecondOperand == "1")
        {
            assign.SecondOperand = null;
            assign.Operation = null;
        }
        else if (assign.FirstOperand == "1")
        {
            assign.FirstOperand = assign.SecondOperand;
            assign.SecondOperand = null;
            assign.Operation = null;
        }
        else if (assign.FirstOperand == "0" || assign.SecondOperand == "0")
        {
            assign.FirstOperand = "0";
            assign.SecondOperand = null;
            assign.Operation = null;
        }
        break;
    case "/": // x / 1 = x, x / x = 1
        if (assign.SecondOperand == "1")
        {
            assign.SecondOperand = null;
            assign.Operation = null;
        }
        else if (assign.FirstOperand == assign.SecondOperand)
        {
            assign.FirstOperand = "1";
            assign.SecondOperand = null;
            assign.Operation = null;
        }
        break;
}
```

## Тесты
INPUT:
```
a = 2 * 3;
b = a / a;
b = a / 1;
b = a * 0;
b = a * 1;
b = a - a;
b = a - 0;
b = 0 + a;
```
TAC:
```
t1 = 2 * 3
a = t1
t2 = a / a
b = t2
t3 = a / 1
b = t3
b = 0
t4 = a * 1
b = t4
b = 0
t5 = a - 0
b = t5
t6 = 0 + a
b = t6
```
OUTPUT:
```
t1 = 2 * 3
a = t1
t2 = 1
b = t2
t3 = a
b = t3
b = 0
t4 = a
b = t4
b = 0
t5 = a
b = t5
t6 = a
b = t6
```

## Вывод
Используя методы, описанные выше, удалось применить свёртку констант и алгебраические тождества.
