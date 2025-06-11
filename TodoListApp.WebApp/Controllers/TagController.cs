using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;

namespace TodoListApp.WebApp.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly ITaskItemService _taskService;

        public TagController(ITagService tagService, ITaskItemService taskService)
        {
            _tagService = tagService;
            _taskService = taskService;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllAsync();
            return View(tags);
        }

        public async Task<IActionResult> Tasks(Guid id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoListApp.WebApi.Models.Tag model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _tagService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
