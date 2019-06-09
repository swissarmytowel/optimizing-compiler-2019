# Вычисление Def-Use в пределах базового блока

## Постановка задачи
Для базового блока реализовать вычисление множеств Def-Use.

## Команда — исполнитель
AW

## Зависимости
Зависит от:
- Трехадресный код (AW)

## Теория
Переменные в базовом блоке разделяются на два типа: определения (Def) и использования (Use).

***Def*** &mdash; это определение переменной, т.е. место в коде программы, когда переменной присваивается какое-либо значение.

***Use*** &mdash; это использование переменной, т.е. место в коде программы, когда переменная участвует в выражении.

Каждый Use хранит ссылку на Def переменной или null, если переменная не определяется в данном базовом блоке. Каждый Def хранит список Use данной переменной в пределах базового блока.

```csharp
x = a;			// x определяется (Def); а использвуется (Use)
...
y = x + z;		// y определяется (Def); x, z используются (Use)
...
if (b) {		// b использвуется (Use)
...
}
```

## Реализация
Для реализации данной задачи было решено использовать Def и Use следующего вида:
```csharp
using LinkedListTacNode = System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>;
using VarNodePair = System.Tuple<string, System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>>;

/// <summary>
/// Определения переменных в базовом блоке
/// Ключ: пара имя переменной и нода, где переменная определена
/// Значение: список нодов, где присутствуют использования данного определения переменной 
/// </summary>
public readonly Dictionary<VarNodePair, List<LinkedListTacNode>> Definitions =
    new Dictionary<VarNodePair, List<LinkedListTacNode>>();

/// <summary>
/// Использования переменных в базовом блоке
/// Ключ: пара имя переменной и нода, где переменная используется
/// Значение: нода, где присутствует определение данной используемой переменной
/// </summary>
public readonly Dictionary<VarNodePair, LinkedListTacNode> Usages =
    new Dictionary<VarNodePair, LinkedListTacNode>();
```

Метод для поиска и заполнения Def и Use переменных:

```csharp
public void DetectAndFillDefUse(ThreeAddressCode threeAddressCode)
{
    // Получение ссылки на последний node трехадресного кода
    var lastNode = threeAddressCode.Last;

    // Временный словарь для хранения использований переменной, 
    //пока не будет найдено определение переменной
    // Ключ - имя переменной
    // Значение - список, состоящий из ссылок на node трехадресного кода, 
    //где используется переменная
    var tmpUsagesNodes = new Dictionary<string, List<LinkedListTacNode>>();

    // Обход трехадресного кода снизу - вверх
    while (lastNode != null)
    {
        switch (lastNode.Value)
        {
            case TacAssignmentNode assignmentNode:
            {
                // Добавление использования переменной (первого операнда в выражении) во временный словарь tmpUsagesNodes
                FillTmpUsagesForNode(assignmentNode.FirstOperand, lastNode, tmpUsagesNodes);

                if (assignmentNode.SecondOperand != null)
                {
                    // Добавление использования переменной (второго операнда в выражении, если он есть) во временный словарь tmpUsagesNodes
                    FillTmpUsagesForNode(assignmentNode.SecondOperand, lastNode, tmpUsagesNodes);
                }

                // Создание новой записи в словарь Definitions с текущим идентификатором переменной
                var keyNode = new VarNodePair(assignmentNode.LeftPartIdentifier, lastNode);
                Definitions[keyNode] = new List<LinkedListTacNode>();

                // Если встретили использование переменной, ранее определенной в tmpUsagesNodes  
                if (tmpUsagesNodes.ContainsKey(keyNode.Item1))
                {
                    // Заполнить список Definitions текущей переменной из tmpUsagesNodes
                    Definitions[keyNode] = tmpUsagesNodes[keyNode.Item1];

                    // Заполнить Usages нодами из tmpUsagesNodes
                    foreach (var tmp in tmpUsagesNodes[keyNode.Item1])
                    {
                        Usages[new VarNodePair(assignmentNode.LeftPartIdentifier, tmp)] = lastNode;
                    }

                    // Удаляем из временного словаря запись о просмотренной переменной 
                    tmpUsagesNodes.Remove(keyNode.Item1);
                }
                break;
            }
            case TacIfGotoNode ifGotoNode:
            {
                // В случае goto мы встречаем только использования переменных, поэтому просто добавляем их в tmpUsagesNodes
                FillTmpUsagesForNode(ifGotoNode.Condition, lastNode, tmpUsagesNodes);
                break;
            }
        }
        lastNode = lastNode.Previous;
    }

    // Если для каких-то переменных есть использования, но нет переменных, заполняем Usages, Definitions для данной переменной будет пустым
    FillUsagesWithoutDefinitions(tmpUsagesNodes);
}        
```

