# Тестирование

В данную директорию помещаются тесты, выполняемые вручную с помощью сравнения трех-адресного кода двух программ. 

## Порядок действий

1. Назначьте запускаемым проект с тестированием.
1. В директории `Handlers` создайте наследника класса `TextTestsHandler`. Например, `CommonSubexprTestsHandler`.
2. В методе `ProcessTAC(...)` опишите действия, которые надо выполнить над трех-адресным кодом.
3. В директории `Tests` создайте свою директорию по тому же принципу, что в юнит-тестах. Внутрь поместите папку с именем тестируемого класса. Например, `Tests\Optimizations\CommonSubexprOptimization\`.
4. В созданную директорию добавляйте файлы парами `{номер_теста}-in.txt` (исходный код программы) и `{номер_теста}-out.txt` (ожидаемый код программы после оптимизации).
5. Добавьте создание объекта вашего класса в `Main()` и вызов метода обработки файлов. Пример см. ниже.
6. В вашей директории должны появиться файлы с именами `{номер_теста}-in-tac.txt` (трех-адресный код программы после оптимизации) и `{номер_теста}-out-tac.txt` (трех-адресный код ожидаемого результата).
7. **ОБЯЗАТЕЛЬНО** сравните полученные результаты и убедитесь, что ваша оптимизация работает корректно.
8. Вы великолепны! 

## Пример

```c#
public class CommonSubexprTestsHandler : TextTestsHandler
{
  public CommonSubexprTestsHandler(string directoryName) : base(directoryName) { }

  protected override ThreeAddressCode ProcessTAC(ThreeAddressCode tacContainer)
  {
      var optimization = new CommonSubexprOptimization();
      optimization.Optimize(tacContainer);
      return tacContainer;
  }
}

// ...

var path = GetFullPath("Tests\\Optimizations\\CommonSubexprOptimization\\");
var handler = new CommonSubexprTestsHandler(path);
handler.ProcessAllFilesInDirectory();
```
