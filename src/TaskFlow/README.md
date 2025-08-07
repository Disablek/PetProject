# TaskFlow - Система управления задачами

## Архитектура проекта

Проект построен по принципам Clean Architecture с разделением на слои:

```
TaskFlow/
├── Domain/                 # Доменный слой
│   ├── Entities/          # Доменные сущности
│   └── Interfaces/        # Интерфейсы репозиториев
├── Application/           # Слой приложения (Business Logic)
│   ├── DTO/              # Data Transfer Objects
│   ├── Services/         # Бизнес-сервисы
│   └── Examples/         # Примеры использования
├── Infrastructure/       # Слой инфраструктуры (Data Access)
│   ├── Entities/         # Entity Framework сущности
│   ├── Configurations/   # Конфигурации EF
│   └── Repositories/     # Реализации репозиториев
└── API/                  # Presentation слой
    └── Controllers/      # API контроллеры
```

## Зависимости между слоями

```
API → Application → Domain ← Infrastructure
```

- **API** зависит от **Application** (использует сервисы)
- **Application** зависит от **Domain** (использует интерфейсы и сущности)
- **Infrastructure** зависит от **Domain** (реализует интерфейсы)
- **Domain** не зависит ни от кого (содержит только бизнес-логику)

## Основные сущности

### TaskItem (Domain)
- Основная доменная сущность задачи
- Содержит бизнес-логику и правила
- Не зависит от внешних слоев

### TaskEntity (Infrastructure)
- Entity Framework сущность для работы с БД
- Соответствует структуре таблицы в базе данных
- Содержит навигационные свойства для связей

### DTO (Application)
- Объекты для передачи данных между слоями
- TaskItemDto - полная информация о задаче
- TaskItemListDto - краткая информация для списков
- CreateTaskItemDto - данные для создания
- UpdateTaskItemDto - данные для обновления

## Связи между сущностями

### Пользователь ↔ Задача
- **Создатель задачи**: Один пользователь может создать много задач
- **Исполнитель задачи**: Один пользователь может быть исполнителем многих задач

### Проект ↔ Задача
- **Проект → Задачи**: Один проект содержит много задач

### Проект ↔ Пользователь
- **Участники проекта**: Many-to-many связь через таблицу UserProjects
- **Администратор проекта**: One-to-one связь

## Статусы задач

```csharp
public enum Status
{
    New = 0,        // Новая
    InProgress = 1, // В работе
    Review = 2,     // На проверке
    Done = 3,       // Завершена
    Blocked = 4     // Заблокирована
}
```

## Приоритеты задач

```csharp
public enum Priority
{
    Low = 0,      // Низкий
    Medium = 1,   // Средний
    High = 2,     // Высокий
    Critical = 3  // Критический
}
```

## Основные возможности

### Создание задач
```csharp
var createDto = new CreateTaskItemDto
{
    Title = "Новая задача",
    Description = "Описание задачи",
    Priority = Priority.High,
    DueTime = DateTime.Now.AddDays(7),
    ProjectId = projectId,
    CreatorId = creatorId,
    AssigneeId = assigneeId
};

var task = taskService.CreateTask(createDto);
```

### Управление статусами
```csharp
// Завершить задачу
var completedTask = taskService.MarkTaskCompleted(taskId);

// Перевести в работу
var inProgressTask = taskService.MarkTaskInProgress(taskId);

// Перевести на проверку
var reviewTask = taskService.MarkTaskReview(taskId);

// Заблокировать
var blockedTask = taskService.MarkTaskBlocked(taskId);
```

### Получение задач по критериям
```csharp
// Все задачи проекта
var projectTasks = taskService.GetTasksByProject(projectId);

// Задачи пользователя (созданные + назначенные)
var userTasks = taskService.GetUserTasks(userId);

// Просроченные задачи
var overdueTasks = taskService.GetOverdueTasks();

// Задачи по статусу
var inProgressTasks = taskService.GetTasksByStatus(Status.InProgress);
```

### Переназначение задач
```csharp
var reassignedTask = taskService.ReassignTask(taskId, newAssigneeId);
```

## Конфигурация Entity Framework

### TaskConfiguration
```csharp
// Связь с создателем задачи
builder
    .HasOne(u => u.Creator)
    .WithMany(t => t.CreatedTasks)
    .HasForeignKey(t => t.CreatorId)
    .OnDelete(DeleteBehavior.Restrict);

// Связь с исполнителем задачи
builder
    .HasOne(u => u.Assignee)
    .WithMany(t => t.AssignedTasks)
    .HasForeignKey(t => t.AssigneeId)
    .OnDelete(DeleteBehavior.SetNull);
```

## Преимущества архитектуры

1. **Разделение ответственности**: Каждый слой отвечает за свою область
2. **Тестируемость**: Легко тестировать бизнес-логику изолированно
3. **Гибкость**: Можно легко заменить реализацию репозитория
4. **Масштабируемость**: Структура поддерживает рост приложения
5. **Чистота кода**: Четкие границы между слоями

## Запуск проекта

1. Восстановить зависимости:
```bash
dotnet restore
```

2. Собрать проект:
```bash
dotnet build
```

3. Запустить API:
```bash
cd API
dotnet run
```

## Структура базы данных

### Таблицы
- **Tasks** - задачи
- **Users** - пользователи
- **Projects** - проекты
- **UserProjects** - связь пользователей с проектами

### Связи
- Tasks.ProjectId → Projects.Id
- Tasks.CreatorId → Users.Id
- Tasks.AssigneeId → Users.Id
- Projects.AdminId → Users.Id
- UserProjects (many-to-many между Users и Projects)

## Дальнейшее развитие

1. **Аутентификация и авторизация**
2. **Уведомления о задачах**
3. **Комментарии к задачам**
4. **Вложения к задачам**
5. **Отчеты и аналитика**
6. **Интеграция с внешними системами** 