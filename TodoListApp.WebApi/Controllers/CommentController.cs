using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;
        public CommentController(ICommentService service)
        {
            _service = service;
        }

        [HttpGet("task/{taskItemId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetByTaskItemId(Guid taskItemId)
        {
            var comments = await _service.GetByTaskItemIdAsync(taskItemId);
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetById(Guid id)
        {
            var comment = await _service.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> Create([FromBody] Comment comment)
        {
            var created = await _service.CreateAsync(comment);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Comment>> Update(Guid id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest("Id mismatch");
            }
            var updated = await _service.UpdateAsync(comment);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
