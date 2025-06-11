using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.WebApi.DTO;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetAll()
        {
            var tags = await _tagService.GetAllAsync();
            var dtos = tags.Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDto>> GetById(Guid id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            var dto = new TagDto { Id = tag.Id, Name = tag.Name };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<TagDto>> Create(Tag model)
        {
            var created = await _tagService.CreateAsync(model);
            var dto = new TagDto { Id = created.Id, Name = created.Name };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TagDto>> Update(Guid id, Tag model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            var updated = await _tagService.UpdateAsync(model);
            var dto = new TagDto { Id = updated.Id, Name = updated.Name };
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _tagService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasksByTag(Guid id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag.Tasks.Select(TaskItemDto.FromEntity));
        }
    }
}
