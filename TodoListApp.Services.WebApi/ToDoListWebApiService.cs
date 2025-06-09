using System.Net.Http;
using System.Net.Http.Json;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TodoListWebApiService : ITodoListService
    {
        private readonly HttpClient _httpClient;

        public TodoListWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TodoList?> GetByIdAsync(Guid id) =>
            await _httpClient.GetFromJsonAsync<TodoList>($"api/todolists/{id}");

        public async Task<IEnumerable<TodoList>> GetByUserIdAsync(Guid userId) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<TodoList>>($"api/todolists/user/{userId}");

        public async Task<TodoList> CreateAsync(TodoList list)
        {
            var response = await _httpClient.PostAsJsonAsync("api/todolists", list);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoList>();
        }

        public async Task<TodoList> UpdateAsync(TodoList list)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/todolists/{list.Id}", list);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoList>();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/todolists/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
