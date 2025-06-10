using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ITaskItemService _taskService;

        public SearchController(ITaskItemService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IActionResult> Index(string? title, DateTime? createdFrom, DateTime? createdTo, DateTime? dueFrom, DateTime? dueTo)
        {
            var tasks = await _taskService.SearchAsync(title, createdFrom, createdTo, dueFrom, dueTo);
            ViewBag.Title = title;
            ViewBag.CreatedFrom = createdFrom;
            ViewBag.CreatedTo = createdTo;
            ViewBag.DueFrom = dueFrom;
            ViewBag.DueTo = dueTo;
            ViewBag.Now = DateTime.UtcNow;
            return View(tasks);
        }
    }
}
