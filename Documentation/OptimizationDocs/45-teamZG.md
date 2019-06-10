# Обобщённый итерационный алгоритм. Распространение констант.

## Постановка задачи
Реализовать обобщенный итерационный алгоритм и заменить в программе переменные,
имеющие константное значение, на константу.

## Команда — исполнитель
ZG

## Зависимости
Зависит от:
-   Трёхадресный код
-   Граф потоков управления
-   Def-Use
-   Передаточная функция

## Теория
**Обобщенный итерационный алгоритм**<br />
![](../images/44-teamZG.png)<br />
**Распространение констант**<br />
**Определение.** Замена переменных, имеющих константное значение, на константу.<br />
**Передаточная функция**<br />
![](../images/44-teamZG-1.png)<br />

**Итерационный алгоритм для задачи распространения констант**<br />
![](../images/44-teamZG-2.png)<br />


## Реализация
Для решения поставленной задачи был реализован класс IterationAlgorithm.
В классе IterationAlgorithm реализован метод `Execute`, который является общим
для всех итерационных алгоритмов. Его результат зависит от способа инициализации множеств `IN[B]` и `OUT[B]`, направления обхода
графа потоков управления, передаточной функции и оператора сбора.<br />
Для начала инициализируем множество OUT[B]:
```csharp
foreach(var vertex in vertices)
{
    InOut.Out[vertex] = InitilizationSet;
}
```
Пока внесены изменения в OUT выполняем:
```csharp
while (isChanged)
{
   isChanged = false;
   foreach(var vertex in vertices)
   {
       var pred = (entryPoints.Contains(vertex))?
           new HashSet<T>() :
           GetPredVertices(vertex)
           .Select(e => InOut.Out[e])
           .Aggregate((a,b) => CollectionOperator(a,b));

       InOut.In[vertex] = pred;
       var tmp = InOut.Out[vertex];
       InOut.Out[vertex] = TransmissionFunc(InOut.In[vertex], vertex);
       if (!tmp.SequenceEqual(InOut.Out[vertex]))
       {
           isChanged = true;
       }
   }
}
```
Для решения задачи распространения констант был реализован класс ConstDistributionITA, наследуемый от IterationAlgorithm.
Алгоритм аналогичен вышеописанному. В качестве передаточной функции использует класс ConstDistribFunction, в качестве оператора сбора -
ConstDistribOperator.
## Тесты
&mdash;

## Вывод
Реализован обобщенный итерационный алгоритм и итерационный алгоритм для задачи распространения констант.
