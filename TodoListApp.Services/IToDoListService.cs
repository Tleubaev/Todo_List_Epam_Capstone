using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services;
public interface ITodoListService
{
    Task<TodoList?> GetByIdAsync(Guid id);
    Task<IEnumerable<TodoList>> GetByUserIdAsync(Guid userId);
    Task<TodoList> CreateAsync(TodoList list);
    Task<TodoList> UpdateAsync(TodoList list);
    Task DeleteAsync(Guid id);
}
