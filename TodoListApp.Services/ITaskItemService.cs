using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}
