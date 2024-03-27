using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ToDoList.Backend.Domain.Enum;

namespace ToDoList.Backend.Domain.Entity
{
    public class TaskEntity
    {
        [Comment("Task ID")]
        public long Id { get; set; }

        [Comment("Task name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Comment("Task description")]
        public string? Description { get; set; }

        [Comment("Task priority")]
        public Priority Priority { get; set; }

        [Comment("Task creation time")]
        public DateTime CreatedAt { get; set; }

        [Comment("Has the task been completed")]
        public bool IsDone {  get; set; }

    }
}
