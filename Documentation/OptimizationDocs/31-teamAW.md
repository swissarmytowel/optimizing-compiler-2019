# Название задачи
Хранение IN-OUT.

## Постановка задачи
Реализовать	алгоритм заполнения IN-OUT  множеств для всех базовых блоков программы.
## Команда — исполнитель
AW

## Зависимости
Зависит от:
- Трехадресный код (AW)
- Разбиение на базовые блоки (Enterprise)
- Заполнение множеств GenB и KillB (Kt, Enterprise)

## Теория
![](../images/31-teamAW-1.PNG)
Оператор сбора – операция над множествами. Для данной задачи оператором сбора является объединение.

## Реализация
Методы для нахождения и хранения In-Out по всем базовым блокам:
```csharp
public Dictionary<OneBasicBlock, HashSet<TacNode>> In = new Dictionary<OneBasicBlock, HashSet<TacNode>>();
public Dictionary<OneBasicBlock, HashSet<TacNode>> Out = new Dictionary<OneBasicBlock, HashSet<TacNode>>();

/// <summary>
/// Построение множеств In и Out на основе gen и kill для каждого базового блока
/// </summary>
/// <param name="bBlocks"> Все базовые блоки </param>
/// <param name="genKillContainers"> Все множества gen и kill по всем базовым блокам </param>
public InOutContainer(BasicBlocks bBlocks,
    Dictionary<OneBasicBlock, IExpressionSetsContainer> genKillContainers)
{
    for (var i = 0; i < bBlocks.BasicBlockItems.Count; ++i)
    {
        var curBlock = bBlocks.BasicBlockItems[i];

        if (i == 0)
        {
            In[curBlock] = new HashSet<TacNode>();
        }
        else
        {
            var prevBlock = bBlocks.BasicBlockItems[i - 1];
            FillInForBasicBlock(curBlock, prevBlock);
        }

        FillOutForBasicBlock(curBlock, genKillContainers);
    }
}

/// <summary>
/// Заполняем множество In для текущего базового блока
/// Т.к. каждый последующий In - это объединение Out всех предыдущих блоков,
/// то достаточно знать только In и Out предыдущего блока, чтобы получить 
/// значение In текущего блока
/// </summary>
/// <param name="curBlock"> Рассматриваемы базовый блок </param>
/// <param name="prevBlock"> Предыдущий базовый блок </param>
public void FillInForBasicBlock(OneBasicBlock curBlock, OneBasicBlock prevBlock)
{
    In[curBlock] = new HashSet<TacNode>();
    In[curBlock].UnionWith(In[prevBlock]);
    In[curBlock].UnionWith(Out[prevBlock]);
}

/// <summary>
/// Заполняем множество OUT для текущего базового блока
/// </summary>
/// <param name="curBlock"> Рассматриваемы базовый блок </param>
/// <param name="genKillContainers"> Информация о gen и kill </param>
public void FillOutForBasicBlock(OneBasicBlock curBlock,
    Dictionary<OneBasicBlock, IExpressionSetsContainer> genKillContainers)
{
    if (genKillContainers.ContainsKey(curBlock))
    {
        Out[curBlock] = new HashSet<TacNode>(genKillContainers[curBlock].GetFirstSet()
            .Union(In[curBlock]
                .Except(genKillContainers[curBlock].GetSecondSet())));
    }
    else
    {
        Out[curBlock] = new HashSet<TacNode>(In[curBlock]);
    }
}
```

## Тесты
Узнать как должны выглядить тесты в докуметации.

## Вывод
Используя метод, описанные выше, мы смогли построить множество In-Out для всех базовых блоков. 
