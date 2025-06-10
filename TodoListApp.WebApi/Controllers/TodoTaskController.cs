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
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByTodoListId(Guid todoListId)
        {
            var tasks = await _service.GetByTodoListIdAsync(todoListId);
            return Ok(tasks);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetById(Guid id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create([FromBody] TaskItem model)
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
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
            return Ok(updated);
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
