# Определение того, является ли ребро обратимым и является ли CFG приводимым

## Постановка задачи
Определить, является ли ребро обратимым и является ли CFG приводимым.
## Команда — исполнитель
Kt

## Зависимости
Зависит от  
- Базовых блоков  
- Трехадресного кода  
- Графа потока управления 

## Теория

**Определение.** Обратным в графе CFG называется ребро a → b, у
которого b доминирует над a.
**Утверждение.** Не любое отступающее ребро является обратным.
**Определение.** Граф потока управления называется приводимым если
все его отступающие рёбра являются обратными.
## Реализация

Для решения поставленной задачи был реализован следующий метод :

```csharp
...
public static bool IsReducibility(ControlFlowGraph cfg)
{
	var edgeClassifierService = new EdgeClassifierService(cfg);
	var retreatingEdges = edgeClassifierService.RetreatingEdges;
	var backEdges = edgeClassifierService.BackEdges;
	return retreatingEdges.SetEquals(backEdges);
}
...
```

## Тесты
```csharp
Console.WriteLine("\nCFG TASKS START");
Console.WriteLine(cfg);
var edgeClassifierService = new EdgeClassifierService(cfg);
Console.WriteLine("EdgeClassifierService: \n" + edgeClassifierService);
bool isReducibility = DSTReducibility.IsReducibility(cfg);
Console.WriteLine("IsReducibility: " + isReducibility);
Console.WriteLine("\nCFG TASKS END");
```

## Вывод
Используя методы, описанные выше, мы определили, является ли ребро обратимым и является ли CFG приводимым.
