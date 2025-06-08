using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Controllers
{
    [Authorize]
    public class ToDoListController : Controller
    {
        private readonly IToDoListService _toDoListService;
        private readonly Guid _currentUserId;

        public ToDoListController(IToDoListService toDoListService, IHttpContextAccessor accessor)
        {
            _toDoListService = toDoListService;
            _currentUserId = Guid.Parse(accessor.HttpContext!.User.FindFirst("sub")!.Value);
        }

        public async Task<IActionResult> Index()
        {
            var lists = await _toDoListService.GetByUserIdAsync(_currentUserId);
            return View(lists);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ToDoList model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = _currentUserId;
                await _toDoListService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var list = await _toDoListService.GetByIdAsync(id);
            if (list == null || list.UserId != _currentUserId)
            {
                return NotFound();
            }
            return View(list);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var list = await _toDoListService.GetByIdAsync(id);
            if (list == null || list.UserId != _currentUserId)
            {
                return NotFound();
            }
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ToDoList model)
        {
            if (ModelState.IsValid && model.UserId == _currentUserId)
            {
                await _toDoListService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var list = await _toDoListService.GetByIdAsync(id);
            if (list == null || list.UserId != _currentUserId)
            {
                return NotFound();
            }
            return View(list);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _toDoListService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
