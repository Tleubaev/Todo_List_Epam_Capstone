using TodoListApp.WebApi.Models;

namespace TodoListApp.Services;
public interface ICommentService
{
    Task<Comment?> GetByIdAsync(Guid id);
    Task<IEnumerable<Comment>> GetByTaskItemIdAsync(Guid taskItemId);
    Task<Comment> CreateAsync(Comment comment);
    Task<Comment> UpdateAsync(Comment comment);
    Task DeleteAsync(Guid id);
}
