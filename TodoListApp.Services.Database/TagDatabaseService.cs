using Microsoft.EntityFrameworkCore;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database
{
    public class TagDatabaseService : ITagService
    {
        private readonly TodoDbContext _context;

        public TagDatabaseService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<Tag?> GetByIdAsync(Guid id) =>
            await _context.Tags
                .Include(t => t.Tasks)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IEnumerable<Tag>> GetAllAsync() =>
            await _context.Tags.Include(t => t.Tasks).ToListAsync();

        public async Task<Tag> CreateAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task DeleteAsync(Guid id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
    }
}