---
Вспомогательные методы, которые были использовании при поиске и заполнении определений и использований:

```csharp
/// <summary>
/// Добавление в tmpUses переменной operand ноды, где есть использование данной переменной
/// </summary>
/// <param name="operand"> Переменная, рассматриваемая в данный момент </param>
/// <param name="node"> Нода трехадресного кода, где используется данная переменная </param>
/// <param name="tmpUses"> Временный словарь, где зранится информация об использовании переменных </param>
private void FillTmpUsagesForNode(string operand, LinkedListTacNode node,
    IDictionary<string, List<LinkedListTacNode>> tmpUses)
{
    if (!Utility.Utility.IsVariable(operand)) return;
    if (tmpUses.ContainsKey(operand))
    {
        tmpUses[operand].Add(node);
    }
    else
    {
        tmpUses[operand] = new List<LinkedListTacNode>() {node};
    }
}

/// <summary>
/// Заполнение Usages для переменных, у которых нет определений
/// </summary>
/// <param name="tmpUses"> Временный словарь использований </param>
private void FillUsagesWithoutDefinitions(Dictionary<string, List<LinkedListTacNode>> tmpUses)
{
    foreach (var tmpUse in tmpUses)
    {
        foreach (var usageTacNode in tmpUse.Value)
        {
            Usages[new VarNodePair(tmpUse.Key, usageTacNode)] = null;
        }
    }
}

```

## Тесты
#### INPUT: 
```csharp
i = 2;
a1 = (4 * i);
i = 3;
if (b)
{
  a3 = (4 * i);
}
a2 = (4 * i);
```

#### Three address code:
```csharp
i = 2
t1 = 4 * i
a1 = t1
i = 3
t2 = b
if t2 goto L1
goto L2
L1: t3 = 4 * i
a3 = t3
L2: t4 = 4 * i
a2 = t4
```

#### OUTPUT:
```csharp
DEF:
[a2, a2 = t4  ]: no usages
[t4, L2: t4 = 4 * i]: a2 = t4  
[a3, a3 = t3  ]: no usages
[t3, L1: t3 = 4 * i]: a3 = t3  
[t2, t2 = b  ]: if t2 goto L1
[i, i = 3  ]: L2: t4 = 4 * i, L1: t3 = 4 * i
[a1, a1 = t1  ]: no usages
[t1, t1 = 4 * i]: a1 = t1  
[i, i = 2  ]: t1 = 4 * i

USE:
[t4, a2 = t4  ]: L2: t4 = 4 * i
[t3, a3 = t3  ]: L1: t3 = 4 * i
[t2, if t2 goto L1]: t2 = b
[i, L2: t4 = 4 * i]: i = 3
[i, L1: t3 = 4 * i]: i = 3
[t1, a1 = t1  ]: t1 = 4 * i
[i, t1 = 4 * i]: i = 2
[b, t2 = b  ]: no definitions
```

## Вывод
Используя методы, описанные выше, мы получили Def-Use для каждой переменной в пределах базового блока. Если для какой-либо переменной нет определения, все её использования имеют определение null. Если для какой-либо переменной нет использования, то её определение имеет пустой список использований (Count == 0).
