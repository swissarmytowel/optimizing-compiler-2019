# optimizing-compiler-2019
Project for Optimizing Compilers Development course, IMM&amp;CS, spring 2019

## [Reference to the course page](https://goo.gl/tLTYmW)

## [Lectures PDFs](https://drive.google.com/drive/folders/127Dj3_lesQxzR_1TgBZtKZEX8gE-nLcQ?usp=sharing)

## [Задания](https://github.com/swissarmytowel/optimizing-compiler-2019/tree/master/%D0%97%D0%B0%D0%B4%D0%B0%D0%BD%D0%B8%D1%8F_%D0%A4%D0%BE%D1%82%D0%BE)


|Название команды|Участники|Задания 13.03|Задания 20.03|Задания 27.03|Задания 02.04|Задания 10.04|Задания 17.04|Задания 8.05|Задания 15.05|
|----------------|---------|---------|---------|---------|---------|---------|---------|---------|---------|
|AW|**Галайчук, Рязанова**|4, 11| 2) Вычисление Def - Use  | Трехадресный код | Хранение IN-OUT | Протяжка const на основе ИТА для достиг. перем. |-|-|Для CFG построить дерево доминаторов (ИТА)|
|ЗГ|**Зинченко, Голубев**|6, 12| 4) Свертка const, алгебраические тождества | LVN | - | ИТА для активных переменных + 2-3 теста |Итер. алг.для доступных выраж + 2-3 теста|Обобщенный ИТА (задачи 1-3 + распр. const)|-|
|Enterprise|**Маннаа, Ульянов**|9, 2| 8) Устранение недостижим. кода| Разбиение на ББЛ | GenB/KillB (композиция Fb/Fs)| - |Класс передаточной ф-ии(общий) (см.фото)|Поиск реш-ия м-ом MOP|-|
|ТЗ|**Завгороднев, Тян**|1, 10| 3) Живые и мертвые переменные внутри ББЛ - анализ | - | - | Удаление мертвого кода на основе ИТА для активн. перем. (2-3 теста) | Доступные выраж.-множества e_genB, e_killB. Передаточная ф-ия ББЛ в fB = e_genB U (x - e_killB)|-|-|
|Kt|**Дядичко, Кузнецов**|2, 12| 5) Логические тождества| - | GenB/KillB. Вычислить Fb по явным формулам | - | - | Опр-р сбора /\ и отображение m в задаче о распростр. const|-|
|M&M|**Атоян, Сидоренко**|5, 14| 7) Очистка от пустых опер-ов, устранение переходов через переходы | - | - | Вычисление множеств def и use для активн.перем. |Провести оптимизации на основе анализа доступн.выраж (3 теста)|-|-|
|Null|**Таранова, Швецов**|3, 13| 6) Оптимизация общих подвыражений | Разбиение CFG | - | ИТА для достигающих определений |-|-|-|
|-|**Корниенко, Лимарев**|-, -| - | - | - | - | - |-|-|

## Задачи, которые пока не разобрали команды:
- Передаточная ф-ия в задаче о распростр. const
- ИТА в задаче распростр. const (4 теста)


## Деятельность команд:
- M&M (Атоян, Сидоренко)
  - Ответственные за парсер
- AW (Галайчук, Рязанова)
  - Ответственные за генерацию трехадресного кода
- Галайчук, Рязанова, Голубев
  - Ответственные за интегрирующую программу
- Ульянов
  - Ответственный за документацию
- Тян
  - Ответственный за тесты
