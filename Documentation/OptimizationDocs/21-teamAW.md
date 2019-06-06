# Название задачи
Протяжка констант на основе Def-Use в пределах базового блока.

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
        case TacIfGotoNode ifGotoNode:
            //ifGotoNode.Condition = replacingNode.FirstOperand;
            break;
    }
}
```

## Тесты
Узнать как должны выглядить тесты в докуметации.

## Вывод
Используя метод, описанные выше, мы смогли выполнить протяжку констант на основе Def-Use в пределах базового блока. 

