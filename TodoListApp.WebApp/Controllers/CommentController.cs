using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApp.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly Guid _currentUserId;

        public CommentController(ICommentService commentService, IHttpContextAccessor accessor)
        {
            _commentService = commentService;
            _currentUserId = Guid.Parse(accessor.HttpContext!.User.FindFirst("sub")!.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid taskItemId, string content, Guid todoListId)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                TaskItemId = taskItemId,
                Content = content,
                UserId = _currentUserId,
                CreatedAt = DateTime.UtcNow
            };
            await _commentService.CreateAsync(comment);
            return RedirectToAction("Details", "TaskItem", new { id = taskItemId, todoListId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, Guid taskItemId, Guid todoListId)
        {
            await _commentService.DeleteAsync(id);
            return RedirectToAction("Details", "TaskItem", new { id = taskItemId, todoListId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, Guid taskItemId, Guid todoListId)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null || comment.UserId != _currentUserId)
            {
                return Forbid();
            }
            ViewBag.TaskItemId = taskItemId;
            ViewBag.TodoListId = todoListId;
            return View(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Comment model, Guid taskItemId, Guid todoListId)
        {
            // Можно добавить проверку принадлежности
            model.UserId = _currentUserId;
            await _commentService.UpdateAsync(model);
            return RedirectToAction("Details", "TaskItem", new { id = taskItemId, todoListId });
        }
    }
}
