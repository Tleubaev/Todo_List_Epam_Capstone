using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Controllers
{
    [Authorize]
    public class TaskItemController : Controller
    {
        private readonly ITaskItemService _taskItemService;

        public TaskItemController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        public async Task<IActionResult> Index(Guid todoListId)
        {
            var tasks = await _taskItemService.GetByTodoListIdAsync(todoListId);
            ViewBag.TodoListId = todoListId;
            return View(tasks);
        }

        public IActionResult Create(Guid todoListId)
        {
            ViewBag.TodoListId = todoListId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem model, Guid todoListId)
        {
            if (ModelState.IsValid)
            {
                model.TodoListId = todoListId;
                await _taskItemService.CreateAsync(model);
                return RedirectToAction("Index", new { todoListId });
            }
            ViewBag.TodoListId = todoListId;
            return View(model);
        }

        public async Task<IActionResult> Details(Guid id, Guid todoListId)
        {
            var task = await _taskItemService.GetByIdAsync(id);
            if (task == null || task.TodoListId != todoListId)
            {
                return NotFound();
            }
            ViewBag.TodoListId = todoListId;
            return View(task);
        }

        public async Task<IActionResult> Edit(Guid id, Guid todoListId)
        {
            var task = await _taskItemService.GetByIdAsync(id);
            if (task == null || task.TodoListId != todoListId)
            {
                return NotFound();
            }
            ViewBag.TodoListId = todoListId;
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskItem model, Guid todoListId)
        {
            if (ModelState.IsValid && model.TodoListId == todoListId)
            {
                await _taskItemService.UpdateAsync(model);
                return RedirectToAction("Index", new { todoListId });
            }
            ViewBag.TodoListId = todoListId;
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id, Guid todoListId)
        {
            var task = await _taskItemService.GetByIdAsync(id);
            if (task == null || task.TodoListId != todoListId)
            {
                return NotFound();
            }
            ViewBag.TodoListId = todoListId;
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid todoListId)
        {
            await _taskItemService.DeleteAsync(id);
            return RedirectToAction("Index", new { todoListId });
        }
    }
}
