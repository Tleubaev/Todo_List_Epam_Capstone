using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TodoTaskController : ControllerBase
    {
        private readonly ITaskItemService _service;

        public TodoTaskController(ITaskItemService service)
        {
            _service = service;
        }

        // GET: api/tasks/todolist/{todoListId}
        [HttpGet("todolist/{todoListId}")]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasksByTodoListId(Guid todoListId)
        {
            var tasks = await _service.GetByTodoListIdAsync(todoListId);
            return Ok(tasks.Select(TaskItemDto.FromEntity));
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDto>> GetById(Guid id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(TaskItemDto.FromEntity(task));
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> Create([FromBody] TaskItem model)
        {
            if (model.DueDate.HasValue && model.DueDate.Value.Kind == DateTimeKind.Unspecified)
            {
                model.DueDate = DateTime.SpecifyKind(model.DueDate.Value, DateTimeKind.Utc);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, TaskItemDto.FromEntity(created));
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskItem model)
        {
            if (model.DueDate.HasValue && model.DueDate.Value.Kind == DateTimeKind.Unspecified)
            {
                model.DueDate = DateTime.SpecifyKind(model.DueDate.Value, DateTimeKind.Utc);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest("Id mismatch");
            }

            var updated = await _service.UpdateAsync(model);
            var dto = TaskItemDto.FromEntity(updated);
            return Ok(dto);
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // GET: api/tasks/assigned/{userId}
        [HttpGet("assigned/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAssignedToUser(
            Guid userId, [FromQuery] bool? isCompleted = null, [FromQuery] string? sortBy = null, [FromQuery] bool ascending = true)
        {
            var tasks = await _service.GetAssignedToUserAsync(userId, isCompleted, sortBy, ascending);
            return Ok(tasks.Select(TaskItemDto.FromEntity));
        }

        // GET: api/tasks/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> Search(
            [FromQuery] string? title,
            [FromQuery] DateTime? createdFrom,
            [FromQuery] DateTime? createdTo,
            [FromQuery] DateTime? dueFrom,
            [FromQuery] DateTime? dueTo
        )
        {
            var tasks = await _service.SearchAsync(title, createdFrom, createdTo, dueFrom, dueTo);
            return Ok(tasks.Select(TaskItemDto.FromEntity));
        }

        [HttpPost("{taskId}/add-tag/{tagId}")]
        public async Task<IActionResult> AddTag(Guid taskId, Guid tagId, [FromServices] ITaskItemService taskService, [FromServices] ITagService tagService)
        {
            var task = await taskService.GetByIdAsync(taskId);
            var tag = await tagService.GetByIdAsync(tagId);
            if (task == null || tag == null)
            {
                return NotFound();
            }

            if (!task.Tags.Any(t => t.Id == tagId))
            {
                task.Tags.Add(tag);
                await taskService.UpdateAsync(task);
            }
            return Ok();
        }

        [HttpPost("{taskId}/remove-tag/{tagId}")]
        public async Task<IActionResult> RemoveTag(Guid taskId, Guid tagId, [FromServices] ITaskItemService taskService)
        {
            var task = await taskService.GetByIdAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            var tagToRemove = task.Tags.FirstOrDefault(t => t.Id == tagId);
            if (tagToRemove != null)
            {
                task.Tags.Remove(tagToRemove);
                await taskService.UpdateAsync(task);
            }
            return Ok();
        }
    }
}
