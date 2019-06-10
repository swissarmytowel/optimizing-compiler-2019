# Local Value Numbering (LVN).

## Постановка задачи
Реализовать алгоритм LVN.

## Команда — исполнитель
ZG

## Зависимости
Зависит от:

-   Трёхадресный код
-   Выделение базовых блоков

## Теория
Local Value Numbering принадлежит к локальным оптимизациям. Она
находит избыточные (redundant) выражения в одном ББл и заменяет их
на ранее вычисленное значение. Для этого все значения специальным
образом нумеруются.

**Алгоритм LVN**<br />
Алгоритм обходит базовый блок и присваивает разные значения
каждому значению, вычисляемому в ББл. Пусть все инструкции ББл
имеют вид *Ti = Li Opi Ri*. Заводим словарь dict, отображающий хеш-ключи на номера значений
(value numbers). Ключи представляют собой строки. Вначале словарь
пуст. Будем обозначать номер значения через *VN(Li)*.
Алгоритм LVN по тройке *(VN(Li), Opi, VN(Ri))* формирует строку, записывает её в ключ *Key* и затем присваивает
*dict[Key] = номер*; *dict[Ti] = номер*.

## Реализация
Создаём переменную для нумерации значений и словари, необходимые для работы алгоритма:
```csharp
var currentNumber = 0;
var valueToNumber = new Dictionary<string, int>();
var valueDict = new Dictionary<int, LinkedListNode<TacNode>>();
var numberToT = new Dictionary<int, string>();
var parameters = tac.TACodeLines.OfType<TacAssignmentNode>().Select(e => e.LeftPartIdentifier);
```
Итерируем по узлам трёхадресного кода и получаем значения *Ti*, *Li*, *Opi*, *Ri* из инструкций вида
*Ti = Li Opi Ri*:
```csharp
var Ti = assigned.LeftPartIdentifier;
var Li = assigned.FirstOperand;
var Ri = assigned.SecondOperand;
var Opi = assigned.Operation ?? String.Empty;
```
Нумеруем значения:
```csharp
int valL;
int valR;
if (!valueToNumber.TryGetValue(Li, out valL))
{
    valL = currentNumber++;
    valueToNumber.Add(Li, valL);
}

if (!valueToNumber.TryGetValue(Ri, out valR))
{
    valR = currentNumber++;
    valueToNumber.Add(Ri, valR);
}
```
Формируем хэш:
```csharp
var hash = $"{valL} {Opi} {valR}".Trim();
var hashReversed = $"{valR} {Opi} {valL}".Trim();
```
Далее проверяем, если хэш-ключ содержится в таблице, то заменяем операцию *i*
копией значения в *Ti* и связываем номер значения с *Ti*:
```csharp
   isUsed = true;
   valueToNumber[Ti] = tmp;
   var paramToNumber = valueToNumber.Where(e => parameters.Contains(e.Key) && e.Key != Ti);
   var findable = paramToNumber.FirstOrDefault(e => e.Value == tmp);
   if (findable.Key != null)
   {
       assigned.FirstOperand = findable.Key;
       assigned.Operation = null;
       assigned.SecondOperand = null;
   }
```
Иначе вставляем новый номер значения в таблицу:
```csharp
var tmpTacNode = new TacAssignmentNode()
{
    LeftPartIdentifier = TmpNameManager.Instance.GenerateTmpVariableName(),
    FirstOperand = numberToT[tmp]
};
tac.TACodeLines.AddAfter(valueDict[tmp], tmpTacNode);
assigned.FirstOperand = tmpTacNode.LeftPartIdentifier;
assigned.Operation = null;
assigned.SecondOperand = null;
```  
## Тесты
&mdash;

## Вывод
Реализован алгоритм LVN для замены избыточных выражений.
