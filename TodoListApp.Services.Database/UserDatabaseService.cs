using Microsoft.EntityFrameworkCore;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database
{
    public class UserDatabaseService : IUserService
    {
        private readonly TodoDbContext _context;

        public UserDatabaseService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _context.Users.Include(u => u.ToDoLists).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByUserNameAsync(string userName) =>
            await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
