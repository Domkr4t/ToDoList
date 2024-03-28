using Microsoft.EntityFrameworkCore;
using ToDoList.Backend.Domain.Enum;

namespace ToDoList.Backend.Domain.Filters.Task
{
    public class TaskFilter
    {
        public string? Name { get; set; }
        public Priority? Priority { get; set; }
        public bool? IsDone { get; set; }
    }
}
