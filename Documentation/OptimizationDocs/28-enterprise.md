# Устранение недостижимого кода

## Постановка задачи
Задача состояла в реализации локальной оптимизации трехадресного кода "устранение недостижимого кода"

## Команда — исполнитель
Enterprise

## Зависимости
&mdash;

## Теория
Данная задача состояла в поиске и удалении недостижимого кода. Код считается недостижимым если он следует за оператором `if`, значение которого является `true`, или же он следует за меткой `goto`. Еще одним критерием недостижимого кода является тот факт, что операторы в нем не содержат меток. Исходя из того, что после оператора `if` следует метка `goto`, то код, не содержащий меток, нужно удалить.

## Реализация

Был создан класс, реализующий интерфейс IOptimizer. Была создана функция для нахождения всех меток в коде:
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
private bool CheckLabels(HashSet<string> lables, 
            LinkedListNode<TacNode> ifNode, string targetLabel, List<TacNode> tacNodesToRemove)
        {
            var currentNode = ifNode;
            var line = currentNode.Value;

            while (!Equals(line.Label, targetLabel))
            {
                if (lables.Contains(line.Label))
                {
                    return false;
                } 
                tacNodesToRemove.Add(line);
                currentNode = currentNode.Next;
                line = currentNode.Value;
            }

            return true;
        }
```
В ключевом методе Optimize осуществляется проход по всем линиям трехадресного кода с двумя проверками: если линия является присваиванием и если линиия реализует оператор `if`.
## Тесты

Трехадресный код до применения оптимизации:
```
goto l1
b = 8
t1 = 9 + b
c = t1
l1: a = 15
```
Трехадресный код после применения оптимизации:
```
goto l1
l1: a = 15  
```

Трехадресный код до применения оптимизации:
```
a = 0
t1 = True
if t1 goto L1
goto L2
L1: goto l2
t2 = a + 3
a = t2
b = 8
c = 10
L2:
l2: a = 15
```
Трехадресный код после применения оптимизации:
```
a = 0  
t2 = True  
if t2 goto L1
goto L2
L1: goto l2
L2: 
l2: a = 15    
```

Трехадресный код до применения оптимизации:
```
b = 8
goto l2
t1 = a + 3
a = t1
c = 10
l2: a = 15
```
Трехадресный код после применения оптимизации:
```
b = 8  
goto l2
l2: a = 15     
```
## Вывод
В результате работы была написана функция, которая удаляет недостижимый код из трехадресного кода. Его работоспособность была успешно протестирована.
