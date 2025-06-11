using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.DTO;
using TodoListApp.WebApi.Models;
namespace TodoListApp.WebApi.Controllers
{
    [Route("api/todolists")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _service;

        public TodoListController(ITodoListService service)
        {
            this._service = service;
        }

        // GET: api/todolists/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TodoListDto>>> GetTodoListsByUser(Guid userId)
        {
            var lists = await this._service.GetByUserIdAsync(userId);
            return this.Ok(lists.Select(list => new TodoListDto
            {
                Id = list.Id,
                Title = list.Title,
                Description = list.Description,
                UserId = list.UserId,
                Tasks = list.Tasks?.Select(TaskItemDto.FromEntity).ToList()
            }));
        }

        // GET: api/todolists/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListDto>> GetById(Guid id)
        {
            var list = await _service.GetByIdAsync(id);
            if (list == null)
            {
                return NotFound();
            }

            var dto = new TodoListDto
            {
                Id = list.Id,
                Title = list.Title,
                Description = list.Description,
                UserId = list.UserId,
                Tasks = list.Tasks?.Select(TaskItemDto.FromEntity).ToList()
            };

            return Ok(dto);
        }

        // POST: api/todolists
        [HttpPost]
        public async Task<ActionResult<TodoList>> Create([FromBody] TodoList model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/todolists/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TodoList model)
        {
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

        // DELETE: api/todolists/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
