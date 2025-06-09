using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database
{
    public class TodoListDatabaseService : ITodoListService
    {
        private readonly TodoDbContext _context;

        public TodoListDatabaseService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<TodoList?> GetByIdAsync(Guid id)
        {
            return await _context.TodoLists.Include(l => l.Tasks).FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<TodoList>> GetByUserIdAsync(Guid userId)
        {
            return await _context.TodoLists.Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<TodoList> CreateAsync(TodoList list)
        {
            _context.TodoLists.Add(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task<TodoList> UpdateAsync(TodoList list)
        {
            _context.TodoLists.Update(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task DeleteAsync(Guid id)
        {
            var list = await _context.TodoLists.FindAsync(id);
            if (list != null)
            {
                _context.TodoLists.Remove(list);
                await _context.SaveChangesAsync();
            }
        }
    }
}
