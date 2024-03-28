using Microsoft.AspNetCore.Mvc;
using ToDoList.Backend.Domain.Filters.Task;
using ToDoList.Backend.Domain.ViewModel.Task;
using ToDoList.Backend.Services.Interfaces;

namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AllTasks()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskViewModel model)
        {
            var response = await _taskService.CreateTask(model);

            if (response.StatusCode == Backend.Domain.Enum.StatusCode.Ok)
            {
                return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }

        [HttpPost]
        public async Task<IActionResult> GetAllTasksToday()
        {
            var response = await _taskService.GetAllTasksToday();

            return Json(new { data = response.Data });
        }

        [Route("/AllTasks")]
        [HttpPost]
        public async Task<IActionResult> GetAllTasks(TaskFilter filter)
        {
            var response = await _taskService.GetAllTasks(filter);

            return Json(new { data = response.Data });
        }
    }
}