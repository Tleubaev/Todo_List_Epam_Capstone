using TodoListApp.WebApi.Models;

namespace TodoListApp.Services;
public interface ITaskItemService
{
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetByTodoListIdAsync(Guid todoListId);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetAssignedToUserAsync(Guid userId, bool? isCompleted = null, string? sortBy = null, bool ascending = true);
    Task<IEnumerable<TaskItem>> SearchAsync(string? title, DateTime? createdFrom, DateTime? createdTo, DateTime? dueFrom, DateTime? dueTo);

    Task AddTagAsync(Guid taskId, Guid tagId);
    Task RemoveTagAsync(Guid taskId, Guid tagId);
}
