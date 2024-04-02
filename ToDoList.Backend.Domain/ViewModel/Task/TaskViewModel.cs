using ToDoList.Backend.Domain.Enum;

namespace ToDoList.Backend.Domain.ViewModel.Task
{
    public class TaskViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string CreatedAt { get; set; }
        public string IsDone { get; set; }
    }
}
