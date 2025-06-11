using TodoListApp.WebApi.Models;

namespace TodoListApp.Services;
public interface ITagService
{
    Task<Tag?> GetByIdAsync(Guid id);
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<Tag> CreateAsync(Tag tag);
    Task<Tag> UpdateAsync(Tag tag);
    Task DeleteAsync(Guid id);

    Task AddTagAsync(Guid taskId, Guid tagId);
    Task RemoveTagAsync(Guid taskId, Guid tagId);
}
