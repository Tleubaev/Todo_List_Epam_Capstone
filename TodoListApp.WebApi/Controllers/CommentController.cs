using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.DTO;
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
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetByTaskItemId(Guid taskItemId)
        {
            var comments = await _service.GetByTaskItemIdAsync(taskItemId);
            var dtos = comments.Select(comment => new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserId = comment.UserId,
                UserName = comment.User?.UserName
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetById(Guid id)
        {
            var comment = await _service.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            var dto = new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserId = comment.UserId,
                UserName = comment.User?.UserName
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> Create([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
