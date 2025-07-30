using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTO
{
    // DTO для списка задач (Меньше полей > производительность)
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
