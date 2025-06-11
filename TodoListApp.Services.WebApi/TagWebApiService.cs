using System.Net.Http;
using System.Net.Http.Json;
using TodoListApp.Services;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class TagWebApiService : ITagService
    {
        private readonly HttpClient _httpClient;

        public TagWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Tag?> GetByIdAsync(Guid id) =>
            await _httpClient.GetFromJsonAsync<Tag>($"api/tags/{id}");

        public async Task<IEnumerable<Tag>> GetAllAsync() =>
            await _httpClient.GetFromJsonAsync<IEnumerable<Tag>>("api/tags");

        public async Task<Tag> CreateAsync(Tag tag)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tags", tag);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Tag>();
        }

        public async Task<Tag> UpdateAsync(Tag tag)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/tags/{tag.Id}", tag);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Tag>();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/tags/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task AddTagAsync(Guid taskId, Guid tagId)
        {
            var response = await _httpClient.PostAsync($"api/tasks/{taskId}/add-tag/{tagId}", null);
            response.EnsureSuccessStatusCode();
        }
        public async Task RemoveTagAsync(Guid taskId, Guid tagId)
        {
            var response = await _httpClient.PostAsync($"api/tasks/{taskId}/remove-tag/{tagId}", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
