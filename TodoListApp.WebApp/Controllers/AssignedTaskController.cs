using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Controllers
{
    [Authorize]
    public class AssignedTaskController : Controller
    {
        private readonly ITaskItemService _taskService;
        private readonly Guid _currentUserId;

        public AssignedTaskController(ITaskItemService taskService, IHttpContextAccessor accessor)
        {
            _taskService = taskService;
            _currentUserId = Guid.Parse(accessor.HttpContext!.User.FindFirst("sub")!.Value);
        }

        public async Task<IActionResult> Index(bool? isCompleted = null, string? sortBy = null, bool ascending = true)
        {
            var tasks = await _taskService.GetAssignedToUserAsync(_currentUserId, isCompleted, sortBy, ascending);
            ViewBag.Filter = isCompleted;
            ViewBag.SortBy = sortBy;
            ViewBag.Ascending = ascending;
            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(Guid id, bool isCompleted)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null || task.AssignedUserId != _currentUserId)
            {
                return NotFound();
            }

            task.IsCompleted = isCompleted;
            await _taskService.UpdateAsync(task);
            return RedirectToAction(nameof(Index));
        }
    }
}
