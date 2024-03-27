using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ToDoList.Backend.Domain.Enum;

namespace ToDoList.Backend.Domain.ViewModel.Task
{
    public class CreateTaskViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDone { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) 
            { 
                throw new ArgumentNullException(Name, "Укажите название задачи");
            }
        }
    }
}
