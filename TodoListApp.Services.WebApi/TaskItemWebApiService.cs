using System.Net.Http;
using System.Net.Http.Json;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TaskItemWebApiService : ITaskItemService
    {
        private readonly HttpClient _httpClient;

        public TaskItemWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id) =>
            await _httpClient.GetFromJsonAsync<TaskItem>($"api/tasks/{id}");

        public async Task<IEnumerable<TaskItem>> GetByToDoListIdAsync(Guid toDoListId) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<TaskItem>>($"api/tasks/todolist/{toDoListId}");

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tasks", task);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TaskItem>();
        }

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/tasks/{task.Id}", task);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TaskItem>();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/tasks/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
