using ToDoList.Backend.Domain.Filters.Task;
using ToDoList.Backend.Domain.Response;
using ToDoList.Backend.Domain.ViewModel.Task;

namespace ToDoList.Backend.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IBaseResponse<CreateTaskViewModel>> CreateTask (CreateTaskViewModel model);

        Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetAllTasksToday();

        Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetAllTasks(TaskFilter filter);

        Task<IBaseResponse<bool>> EndTask(long id);

        Task<IBaseResponse<bool>> DeleteTask(long id);
    }
}
