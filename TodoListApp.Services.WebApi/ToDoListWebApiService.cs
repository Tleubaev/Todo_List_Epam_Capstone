using System.Net.Http;
using System.Net.Http.Json;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class ToDoListWebApiService : IToDoListService
    {
        private readonly HttpClient _httpClient;

        public ToDoListWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ToDoList?> GetByIdAsync(Guid id) =>
            await _httpClient.GetFromJsonAsync<ToDoList>($"api/todolists/{id}");

        public async Task<IEnumerable<ToDoList>> GetByUserIdAsync(Guid userId) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<ToDoList>>($"api/todolists/user/{userId}");

        public async Task<ToDoList> CreateAsync(ToDoList list)
        {
            var response = await _httpClient.PostAsJsonAsync("api/todolists", list);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ToDoList>();
        }

        public async Task<ToDoList> UpdateAsync(ToDoList list)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/todolists/{list.Id}", list);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ToDoList>();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/todolists/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
