using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UpdateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
