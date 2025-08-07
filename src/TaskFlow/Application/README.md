# TaskFlow Application Layer (Business Logic)

## Обзор

Application слой содержит бизнес-логику приложения и является мостом между Presentation (API) и Infrastructure (Data) слоями.

## Структура

### DTO (Data Transfer Objects)
- **TaskItemDto** - полная информация о задаче
- **TaskItemListDto** - краткая информация о задаче для списков
- **CreateTaskItemDto** - данные для создания задачи
- **UpdateTaskItemDto** - данные для обновления задачи

### Services
- **TaskService** - основной сервис для работы с задачами

### Interfaces
- **ITaskRepository** - интерфейс репозитория для работы с задачами

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

### Обновление задач
```csharp
var updateDto = new UpdateTaskItemDto
{
    Title = "Обновленная задача",
    Description = "Новое описание",
    Status = Status.InProgress,
    Priority = Priority.Medium,
    DueTime = DateTime.Now.AddDays(5),
    AssigneeId = newAssigneeId
};

var updatedTask = taskService.UpdateTask(taskId, updateDto);
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

### Переназначение задач
```csharp
var reassignedTask = taskService.ReassignTask(taskId, newAssigneeId);
```

### Получение задач по критериям
```csharp
// Все задачи проекта
var projectTasks = taskService.GetTasksByProject(projectId);

// Задачи создателя
var createdTasks = taskService.GetTasksByCreator(creatorId);

// Задачи исполнителя
var assignedTasks = taskService.GetTasksByAssignee(assigneeId);

// Задачи по статусу
var inProgressTasks = taskService.GetTasksByStatus(Status.InProgress);

// Просроченные задачи
var overdueTasks = taskService.GetOverdueTasks();

// Все задачи пользователя (созданные + назначенные)
var userTasks = taskService.GetUserTasks(userId);
```

## Архитектурные принципы

### 1. Dependency Inversion
- Application слой зависит от абстракций (интерфейсов), а не от конкретных реализаций
- Интерфейс `ITaskRepository` определяет контракт для работы с данными

### 2. Single Responsibility
- Каждый сервис отвечает за одну область бизнес-логики
- DTO содержат только данные, необходимые для передачи

### 3. Open/Closed Principle
- Легко добавлять новые методы в сервисы без изменения существующего кода
- Новые DTO можно создавать без изменения существующих

### 4. Interface Segregation
- Интерфейсы содержат только необходимые методы
- Разделение на разные DTO для разных сценариев использования

## Маппинг данных

### Domain → DTO
```csharp
// Из Domain Entity
var dto = TaskItemDto.FromEntity(domainTask);

// Из Infrastructure Entity
var dto = TaskItemDto.FromInfrastructureEntity(infrastructureTask);
```

### DTO → Domain
```csharp
var task = new TaskItem(
    createDto.Title,
    createDto.Description,
    createDto.CreatorId,
    createDto.AssigneeId,
    createDto.ProjectId,
    createDto.DueTime
);
```

## Обработка ошибок

### Проверки валидности
```csharp
public TaskItemDto? UpdateTask(Guid id, UpdateTaskItemDto updateDto)
{
    var existingTask = _taskRepository.GetById(id) as TaskItem;
    if (existingTask == null)
        return null; // Задача не найдена

    // Обновление логики...
    var success = _taskRepository.Update(existingTask);
    return success ? TaskItemDto.FromEntity(existingTask) : null;
}
```

### Бизнес-правила
- Задачи могут быть созданы только участниками проекта
- Исполнитель должен быть участником проекта
- Статус задачи может изменяться только в определенном порядке
- Просроченные задачи должны быть помечены соответствующим образом

## Расширение функциональности

### Добавление новых методов
1. Добавить метод в интерфейс `ITaskRepository`
2. Реализовать метод в `InMemoryTaskRepository`
3. Добавить метод в `TaskService`
4. Создать соответствующий DTO если необходимо

### Добавление новых DTO
```csharp
public class TaskStatisticsDto
{
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int OverdueTasks { get; set; }
    public double CompletionRate { get; set; }
}
```

## Тестирование

### Unit тесты
- Тестирование бизнес-логики в `TaskService`
- Тестирование маппинга в DTO
- Тестирование валидации данных

### Integration тесты
- Тестирование взаимодействия с репозиторием
- Тестирование полного цикла операций

## Производительность

### Оптимизации
- Использование `TaskItemListDto` для списков (меньше данных)
- Ленивая загрузка связанных данных
- Кэширование часто используемых данных

### Мониторинг
- Логирование операций
- Метрики производительности
- Отслеживание времени выполнения операций 