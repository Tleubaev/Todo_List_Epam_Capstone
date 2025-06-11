using TodoListApp.WebApi.Models;

namespace TodoListApp.Services
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUserNameAsync(string userName);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
