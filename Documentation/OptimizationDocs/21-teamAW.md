# Протяжка констант на основе Def-Use в пределах базового блока.

## Постановка задачи
Протянуть константы так	далеко	по	тексту	программы,	как	только это	возможно в пределах базового блока. 

## Команда — исполнитель
AW

## Зависимости
Зависит от:
- Трехадресный код (AW)
- Вычисление Def-Use в пределах базового блока (AW)

## Теория
Протяжка констант состоит в обнаружении использований (Use) и подстановке на место этих использований констант, которые являются правой частью оператора присваивания при определении данной переменной (Def).
Распространение констант происходит	так	далеко	по	тексту	программы,	как	только это	возможно в пределах базового блока. 

## Реализация
Метод для протяжки констант на основе Def-Use в пределах базового блока:
```csharp
using VarNodePair = System.Tuple<string, System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>>;
...
/// <summary>
/// Здесь хранятся Def и Use всех переменных в пределах базового блока
/// </summary>
private readonly DefUseDetector _detector; 
...
/// <summary>
/// Метод, выполняющий протяжку констант, если это возможно. 
/// Данную оптимизацию можно выполнять несколько раз подряд, до тех пор, 
/// пока текст программы в виде трехадресного кода не перестанет изменяться.
/// </summary>
/// <param name="tac"> Трехадресный код программы </param>
/// <returns> Если результат оптимизации изменил трехадресный код, т.е. оптимизация 
/// была выполнена, то возвращаем true, иначе - false </returns>
public bool Optimize(ThreeAddressCode tac)
{
    var initialTac = tac.ToString();

    // Получение ссылки на последний node трехадресного кода
    var node = tac.Last;

    // Обход трехадресного кода снизу - вверх
    while (node != null)
    {
        if (node.Value is TacAssignmentNode assignment)
        {
            if (assignment.SecondOperand == null && Utility.Utility.IsNum(assignment.FirstOperand))
            {
                var key = new VarNodePair(assignment.LeftPartIdentifier, node);
                if (_detector.Definitions.ContainsKey(key))
                {
                    foreach (var usage in _detector.Definitions[key])
                    {
                        ChangeByConst(usage.Value, assignment);
                    }
                    _detector.Definitions.Remove(key);
                }
            }
        }
        node = node.Previous;
    }
    return !initialTac.Equals(tac.ToString());
}
```

---
Вспомогательные методы, которые были использовании для протяжки констант на основе Def-Use в пределах базового блока:

```csharp
/// <summary>
/// Проверка: является ли операнд числом (int или double) 
/// </summary>
/// <param name="expression"> Операнд, который проверяем </param>
/// <returns> Если операнд является числом, то метод возвращает true, 
/// иначе - false </returns>
public static bool IsNum(string expression) => int.TryParse(expression, out _) == true
    || double.TryParse(expression, out _) == true;

/// <summary>
/// Замена использования на константу, которую протягиваем
/// </summary>
/// <param name="node"> Нода с определением переменной </param>
/// <param name="replacingNode"> Нода, в которой хотим произвести замену использования на константу </param>
private void ChangeByConst(TacNode node, TacAssignmentNode replacingNode)
{
    switch (node) {
        case TacAssignmentNode assNode:
            if (assNode.FirstOperand.Equals(replacingNode.LeftPartIdentifier))
            {
                assNode.FirstOperand = replacingNode.FirstOperand;
            } 
                    
            if (assNode.SecondOperand != null && assNode.SecondOperand.Equals(replacingNode.LeftPartIdentifier))
            {
                assNode.SecondOperand = replacingNode.FirstOperand;
            }
            break;
    }
}
```

## Тесты
#### INPUT: 
```csharp
i = 2;
a1 = 4 * i; 
b = i;
c = b;
i = 3;
if (b * c) { 
a3 = 4 * i * d; 
} 
a2 = 4 * i; 
```

#### Three address code:
```csharp
i = 2
t1 = 4 * i
a1 = t1
b = i
c = b
i = 3
t2 = b * c
if t2 goto L1
goto L2
L1: t3 = 4 * i
t4 = t3 * d
a3 = t4
L2: t5 = 4 * i
a2 = t5
```

#### OUTPUT:
Результат после одной итерации протяжки констант:
```csharp
i = 2
t1 = 4 * 2
a1 = t1
b = 2
c = b
i = 3
t2 = b * c
if t2 goto L1
goto L2
L1: t3 = 4 * 3
t4 = t3 * d
a3 = t4
L2: t5 = 4 * 3
a2 = t5
```

Конечный результат протяжки констант:
```csharp
i = 2
t1 = 4 * 2
a1 = t1
b = 2
c = 2
i = 3
t2 = 2 * 2
if t2 goto L1
goto L2
L1: t3 = 4 * 3
t4 = t3 * d
a3 = t4
L2: t5 = 4 * 3
a2 = t5
```

## Вывод
Используя метод, описанные выше, мы смогли выполнить протяжку констант на основе Def-Use в пределах базового блока. 

