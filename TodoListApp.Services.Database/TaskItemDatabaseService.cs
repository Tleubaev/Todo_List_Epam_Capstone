using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database
{
    public class TaskItemDatabaseService : ITaskItemService
    {
        private readonly TodoDbContext _context;

        public TaskItemDatabaseService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id) =>
            await _context.TaskItems
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IEnumerable<TaskItem>> GetByTodoListIdAsync(Guid todoListId) =>
            await _context.TaskItems
                .Where(t => t.TodoListId == todoListId)
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .ToListAsync();

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task != null)
            {
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAssignedToUserAsync(Guid userId, bool? isCompleted = null, string? sortBy = null, bool ascending = true)
        {
            var query = _context.TaskItems
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .Where(t => t.AssignedUserId == userId);

            if (isCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == isCompleted.Value);
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy == "Title")
                {
                    query = ascending ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title);
                }
                else if (sortBy == "DueDate")
                {
                    query = ascending ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> SearchAsync(string? title, DateTime? createdFrom, DateTime? createdTo, DateTime? dueFrom, DateTime? dueTo)
        {
            var query = _context.TaskItems
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(t => t.Title.Contains(title));
            }

            if (createdFrom.HasValue)
            {
                query = query.Where(t => t.CreatedAt >= createdFrom.Value);
            }

            if (createdTo.HasValue)
            {
                query = query.Where(t => t.CreatedAt <= createdTo.Value);
            }

            if (dueFrom.HasValue)
            {
                query = query.Where(t => t.DueDate != null && t.DueDate >= dueFrom.Value);
            }

            if (dueTo.HasValue)
            {
                query = query.Where(t => t.DueDate != null && t.DueDate <= dueTo.Value);
            }

            return await query.ToListAsync();
        }

        public async Task AddTagAsync(Guid taskId, Guid tagId)
        {
            var task = await _context.TaskItems.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
            if (task != null && tag != null && !task.Tags.Contains(tag))
            {
                task.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveTagAsync(Guid taskId, Guid tagId)
        {
            var task = await _context.TaskItems.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == taskId);
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
            if (task != null && tag != null && task.Tags.Contains(tag))
            {
                task.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
    }
}
