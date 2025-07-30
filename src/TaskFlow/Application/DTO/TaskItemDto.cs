using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTO
{
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

        // Маппинг из Domain
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
}
