# Структура для представления трехадресного кода и его генерация

## Постановка задачи

Реализация классов для представления и хранения строк трехадрессного кода, реализация визитора  для генерации строк трехадресного кода по AST-дереву.

## Команда — исполнитель

AW

## Зависимости

Зависит от:

- Парсер языка и генерация AST-дерева (M&M)  

Препятствует:

- Трехадресный код &mdash; это "язык оптимизаций", на базе него построена вся дальнейшая работа.

## Теория

Трехадресный код &mdash; это представление программы как последовательности операторов вида: `a = b op c`, где `a`, `b`, `c` &mdash; это имена переменных или константы, а `op` &mdash; это бинарная арифметическая или логическая операция.

Основным принципом трехадресного кода является то, что каждая строка не может содержать в себе более трех адресов.

Составные выражения при генерации трехадресного кода должны быть разбиты на подвыражения, каждое из которых должно быть присвоено соответствующей временной переменной, название у которых было принято единым: `tn`, где `n` &mdash; это уникальное для текущей временной переменной число-номер, обозначающий ее порядок при проведении генерации (нумерация временных переменных сквозная в рамках одного времени работы компилятора).

Помимо операторов присваивания с двумя операндами и бинарной операцией между ними, в трехадресном коде могут содержаться следующие конструкции:

- Метка строки `Ln:` для совершения перехода (каждая строка трехадресного кода может иметь метку);

- оператор безусловного перехода `goto Ln`;

- оператор условного перехода `if cond goto Ln`, где `cond` &mdash; это условие совершения прыжка к строке с меткой `Ln`;

- присваивания с унарными операциями `t1 = -t2`.

- вызовы функций в качестве первого и второго операндов оператора присваивания `a = func()`

## Реализация

Представление конструкций трехадресного кода (далее &mdash; ТАК) решено было выполнить в качестве объектов-наследников абстрактного класса:

```csharp
namespace SimpleLang.TACode.TacNodes
{
    /// <summary>
    /// Абстрактный класс для строки трехадресного кода
    /// </summary>
    public abstract class TacNode
    {
        /// <summary>
        /// Метка текущей строки трехадресного кода
        /// </summary>
        public string Label { get; set; } = null;
        /// <summary>
        /// Флаг-индикатор того, что данная метка была сгенерирована автоматически при обработке
        /// конструкций if/while/for
        /// </summary>
        public bool IsUtility { get; set; } = false;

        public override string ToString() => Label != null ? Label + ": " : "";
    }
}
```
Данный класс содержит лишь метку, как единственную общую характеристику каждой конструкции ТАК.
В качестве наследников `TacNode` были реализованы следующие классы:

- `TacAssignmentNode` &mdash; оператор присваивания

```csharp
namespace SimpleLang.TACode.TacNodes
{
    /// <inheritdoc />
    /// <summary>
    /// Представление оператора присваивания
    /// </summary>
    public class TacAssignmentNode : TacNode
    {
        /// <summary>
        /// Идентификатор адреса для записи вычисленного значения правой части
        /// </summary>
        public string LeftPartIdentifier { get; set; }
        /// <summary>
        /// первый операнд правой части
        /// </summary>
        public string FirstOperand { get; set; }
        /// <summary>
        /// Второй операнд правой части
        /// </summary>
        public string SecondOperand { get; set; } = null;
        /// <summary>
        /// Выполняемая операция
        /// </summary>
        public string Operation { get; set; } = null;

        public override string ToString()
        {
            var rightPart = (Operation == null) && (SecondOperand == null)
                ? $"{FirstOperand}"
                : $"{FirstOperand} {Operation} {SecondOperand}";
            return $"{base.ToString()}{LeftPartIdentifier} = {FirstOperand} {Operation} {SecondOperand}";
        }
    }
}
```

- `TacGotoNode` &mdash; оператор безусловного перехода

```csharp
namespace SimpleLang.TACode.TacNodes
{
    /// <inheritdoc />
    /// <summary>
    /// Представление оператора безусловного перехода goto
    /// </summary>
    public class TacGotoNode : TacNode
    {
        /// <summary>
        /// Метка для строки, на которую ведется переход
        /// </summary>
        public string TargetLabel { get; set; }
        
        public override string ToString() => $"{base.ToString()}goto {TargetLabel}";
    }
}
```

- `TacIfGotoNode` &mdash; оператор перехода по условию

```csharp
namespace SimpleLang.TACode.TacNodes
{
    /// <inheritdoc />
    /// <summary>
    /// Представление условного перехода if-goto
    /// </summary>
    public class TacIfGotoNode : TacGotoNode
    {
        /// <summary>
        /// Условие для перехода на целевую метку
        /// </summary>
        public string Condition { get; set; }

        public override string ToString() => (Label != null ? Label + ": " : "") + $"if {Condition} goto {TargetLabel}";
    }
}
```

- `TacEmptyNode` &mdash; пустой оператор, наследник `TacNode` в виде неабстрактного класса для возможности инстанцирования

## Тесты

Узнать как должны выглядить тесты в докуметации.

## Вывод

Была реализована структура для хранения 
