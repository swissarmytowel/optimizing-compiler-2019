# Устранение переходов к переходам.

## Постановка задачи
Задача состояла в реализации локальной оптимизации трехадресного кода "Устранение переходов к переходам"

## Команда — исполнитель
Enterprise

## Зависимости
&mdash;

## Теория
Данная задача состояла в нахожениеи узлов `goto`, у которых метка ведет на другой узел `goto`. Таким образом получается, вместо двух переходов метке мы получим один. И далее код озавшийся внутри, будет потенциально удален оптимизацей по устранению недостижимого кода.

## Реализация

Для того чтобы реализовать этот алгоритм, нужно пройтись по всем линиям трехадресного кода и проверить, является ли текующая линия - узлом `goto`и ведет ли она на другой узел.

```csharp
private HashSet<string> FindAllLabels(ThreeAddressCode tac)
        {
            var setOfLables = new HashSet<string>();

            foreach (var line in tac)
                if (line.Label != null) setOfLables.Add(line.Label);

            return setOfLables;
        }
```
Также была реализована функция для проверки меток. Она проверяет, не содержится ли метка между goto и целевым узлом трехадресного кода:
```csharp
var previousNodes = new HashSet<TacNode>();

foreach (var line in tac.TACodeLines)
{
    if (line is TacGotoNode gotoNode 
        && tac[gotoNode.TargetLabel].GetType() == typeof(TacGotoNode) 
        && !previousNodes.Contains(tac[gotoNode.TargetLabel]))
    {
        var tacGoto = tac[gotoNode.TargetLabel] as TacGotoNode;
        gotoNode.TargetLabel = tacGoto.TargetLabel;
        isApplied = true;
    }
    previousNodes.Add(line);
}
```
Стоит заметить, также пришлось хранить предыдущие узлы чтобы избежать переходов вверх по коду.

## Тесты

Трехадресный код до применения оптимизации:
```
a = 123  
goto l7
v = 345  
l7: goto l8
g = 55  
l8: 
```
Трехадресный код после применения оптимизации:
```
a = 123  
goto l8
v = 345  
l7: goto l8
g = 55  
l8: 

```

Трехадресный код до применения оптимизации:
```
a = 123  
v = 345  
t7 = a > 233
if t7 goto L1
goto L2
L1: goto l7
L2: c = 425  
l7: g = 44
```
Трехадресный код после применения оптимизации:
```
a = 123  
v = 345  
t7 = a > 233
if t7 goto l7
goto L2
L1: goto l7
L2: c = 425  
l7: g = 44    

```

## Вывод
В результате работы была написана функция, которая устраняет переходы к переходам. Его работоспособность была успешно протестирована.
