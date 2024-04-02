using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoList.Backend.DAL.Interfaces;
using ToDoList.Backend.Domain.Entity;
using ToDoList.Backend.Domain.Enum;
using ToDoList.Backend.Domain.Response;
using ToDoList.Backend.Domain.Extentions;
using ToDoList.Backend.Domain.ViewModel.Task;
using ToDoList.Backend.Services.Interfaces;
using ToDoList.Backend.Domain.Filters.Task;
using System.Security.Principal;

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

        public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetAllTasksToday()
        {
            try
            {
                var tasks = await _taskRepository.GetAll()
                    .Where(x => x.CreatedAt.Date == DateTime.Today)
                    .Where(x => x.IsDone == false)
                    .Select(x => new TaskViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Priority = x.Priority.GetDisplayName(),
                        CreatedAt = x.CreatedAt.Date.ToLongDateString(),
                        IsDone = x.IsDone == true ? "Выполнена" : "Не выполнена"
                    }).ToListAsync();

                return new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    Data = tasks,
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService:GetAllTasks] : {ex.Message}");

                return new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetAllTasks(TaskFilter filter)
        {
            try
            {
                var tasks = await _taskRepository.GetAll()
                    .WhereIf(filter.Priority.HasValue, x => x.Priority == filter.Priority)
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name == filter.Name)
                    .WhereIf(filter.IsDone.HasValue, x => x.IsDone == filter.IsDone)
                    .Select(x => new TaskViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Priority = x.Priority.GetDisplayName(),
                        CreatedAt = x.CreatedAt.Date.ToLongDateString(),
                        IsDone = x.IsDone == true ? "Выполнена" : "Не выполнена"
                    }).ToListAsync();

                return new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    Data = tasks,
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService:GetAllTasks] : {ex.Message}");

                return new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> EndTask(long id)
        {
            try
            {
                var task = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (task == null)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Такой задачи не существует",
                        StatusCode = StatusCode.TaskIsNotFound
                    };
                }

                task.IsDone = true;

                await _taskRepository.Update(task);

                return new BaseResponse<bool>
                {
                    Description = $"Задача {task.Name} выполнена",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService:EndTask] : {ex.Message}");

                return new BaseResponse<bool>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteTask(long id)
        {
            try
            {
                var task = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (task == null)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Такой задачи не существует",
                        StatusCode = StatusCode.TaskIsNotFound
                    };
                }

                await _taskRepository.Delete(task);

                return new BaseResponse<bool>
                {
                    Description = $"Задача {task.Name} удалена",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService:DeleteTask] : {ex.Message}");

                return new BaseResponse<bool>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetExecutedTasks()
        {
            try
            {
                var tasks = await _taskRepository.GetAll()
                    .Where(x => x.CreatedAt.Date == DateTime.Today)
                    .Where(x => x.IsDone == true)
                    .Select(x => new TaskViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Priority = x.Priority.GetDisplayName(),
                        CreatedAt = x.CreatedAt.Date.ToLongDateString(),
                        IsDone = x.IsDone == true ? "Выполнена" : "Не выполнена"
                    }).ToListAsync();

                return new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    Data = tasks,
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService:GetAllTasks] : {ex.Message}");

                return new BaseResponse<IEnumerable<TaskViewModel>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
