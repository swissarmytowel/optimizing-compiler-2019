# Название задачи
Оптимизация общих подвыражений и протяжка копий.

## Постановка задачи
Создать класс, реализующий алгоритм оптимизации общих подвыражений и класс, реализующий алгоритм протяжки копий.

## Команда — исполнитель
Null

## Зависимости
Зависит от:
- Трехадресный код (AW)

## Теория
#### Оптимизация общих подвыражений
Удаление повторных вычислений – поиск идентичных подвыражений
и сохранение результата вычисления во временной переменной для 
последующего повторного использования.

Пример:
```
a = b * c + g; 
d = b * c * d; 
 => CSE =>
tmp = b * c; 
a = tmp + g;
d = tmp * d; 
```

#### Протяжка копий
Процесс замены переменных их значениями.

Пример:
```
y = x;
z = 3 + y 
=> copy propagation =>
z = 3 + x
```

## Реализация

#### Оптимизация общих подвыражений

Был реализован класс CommonSubexprOptimization. 

```csharp
public class CommonSubexprOptimization : IOptimizer
{
... 
/// Создаём словарь для записи выражений 
var nodesByExpression = new Dictionary<string, LinkedListNode<TacNode>>();
...
/// Если мы встретили новое выражение, то записываем в словарь
if (!nodesByExpression.ContainsKey(expression))
	nodesByExpression.Add(expression, currentNode);
...
/// Если заметили повтор выражения, то производим переприсваивание
/// x = a + b
/// y = a + b ---> y = x;
if (IsPossibleToOptimize(node, currentNode, 
	assignmentNode.FirstOperand, assignmentNode.SecondOperand))
{
	assignment.FirstOperand = assignmentNode.LeftPartIdentifier;
	assignment.Operation = null;
	assignment.SecondOperand = null;
	...
///  Заменяем данные о старом выражении на новые.
else
	nodesByExpression[expression] = currentNode;
	...
/// Переход на след строку кода
currentNode = currentNode.Next;
...
```

#### Протяжка копий

Был реализован класс CopyPropagationOptimization.

```csharp
 public class CopyPropagationOptimization : IOptimizer
{
...
/// Создаём словарь для записи переменных 
var directAssignments = new Dictionary<string, string>();
...
/// Если мы встречаем переприсваивние ранее записанной пременной, то удаляем со словаря
if (directAssignments.ContainsKey(assignmentNode.LeftPartIdentifier))
	directAssignments.Remove(assignmentNode.LeftPartIdentifier);

/// Если не производятся никакие операции и встретели новое присваивание, то записываем новую переменную в словарь
if (assignmentNode.Operation == null)
{
	if (!int.TryParse(assignmentNode.FirstOperand, out int firstOpValue))
	{
		var id = assignmentNode.LeftPartIdentifier;
		var firstOp = assignmentNode.FirstOperand;
		directAssignments.Add(id, firstOp);
	}
}
/// a = x
/// b = [a] + 1 ---> b = x + 1					
if (directAssignments.ContainsKey(assignmentNode.FirstOperand))
{
	assignmentNode.FirstOperand = directAssignments[assignmentNode.FirstOperand];
...
/// a = x
/// b = 1 - [a] ---> b = 1 - x
if (assignmentNode.SecondOperand != null && directAssignments.ContainsKey(assignmentNode.SecondOperand))
{
	assignmentNode.SecondOperand = directAssignments[assignmentNode.SecondOperand];
...

/// Переход на след строку кода
currentNode = currentNode.Next;
...
```

## Тесты

### Оптимизация общих подвыражений

#### INPUT:
```
t1 = a + b
t2 = c + d
t3 = c + d
t4 = a + b
```
#### OUTPUT:
```
t1 = a + b
t2 = c + d
t3 = t3 = t2
t4 = t4 = t1
```

### Протяжка копий

#### INPUT:
```
a = b
c = b - a
d = c + 1
e = d * a 
a = 35 - f
k = c + a
```
#### OUTPUT:
```
a = b
 c = b - b
d = c + 1
e = d * b
a = 35 - f
k = c + a
```

## Вывод
Используя метод, описанные выше, мы смогли выполнить оптимизацию общих подвыражений и протяжку копий в пределах базового блока. 
