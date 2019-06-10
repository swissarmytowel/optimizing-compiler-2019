# Доступные выраж.-множества e_genB, e_killB. Передаточная ф-ия ББЛ в fB = e_genB U (x - e_killB).

## Постановка задачи
Необходимо реализовать алгоритм заполнения множеств e_genB и e_killB, а так же определить передаточную функцию для ИТА.

## Команда — исполнитель
TZ

## Зависимости
От данной задачи зависит команда ЗГ, задача ИТА для доступных выражений.

## Теория
**Определение.** x+y доступно в точке p если любой путь от входа к p
вычисляет x+y и после последнего вычисления до достижения p нет
присваиваний x и y

**Определение.** Множество **e_genB** представляет собой все выражения определеные в блоке, такие что в случае переопределенния одного из операндов выражения, выражение было вычеслено вновь.

**Определение.** Множество **e_killB** представляет собой все выражения, уничтоженные операциями присваивания. В том числе, из других блоков.

## Реализация
Для заполнения множества e_killB нам потребуется универсальное множество - множество, которое содержит все выражения кода.
```csharp
public HashSet<TacNode> FillUniversalSet(BasicBlocks bblocks)
        {
            var uSet = new HashSet<TacNode>();
            foreach (var bblock in bblocks)
            {
                foreach (var line in bblock)
                {
                    if (line is TacAssignmentNode assignmentNode)
                    {
                        uSet.Add(line);
                    }
                }
            }
            return uSet;
        }
```

Проходя по всем линиям трехадресного кода, мы обрабатываем только объекты AssignmentNode. В случае, когда левая часть (объект которому присваивается выражение) является временной переменной, мы добавляем выражение во множество e_genB и удаляем e_killB.
Если же левая часть является обычной переменной, мы выполняем обратное действие
```csharp
...........
if (line is TacAssignmentNode assignmentNode)
                    {
                        if (IsTempVariable(assignmentNode.LeftPartIdentifier))
                        {
                            genSet1.Add(line);
                            killSet1.Remove(line);
                        }
                        else
                        {
                            AddToKill(killSet1, uSet, assignmentNode.LeftPartIdentifier);
                            DeleteFromGen(ref genSet1, assignmentNode.LeftPartIdentifier);
                        }
...........
```
Учитывая специфику реализации, для определения передаточной функции от нас потребовалось только переопределить операцию Intersection для HashSet<TacNode>:
```csharp
public class IntersectCollectionOperator<TacNode> : ICollectionOperator<TacNode>
    {
        private HashSet<TacNode> IntersectAvailableExpressions(HashSet<TacNode> firstSet, HashSet<TacNode> secondSet)
        {
            var res = new HashSet<TacNode>();
            foreach (var item1 in firstSet)
                foreach (var item2 in secondSet)
                    if ((item1 as TacAssignmentNode).FirstOperand == (item2 as TacAssignmentNode).FirstOperand && 
                        (item1 as TacAssignmentNode).Operation == (item2 as TacAssignmentNode).Operation &&
                        (item1 as TacAssignmentNode).SecondOperand == (item2 as TacAssignmentNode).SecondOperand)
                    {
                        res.Add(item1);
                    }
            return res;
        }
        public HashSet<TacNode> Collect(HashSet<TacNode> firstSet, HashSet<TacNode> secondSet)
        {         
            return IntersectAvailableExpressions(firstSet as HashSet<TacNode>, secondSet as HashSet<TacNode>);
        }
    }
```
## Тесты
```csharp
[TestMethod]
        public void Optimize_CombinationOfBlocks()
        {
            /*
            x = b;
            x = a; --> Should be deleted despite of it's latest operation in block
            if (1==1)
            {
            x = b;
            y = x;
            }
            x = 1;
            v = x;
             */
            TmpNameManager.Instance.Drop();
            var tacVisitor = new ThreeAddressCodeVisitor();

            var expectedResult = "t1 = 1 == 1\nif t1 goto L1\n";
            var source = "x = b;\nx = a;\nif (1==1){\nx = b;\n y = x;\n}\n x = 1;\n v = x;\n";

            var scanner = new Scanner();
            scanner.SetSource(source, 0);
            var parser = new Parser(scanner);
            parser.Parse();
            var root = parser.root;

            root.Visit(tacVisitor);
            tacVisitor.Postprocess();

            var cfg = new ControlFlowGraph(tacVisitor.TACodeContainer);
            var defUseContainers = DefUseForBlocksGenerator.Execute(cfg.SourceBasicBlocks);
            var activeVariablesITA = new ActiveVariablesITA(cfg, defUseContainers);
            DeadCodeOptimizationWithITA optimization = new DeadCodeOptimizationWithITA();
            var isOptimized = optimization.Optimize(activeVariablesITA);
            Assert.AreEqual(expectedResult, activeVariablesITA.controlFlowGraph.SourceBasicBlocks.BasicBlockItems[0].ToString());

        }
```
## Вывод
Используя метод, описанные выше, нам удалось реализовать алгоритм заполнения множеств e_genB и e_killB.
