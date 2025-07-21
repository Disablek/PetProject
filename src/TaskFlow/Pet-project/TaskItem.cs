using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pet_project
{
    public class TaskItem 
    {
        public Guid Id { get; }
        public string Title { get; set; }
        public string? Description { get; set; } = default!;
        public bool IsCompleted { get; set; }

        public TaskItem(string title, string description = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
        }

        public void MarkCompleted()
        {
            IsCompleted = true;
        }
        public override string ToString() =>
            $"{(IsCompleted ? "[x]" : "[ ]")} {Title} (Id: {Id})";
    }
}
