# WriteAndErase

Это приложение представляет собой торговую систему, разработанную на C# с использованием Avalonia. Оно позволяет пользователям аутентифицироваться, просматривать товары, формировать заказы и управлять ими в зависимости от роли пользователя.

## Оглавление

- [Функциональные возможности](#функциональные-возможности)
- [Установка](#установка)
- [Использование](#использование)
- [Структура проекта](#структура-проекта)
- [Требования](#требования)
- [Лицензия](#лицензия)

## Функциональные возможности

1. **Аутентификация пользователей**:
   - Вход с логином и паролем.
   - Возможность входа в качестве гостя.
   - Защита от неуспешной авторизации с использованием CAPTCHA.

2. **Управление пользователями**:
   - Разные уровни доступа для клиентов, менеджеров и администраторов.
   - Отображение ФИО пользователя в интерфейсе после входа.

3. **Просмотр товаров**:
   - Вывод списка товаров из базы данных.
   - Подсветка товаров в зависимости от размера скидки.
   - Возможность сортировки, фильтрации и поиска товаров в реальном времени.
   - Отображение количества товаров и общего количества записей.

4. **Формирование заказов**:
   - Возможность добавления товаров в заказ через контекстное меню.
   - Отображение информации о заказе в отдельном модальном окне.
   - Возможность удаления товаров из заказа.

## Установка

1. Убедитесь, что у вас установлен [.NET SDK](https://dotnet.microsoft.com/download) версии 6.0 или выше.
2. Склонируйте репозиторий:
   ```bash
   git clone https://github.com/nljubivaya/up_2
3. **Запустите приложение:**
   

   ## Использование

   При запуске приложения появится окно входа. Введите свои учетные данные или выберите вход в качестве гостя. После успешной аутентификации вы получите доступ к функционалу, зависящему от вашей роли.

   Вы сможете:

   * Просматривать товары
   * Добавлять товары в заказ
   * Управлять своими заказами


   ## Структура проекта

   * **Models/:** Модели данных приложения.
   * **Views/:** Представления (интерфейсы) для различных экранов.
   * **ViewModels/:** Логика представлений, связывающая модели и представления.
   * **Resources/:** Ресурсы, такие как изображения и стили.
   * **Services/:** Сервисы для работы с данными и бизнес-логикой.

# Сессия 2: Разработка библиотеки для формирования оптимального графика работы сотрудников

## Описание проекта

В рамках данного проекта была разработана библиотека, позволяющая возвращать список свободных временных интервалов заданного размера в графике сотрудника. Это необходимо для формирования оптимального графика работы сотрудников, учитывая занятые промежутки времени.

## Функциональность библиотеки

Библиотека предоставляет метод, который принимает на вход список занятых промежутков времени и возвращает список свободных временных интервалов, подходящих для назначения консультаций. Метод учитывает минимальное необходимое время для работы менеджера и рабочие часы сотрудника.

### Входные параметры

- `TimeSpan[] startTimes` - массив, содержащий время начала занятых промежутков.
- `int[] durations` - массив, содержащий длительности занятых промежутков.
- `TimeSpan beginWorkingTime` - время начала рабочего дня сотрудника.
- `TimeSpan endWorkingTime` - время завершения рабочего дня сотрудника.
- `int consultationTime` - минимальное необходимое время для консультации.

### Выходные параметры

- `string[]` - массив строк, представляющий свободные временные промежутки в формате `HH:mm-HH:mm`.

## Пример использования

### Входные данные

| startTime | duration |
|-----------|----------|
| 10:00     | 60       |
| 11:00     | 30       |
| 15:00     | 10       |
| 15:30     | 10       |
| 16:50     | 40       |

**Рабочие часы**: 08:00-18:00  
**Время консультации**: 30 минут

### Выходные данные

08:00-08:30
08:30-09:00
09:00-09:30
09:30-10:00
11:30-12:00
12:00-12:30
12:30-13:00
13:00-13:30
13:30-14:00
14:00-14:30
14:30-15:00
15:40-16:10
16:10-16:40
17:30-18:00


## Требования к именованию

Для обеспечения корректной работы библиотеки необходимо следовать правилам именования:

- **Название библиотеки**: `SF2022User {NN}Lib.dll` 
- **Название класса**: `Calculations`.
- **Название метода**: `AvailablePeriods()`.
- **Входящие обязательные параметры**: 
  - Для C#: `TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime`.
- **Возвращаемые параметры**: `string[]`.

## Модульное тестирование

Для обеспечения корректности работы библиотеки были реализованы 10 unit-тестов на основе технологии TDD. Тестовые данные учитывают различные ситуации, такие как недостаточное время в промежутках между консультациями, а также различные длительности консультаций.

## Документация по тестированию

Для выполнения процедуры тестирования были описаны пять сценариев, связанных с добавлением товара администратором. Сценарии демонстрируют различные исходы работы алгоритма, что позволяет проверить его надежность и корректность.

   ## Требования

   * .NET SDK версии 6.0 или выше.
   * Avalonia UI.


   
