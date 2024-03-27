using ToDoList.Backend.Domain.Response;
using ToDoList.Backend.Domain.ViewModel.Task;

namespace ToDoList.Backend.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IBaseResponse<CreateTaskViewModel>> CreateTask (CreateTaskViewModel model);

        Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetAllTasks ();
    }
}
