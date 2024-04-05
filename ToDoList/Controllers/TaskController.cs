using Microsoft.AspNetCore.Mvc;
using ToDoList.Backend.Domain.Filters.Task;
using ToDoList.Backend.Domain.Response;
using ToDoList.Backend.Domain.Utils;
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

        [HttpPost]
        public async Task<IActionResult> GetAllTasks(TaskFilter filter)
        {
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();

            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;

            filter.PageSize = pageSize;
            filter.Skip = skip;

            var response = await _taskService.GetAllTasks(filter);

            return Json(new { recordsTotal = response.Total, data = response.Data });
        }

        [HttpPost]
        public async Task<IActionResult> GetExecutedTasks()
        {
            var response = await _taskService.GetExecutedTasks();

            return Json(new { data = response.Data });
        }

        [HttpPost]
        public async Task<IActionResult> EndTask(long id)
        {
            var response = await _taskService.EndTask(id);

            if (response.StatusCode == Backend.Domain.Enum.StatusCode.Ok)
            {
                return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(long id)
        {
            var response = await _taskService.DeleteTask(id);

            if (response.StatusCode == Backend.Domain.Enum.StatusCode.Ok)
            {
                return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }

        [HttpPost]
        public async Task<IActionResult> EndOfDayReport()
        {
            var response = await _taskService.EndOfDayReport();

            if (response.StatusCode == Backend.Domain.Enum.StatusCode.Ok)
            {
                var csvService = new CsvBaseService<IEnumerable<TaskViewModel>>();
                var downloadFile = csvService.DownloadFile(response.Data);

                return File(downloadFile, "txt/csv", $"Отчет задач за день {DateTime.Now.ToLongDateString()}csv");
            }
            return BadRequest(new { description = response.Description });
        }
    }
}