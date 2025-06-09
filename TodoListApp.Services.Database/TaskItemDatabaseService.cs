using Microsoft.EntityFrameworkCore;
using TodoListApp.Services;
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
    }
}
