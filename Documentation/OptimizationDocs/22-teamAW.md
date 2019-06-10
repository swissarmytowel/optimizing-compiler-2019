# Протяжка копий на основе Def-Use в пределах базового блока.

## Постановка задачи
Протянуть копии так	далеко	по	тексту	программы,	как	только это	возможно в пределах базового блока.

## Команда — исполнитель
AW

## Зависимости
Зависит от:
- Трехадресный код (AW)
- Вычисление Def-Use в пределах базового блока (AW)

## Теория
Протяжка копий состоит в обнаружении использований (Use) и подстановке на место этих использований копий, которые являются правой частью оператора присваивания при определении данной переменной (Def).
Распространение копий происходит так	далеко	по	тексту	программы,	как	только это	возможно в пределах базового блока.

## Реализация
Метод для протяжки копий на основе Def-Use в пределах базового блока:
```csharp
using VarNodePair = System.Tuple<string, System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>>;
...
/// <summary>
/// Здесь хранятся Def и Use всех переменных в пределах базового блока
/// </summary>
private readonly DefUseDetector _detector;
...
/// <summary>
/// Метод, выполняющий протяжку копий, если это возможно.
/// Данную оптимизацию можно выполнять несколько раз подряд, до тех пор,
/// пока текст программы в виде трехадресного кода не перестанет изменяться.
/// </summary>
/// <param name="tac"> Трехадресный код программы </param>
/// <returns> Если результат оптимизации изменил трехадресный код, т.е. оптимизация
/// была выполнена, то возвращаем true, иначе - false </returns>
public bool Optimize(ThreeAddressCode tac)
{
    var initialTac = tac.ToString();

    // Получение ссылки на первый node трехадресного кода
    var node = tac.First;

    // Обход трехадресного кода сверху - вниз
    while (node != null)
    {
        if (node.Value is TacAssignmentNode assignment)
        {
            if (assignment.SecondOperand == null && Utility.Utility.IsVariable(assignment.FirstOperand))
            {
                var key = new VarNodePair(assignment.LeftPartIdentifier, node);
                if (_detector.Definitions.ContainsKey(key))
                {
                    foreach (var usage in _detector.Definitions[key])
                    {
                        ChangeByVariable(usage.Value, assignment);
                        var keyAdded = new VarNodePair(assignment.FirstOperand, usage);
                        var keyDefenition = new VarNodePair(assignment.FirstOperand, node);
                        _detector.Usages[keyAdded] = _detector.Usages[keyDefenition];
                        if (_detector.Usages[keyDefenition] != null)
                        {
                            var keyUsage = new VarNodePair(assignment.FirstOperand, _detector.Usages[keyDefenition]);
                            _detector.Definitions[keyUsage].Add(node);
                        }
                    }
                    _detector.Definitions.Remove(key);
                }
            }
        }
        node = node.Next;
    }
    return !initialTac.Equals(tac.ToString());
}
```
---
Вспомогательные методы, которые были использовании для протяжки констант на основе Def-Use в пределах базового блока:
```csharp
/// <summary>
/// Проверка: является ли операнд переменной (не bool, int или double)
/// </summary>
/// <param name="expression"> Операнд, который проверяем </param>
/// <returns> Если операнд является переменной, то метод возвращает true,
/// иначе - false </returns>
public static bool IsVariable(string expression) => int.TryParse(expression, out _) == false
                                                && double.TryParse(expression, out _) == false
                                                && bool.TryParse(expression, out _) == false;
/// <summary>
/// Замена использования на копию, которую протягиваем
/// </summary>
/// <param name="node"> Нода с определением переменной </param>
/// <param name="replacingNode"> Нода, в которой хотим произвести замену использования на копию </param>
private void ChangeByVariable(TacNode node, TacAssignmentNode replacingNode)
{
    switch (node)
    {
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
```csharp
i = 2
t1 = 4 * i
a1 = t1
b = i
c = i
i = 3
t2 = i * i
if t2 goto L1
goto L2
L1: t3 = 4 * i
t4 = t3 * d
a3 = t4
L2: t5 = 4 * i
a2 = t5
```

## Вывод
Используя метод, описанные выше, мы смогли выполнить протяжку копий на основе Def-Use в пределах базового блока.
