using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTO
{
    public class CreateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
