using Microsoft.EntityFrameworkCore;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database
{
    public class ToDoListDatabaseService : IToDoListService
    {
        private readonly TodoDbContext _context;

        public ToDoListDatabaseService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<ToDoList?> GetByIdAsync(Guid id) =>
            await _context.ToDoLists.Include(l => l.Tasks).FirstOrDefaultAsync(l => l.Id == id);

        public async Task<IEnumerable<ToDoList>> GetByUserIdAsync(Guid userId) =>
            await _context.ToDoLists.Where(l => l.UserId == userId).ToListAsync();

        public async Task<ToDoList> CreateAsync(ToDoList list)
        {
            _context.ToDoLists.Add(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task<ToDoList> UpdateAsync(ToDoList list)
        {
            _context.ToDoLists.Update(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task DeleteAsync(Guid id)
        {
            var list = await _context.ToDoLists.FindAsync(id);
            if (list != null)
            {
                _context.ToDoLists.Remove(list);
                await _context.SaveChangesAsync();
            }
        }
    }
}
