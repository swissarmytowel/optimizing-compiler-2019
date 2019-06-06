# Тестирование

Статья о тестировании: [habr.com](https://habr.com/ru/post/169381/)

Прогресс: [docs.google.com](https://docs.google.com/spreadsheets/d/1u98H52a17wpgJkMp_O0qtMFU6mfNi8Iehc6hL6ILmbQ/edit?usp=sharing)

## Основные правила

* Тесты должны быть достоверными.
* Каждый тестирующий класс должен тестировать только одну сущность.
* Каждый тест должен проверять только одну вещь.
* Необходимо соблюдать указанные ниже правила именования.

## Расположение тестов

* Все тесты располагаются в проекте и, соответственно, директории с названием `UnitTests`. 
* Для каждой категории тестов создается своя директория (и пространство имен). Название директории повторяет название директории 
из основного проекта, в котором находятся тестируемые классы. 

#### Пример

Было: `Compilers/Optimizations/`. 

Стало: `UnitTests/Optimizations/`.

## Cтиль именования для тестовых классов

1. Из названия тестируемого класса удаляется слово, совпадающее с названием директории. 
2. К полученному имени класса добавляется слово `Tests`. 

#### Пример

Было: `Compilers/Optimizations/CommonSubexprOptimization.cs`. 

Стало: `UnitTests/Optimizations/CommonSubexprTests.cs`.

## Cтиль именования для методов

Все тестирующие методы должны придерживаться следующего стиля именования:
`{ТестируемыйМетод}_{КраткоеОписаниеРассматриваемогоСлучая}`.

#### Пример

`public void Optimize_ThreeTimesRepeatedExpression() {...}`

## Стиль написания тела теста

В качестве стиля написания тела используется шаблон "Arrange-Act-Assert".

#### Пример

См. ниже.

## Вспомогательные методы

В статическом классе `Utils` реализованы вспомогательные методы для тестирования, которые позволяют добавлять узлы трехадресного кода
непосредственно в некоторый контейнер трехадресного кода `ThreeAddressCode` или в контейнер визитора для трехадресного кода 
`ThreeAddressCodeVisitor` с написанием минимального количества кода.

#### Пример

Было: `L1: a = b + c`. 

Стало: `Utils.AddAssignmentNode(container, "L1", "a", "b", "+", "c");`.

## Пример

```c#
namespace UnitTests.Optimizations
{
    [TestClass]
    public class CommonSubexprTests
    {
        [TestMethod]
        public void Optimize_ThreeTimesRepeatedExpression()
        {
            // arrange
            var tacVisitor = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(tacVisitor, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(tacVisitor, null, "t2", "2");
            Utils.AddAssignmentNode(tacVisitor, null, "t3", "a", "+", "b");
            Utils.AddAssignmentNode(tacVisitor, null, "t4", "10");
            Utils.AddAssignmentNode(tacVisitor, null, "t5", "a", "+", "b");

            var expectedResult = new ThreeAddressCodeVisitor();
            Utils.AddAssignmentNode(expectedResult, "L1", "t1", "a", "+", "b");
            Utils.AddAssignmentNode(expectedResult, null, "t2", "2");
            Utils.AddAssignmentNode(expectedResult, null, "t3", "t1");
            Utils.AddAssignmentNode(expectedResult, null, "t4", "10");
            Utils.AddAssignmentNode(expectedResult, null, "t5", "t1");
            
            // act
            var optimization = new CommonSubexprOptimization();
            var isOptimized = optimization.Optimize(tacVisitor.TACodeContainer);
            
            // assert
            Assert.IsTrue(isOptimized);
            Assert.AreEqual(tacVisitor.ToString(), expectedResult.ToString());
        }
    }
}
```
