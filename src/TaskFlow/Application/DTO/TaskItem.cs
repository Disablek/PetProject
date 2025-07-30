using Domain.Entities;

namespace Application.DTO
{
    // DTO для создания новой задачи
    public class CreateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }

    // DTO для обновления задачи
    public class UpdateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Status { get; set; } = "Pending";
    }

    // DTO для отображения задачи (ответ API)
    public class TaskItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Status { get; set; } = "Pending";

        // Конструктор для маппинга из доменной модели
        public static TaskItemDto FromEntity(TaskItem entity)
        {
            return new TaskItemDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                IsCompleted = entity.IsCompleted,
                CreatedAt = entity.CreatedAt,
                DueDate = entity.DueDate,
                CompletedAt = entity.CompletedAt,
                Status = entity.Status
            };
        }
    }

    // DTO для списка задач (упрощенная версия для списков)
    public class TaskItemListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = "Pending";

        public static TaskItemListDto FromEntity(TaskItem entity)
        {
            return new TaskItemListDto
            {
                Id = entity.Id,
                Title = entity.Title,
                IsCompleted = entity.IsCompleted,
                DueDate = entity.DueDate,
                Status = entity.Status
            };
        }
    }
} 