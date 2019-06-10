# Удаление мертвого кода на основе ИТА для активных переменных.

## Постановка задачи
Необходимо реализовать алгоритм удаления мертвого кода на основе ИТА для активных переменных

## Команда — исполнитель
TZ

## Зависимости
Данная задача зависит от задачи команды ЗГ - ИТА для активных переменных.

## Теория
Алгоритм удаления мертвого кода на основе ИТА для активных переменных позволяет избавиться от допущения, принимаемого в алгоритме удаления мертвого кода для произвольного блока. А именно то, что все переменные на выходе блока считаются живыми.

## Реализация
Для того, чтобы избавиться от упомянутого выше допущения, была переопределена функция инициализации состояний переменных. Переменные объявляются мертвыми, если об обратном нам не говорит их наличие в OUT текущего блока.
```csharp
foreach (var item in outData)
            {
                if (result.ContainsKey(item.ToString()))
                    result[item.ToString()] = true;
            }
```
## Тесты
```csharp
public void Optimize_SimpleBlock()
        {
            TmpNameManager.Instance.Drop();
            /*
             *  
	            x = a;  To be removed
                x = b;
                y = x + 1;
            */

            var tacContainer = new ThreeAddressCode();
            Utils.AddAssignmentNode(tacContainer, null, "x", "a");
            Utils.AddAssignmentNode(tacContainer, null, "x", "b");
            Utils.AddAssignmentNode(tacContainer, null, "y", "x", "+", "1");
            Utils.AddAssignmentNode(tacContainer, null, "e", "d", "*", "a");

            var expectedResult = new ThreeAddressCode();
            Utils.AddAssignmentNode(expectedResult, null, "x", "b");
            Utils.AddAssignmentNode(expectedResult, null, "y", "x", "+", "1");
            Utils.AddAssignmentNode(expectedResult, null, "e", "d", "*", "a");

            var optimization = new DeadCodeOptimization();

            var isOptimized = optimization.Optimize(tacContainer);

            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacContainer.ToString(), expectedResult.ToString());
            isOptimized = optimization.Optimize(tacContainer);
            Assert.IsFalse(isOptimized);

        }
```
## Вывод
Используя метод, описанные выше, мы смогли использовать удаление мертвого кода на основе ИТА для активных переменных. Что позволило удалять мертвый код, находящийся в конце блока.
