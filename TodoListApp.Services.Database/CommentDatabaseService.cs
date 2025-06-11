using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database
{
    public class CommentDatabaseService : ICommentService
    {
        private readonly TodoDbContext _context;

        public CommentDatabaseService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<Comment?> GetByIdAsync(Guid id) =>
            await _context.Comments
                .Include(c => c.User)
                .Include(c => c.TaskItem)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<Comment>> GetByTaskItemIdAsync(Guid taskItemId) =>
            await _context.Comments
                .Where(c => c.TaskItemId == taskItemId)
                .Include(c => c.User)
                .ToListAsync();

        public async Task<Comment> CreateAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
