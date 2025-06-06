using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services;
public interface IToDoListService
{
    Task<ToDoList?> GetByIdAsync(Guid id);
    Task<IEnumerable<ToDoList>> GetByUserIdAsync(Guid userId);
    Task<ToDoList> CreateAsync(ToDoList list);
    Task<ToDoList> UpdateAsync(ToDoList list);
    Task DeleteAsync(Guid id);
}
