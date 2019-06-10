# Вычисление множеств def и use для активных переменных.

## Постановка задачи
Реализовать пару множеств Use(B) и Def(B)
## Команда — исполнитель
M&M

## Зависимости
Зависит от  
- Базовых блоков  
- Трехадресного кода

## Теория
В точке p переменная x является активной, если существует путь, проходящий через p, начинающийся присваиванием, заканчивается ее использованием и на всем промежутке нет других присваиваний переменной x. 

Def(B) - множество переменных, определенных в базовом блоке до любого их использования.
Существует также альтернитивное определение: Def(B) - множество переменных, определенных в базовом блоке. 

Use(B) - множество переменных, определенных в базовом блоке до любого их определения.
## Реализация

Для решения поставленной задачи был реализован DefUseContainer.

```csharp
class DefUseContainer : IExpressionSetsContainer
{
	//множество переменных, значения которых могут использоваться в B до любого их определения
	public HashSet<TacNodeVarDecorator> use = new HashSet<TacNodeVarDecorator>();
	
	//множество переменных, определённых в B до любого их использования
	public HashSet<TacNodeVarDecorator> def = new HashSet<TacNodeVarDecorator>();
	
	public HashSet<TacNode> GetFirstSet() {
		HashSet<TacNode> res = new HashSet<TacNode>();
		foreach (var el in use)
		res.Add(el);
		return res;
	}
	
	public HashSet<TacNode> GetSecondSet() {
		HashSet<TacNode> res = new HashSet<TacNode>();
		foreach (var el in def)
		res.Add(el);
		return res;
	}
	
	public void AddToFirstSet(TacNode line)
	{
		use.Add(line as TacNodeVarDecorator);
	}
	public void AddToSecondSet(TacNode line)
	{
		def.Add(line as TacNodeVarDecorator);
	}
}
```

## Тесты

```csharp
[TestMethod]
public void DefUse_Test1()
{
    var tacContainer = new ThreeAddressCode();
    Utils.AddAssignmentNode(tacContainer, null, "a", "b", "+", "c");
    Utils.AddAssignmentNode(tacContainer, null, "b", "a", "-", "d");
    Utils.AddAssignmentNode(tacContainer, null, "v", "l", "+", "c");
    Utils.AddAssignmentNode(tacContainer, null, "d", "a", "-", "d");
    var cfg = new ControlFlowGraph(tacContainer);
    var defUseContainers = DefUseForBlocksGenerator.Execute(cfg.SourceBasicBlocks);
    var container = defUseContainers.First().Value;
    Assert.IsTrue(
        container.GetSecondSet().SetEquals(new HashSet<TacNode>() {
            new TacNodeVarDecorator { VarName = "a" },
            new TacNodeVarDecorator { VarName = "v" }
        })
    );
    Assert.IsTrue(
        container.GetFirstSet().SetEquals(new HashSet<TacNode>() {
            new TacNodeVarDecorator { VarName = "b" },
            new TacNodeVarDecorator { VarName = "c" },
            new TacNodeVarDecorator { VarName = "d" },
            new TacNodeVarDecorator { VarName = "l" }
    })
    );
}
```

## Вывод
Используя методы, описанные выше, мы получили множества Use(B) и Def(B) для активных переменных.
