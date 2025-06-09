using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ITodoListService _todoListService;
        private readonly Guid _currentUserId;

        public TodoListController(ITodoListService todoListService, IHttpContextAccessor accessor)
        {
            _todoListService = todoListService;
            _currentUserId = Guid.Parse(accessor.HttpContext!.User.FindFirst("sub")!.Value);
        }

        public async Task<IActionResult> Index()
        {
            var lists = await _todoListService.GetByUserIdAsync(_currentUserId);
            return View(lists);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(TodoList model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = _currentUserId;
                await _todoListService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var list = await _todoListService.GetByIdAsync(id);
            if (list == null || list.UserId != _currentUserId)
            {
                return NotFound();
            }
            return View(list);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var list = await _todoListService.GetByIdAsync(id);
            if (list == null || list.UserId != _currentUserId)
            {
                return NotFound();
            }
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TodoList model)
        {
            if (ModelState.IsValid && model.UserId == _currentUserId)
            {
                await _todoListService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var list = await _todoListService.GetByIdAsync(id);
            if (list == null || list.UserId != _currentUserId)
            {
                return NotFound();
            }
            return View(list);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _todoListService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
