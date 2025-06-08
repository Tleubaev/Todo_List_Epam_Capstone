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

        public async Task<IActionResult> Index(Guid toDoListId)
        {
            var tasks = await _taskItemService.GetByToDoListIdAsync(toDoListId);
            ViewBag.ToDoListId = toDoListId;
            return View(tasks);
        }

        public IActionResult Create(Guid toDoListId)
        {
            ViewBag.ToDoListId = toDoListId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem model, Guid toDoListId)
        {
            if (ModelState.IsValid)
            {
                model.ToDoListId = toDoListId;
                await _taskItemService.CreateAsync(model);
                return RedirectToAction("Index", new { toDoListId });
            }
            ViewBag.ToDoListId = toDoListId;
            return View(model);
        }

        public async Task<IActionResult> Details(Guid id, Guid toDoListId)
        {
            var task = await _taskItemService.GetByIdAsync(id);
            if (task == null || task.ToDoListId != toDoListId)
            {
                return NotFound();
            }
            ViewBag.ToDoListId = toDoListId;
            return View(task);
        }

        public async Task<IActionResult> Edit(Guid id, Guid toDoListId)
        {
            var task = await _taskItemService.GetByIdAsync(id);
            if (task == null || task.ToDoListId != toDoListId) return NotFound();
            ViewBag.ToDoListId = toDoListId;
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskItem model, Guid toDoListId)
        {
            if (ModelState.IsValid && model.ToDoListId == toDoListId)
            {
                await _taskItemService.UpdateAsync(model);
                return RedirectToAction("Index", new { toDoListId });
            }
            ViewBag.ToDoListId = toDoListId;
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id, Guid toDoListId)
        {
            var task = await _taskItemService.GetByIdAsync(id);
            if (task == null || task.ToDoListId != toDoListId)
            {
                return NotFound();
            }
            ViewBag.ToDoListId = toDoListId;
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid toDoListId)
        {
            await _taskItemService.DeleteAsync(id);
            return RedirectToAction("Index", new { toDoListId });
        }
    }
}
