# Итерационный алгоритм для доступных выражений.

## Постановка задачи
Реализовать итерационный алгоритм для доступных выражений

## Команда — исполнитель
ZG

## Зависимости
Зависит от:
-   Трёхадресный код
-   Граф потоков управления
-   Def-Use
-   Передаточная функция

## Теория
**Определение.** x+y доступно в точке p, если любой путь от входа к p
вычисляет x+y и после последнего вычисления до достижения p нет
присваиваний x и y.<br />
**Определение**<br />
Блок уничтожает выражение x+y,
если он присваивает x или y и потом
не перевычисляет x+y.<br />
*e_killB* − множество всех выражений,
уничтожаемых блоком B.<br />
**Определение**<br />
Блок генерирует выражение x+y,
если он вычисляет x+y и потом не
переопределяет x и y.<br />
*e_genB* − множество всех выражений,
генерируемых блоком B.<br />
![](../images/40-teamZG.png)

**Алгоритм**<br />
Вход: граф потока управления, в котором для каждого ББл вычислены
e_genB и e_killB<br />
Выход: Множества выражений, доступных на входе IN[B] и на выходе OUT[B]
для всех ББл B
![](../images/40-teamZG-1.png)<br />
*U* – универсальное множество всех выражений, появляющихся в правых
частях

## Реализация
Для решения поставленной задачи был реализован класс AvailableExpressionsITA, наследуемый от IterationAlgorithm.
В классе IterationAlgorithm реализован метод `Execute` (более подробно описан [здесь](44-teamZG.md)), который является общим
для всех итерационных алгоритмов. Его результат зависит от способа инициализации множеств `IN[B]` и `OUT[B]`, направления обхода
графа потоков управления, передаточной функции и оператора сбора.<br />
**Параметры итерационного алгоритма для доступных выражений**
- Направление: прямое
- Передаточная функция: *e_genB ∪ (x - e_killB)*
- Оператор сбора: ∩
- Инициализация: IN[B] = *U*

## Тесты
INPUT:
```
BLOCK0:
a = 9
t1 = 9 > 5
if t1 goto L1
BLOCK1:
goto L2
BLOCK2:
L1: goto l1
BLOCK3:
L2: b = 8
t2 = 9 + b
c = t2
BLOCK4:
l1: a = 15
```

OUTPUT:
```
--- IN 0 :
null
--- OUT 0:
0)t1 = 9 > 5

--- IN 1 :
0)t1 = 9 > 5

--- OUT 1:
0)t1 = 9 > 5

--- IN 2 :
0)t1 = 9 > 5

--- OUT 2:
0)t1 = 9 > 5

--- IN 3 :
0)t1 = 9 > 5

--- OUT 3:
0)t2 = 9 + b
1)t1 = 9 > 5

--- IN 4 :
0)t1 = 9 > 5

--- OUT 4:
0)t1 = 9 > 5
```

## Вывод
Реализован итерационный алгоритм для доступных выражений.
