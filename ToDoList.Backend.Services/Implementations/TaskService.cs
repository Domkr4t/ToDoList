using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using ToDoList.Backend.DAL.Interfaces;
using ToDoList.Backend.Domain.Entity;
using ToDoList.Backend.Domain.Enum;
using ToDoList.Backend.Domain.Response;
using ToDoList.Backend.Domain.ViewModel.Task;
using ToDoList.Backend.Services.Interfaces;

namespace ToDoList.Backend.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IBaseRepository<TaskEntity> _taskRepository;
        private ILogger<TaskService> _logger;

        public TaskService(IBaseRepository<TaskEntity> taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<IBaseResponse<CreateTaskViewModel>> CreateTask(CreateTaskViewModel model)
        {
            try
            {
                model.Validate();

                _logger.LogInformation($"Запрос на создание задачи - {model.Name}");

                var task = await _taskRepository.GetAll().
                    Where(x => x.CreatedAt.Date == DateTime.Today).
                    FirstOrDefaultAsync(x => x.Name == model.Name);

                if (task != null)
                {
                    return new BaseResponse<CreateTaskViewModel>
                    {
                        Description = $"Задача с именем \"{model.Name}\" уже создана сегодня",
                        StatusCode = StatusCode.TaskIsHasAlready
                    };
                }

                task = new TaskEntity()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Priority = model.Priority,
                    CreatedAt = DateTime.Now,
                    IsDone = false
                };

                await _taskRepository.Create(task);

                return new BaseResponse<CreateTaskViewModel>
                {
                    Description = "Задача создана",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService:CreateTask] : {ex.Message}");

                return new BaseResponse<CreateTaskViewModel>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
