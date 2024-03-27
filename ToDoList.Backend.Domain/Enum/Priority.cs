using System.ComponentModel.DataAnnotations;

namespace ToDoList.Backend.Domain.Enum
{
    public enum Priority
    {
        [Display (Name = "Низкая приоритетность")]
        Low = 0,
        [Display(Name = "Средняя приоритетность")]
        Medium = 1,
        [Display(Name = "Высокая приоритетность")]
        High = 2,
    }
}
