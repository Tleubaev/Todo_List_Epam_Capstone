using System.Net.Http;
using System.Net.Http.Json;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class UserWebApiService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _httpClient.GetFromJsonAsync<User>($"api/users/{id}");

        public async Task<User?> GetByUserNameAsync(string userName) =>
            await _httpClient.GetFromJsonAsync<User>($"api/users/byusername/{userName}");

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _httpClient.GetFromJsonAsync<IEnumerable<User>>("api/users");

        public async Task<User> CreateAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<User> UpdateAsync(User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/users/{user.Id}", user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/users/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
