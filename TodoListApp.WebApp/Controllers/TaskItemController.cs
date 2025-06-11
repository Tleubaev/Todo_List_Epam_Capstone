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
        private readonly ITagService _tagService;
        private readonly Guid _currentUserId;

        public TaskItemController(
            ITaskItemService taskItemService,
            IHttpContextAccessor accessor,
            ITagService tagService
        )
        {
            _taskItemService = taskItemService;
            _tagService = tagService;
            _currentUserId = Guid.Parse(accessor.HttpContext!.User.FindFirst("sub")!.Value);
        }

        public async Task<IActionResult> Index(Guid todoListId)
        {
            var tasks = await _taskItemService.GetByTodoListIdAsync(todoListId);
            ViewBag.TodoListId = todoListId;
            ViewBag.Now = DateTime.UtcNow;
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
                model.AssignedUserId = _currentUserId;
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
            ViewBag.AllTags = await _tagService.GetAllAsync();
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

        [HttpPost]
        public async Task<IActionResult> AddTag(Guid taskId, Guid tagId, Guid todoListId)
        {
            await _taskItemService.AddTagAsync(taskId, tagId);
            return RedirectToAction("Edit", new { id = taskId, todoListId });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveTag(Guid taskId, Guid tagId, Guid todoListId)
        {
            await _taskItemService.RemoveTagAsync(taskId, tagId);
            return RedirectToAction("Edit", new { id = taskId, todoListId });
        }
    }
}
