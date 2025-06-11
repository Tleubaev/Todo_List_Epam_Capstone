using System.Net.Http.Json;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.WebApi
{
    public class CommentWebApiService : ICommentService
    {
        private readonly HttpClient _httpClient;
        public CommentWebApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Comment?> GetByIdAsync(Guid id) =>
            await _httpClient.GetFromJsonAsync<Comment>($"api/comments/{id}");

        public async Task<IEnumerable<Comment>> GetByTaskItemIdAsync(Guid taskItemId) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<Comment>>($"api/comments/task/{taskItemId}");

        public async Task<Comment> CreateAsync(Comment comment)
        {
            var response = await _httpClient.PostAsJsonAsync("api/comments", comment);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Comment>()!;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/comments/{comment.Id}", comment);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Comment>();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/comments/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
