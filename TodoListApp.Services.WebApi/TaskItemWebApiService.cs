using System.Net.Http.Json;
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

        public async Task<TaskItem?> GetByIdAsync(Guid id)
            => await _httpClient.GetFromJsonAsync<TaskItem>($"api/tasks/{id}");

        public async Task<IEnumerable<TaskItem>> GetByTodoListIdAsync(Guid todoListId)
            => await _httpClient.GetFromJsonAsync<IEnumerable<TaskItem>>($"api/tasks/todolist/{todoListId}");

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tasks", task);
            response.EnsureSuccessStatusCode();
            var dto = await response.Content.ReadFromJsonAsync<TaskItemDto>();
            return MapFromDto(dto!);
        }

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/tasks/{task.Id}", task);
            response.EnsureSuccessStatusCode();
            var dto = await response.Content.ReadFromJsonAsync<TaskItemDto>();
            return MapFromDto(dto!);
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/tasks/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<TaskItem>> GetAssignedToUserAsync(Guid userId, bool? isCompleted = null, string? sortBy = null, bool ascending = true)
        {
            var url = $"api/tasks/assigned/{userId}";
            var query = new List<string>();

            if (isCompleted.HasValue)
            {
                query.Add($"isCompleted={isCompleted.Value.ToString().ToLower()}");
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query.Add($"sortBy={sortBy}");
            }
            if (!ascending)
            {
                query.Add("ascending=false");
            }
            if (query.Any())
            {
                url += "?" + string.Join("&", query);
            }

            return await _httpClient.GetFromJsonAsync<IEnumerable<TaskItem>>(url);
        }

        public async Task<IEnumerable<TaskItem>> SearchAsync(string? title, DateTime? createdFrom, DateTime? createdTo, DateTime? dueFrom, DateTime? dueTo)
        {
            var url = "api/tasks/search?";
            var param = new List<string>();
            if (!string.IsNullOrWhiteSpace(title))
            {
                param.Add($"title={Uri.EscapeDataString(title)}");
            }
            if (createdFrom.HasValue)
            {
                param.Add($"createdFrom={createdFrom.Value:O}");
            }
            if (createdTo.HasValue)
            {
                param.Add($"createdTo={createdTo.Value:O}");
            }
            if (dueFrom.HasValue)
            {
                param.Add($"dueFrom={dueFrom.Value:O}");
            }
            if (dueTo.HasValue)
            {
                param.Add($"dueTo={dueTo.Value:O}");
            }
            url += string.Join("&", param);
            return await _httpClient.GetFromJsonAsync<IEnumerable<TaskItem>>(url);
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

        private TaskItem MapFromDto(TaskItemDto dto)
        {
            return new TaskItem
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                TodoListId = dto.TodoListId,
                IsCompleted = dto.IsCompleted,
                CreatedAt = dto.CreatedAt,
                DueDate = dto.DueDate,
                Tags = dto.Tags?.Select(t => new Tag { Id = t.Id, Name = t.Name }).ToList(),
                Comments = dto.Comments?.Select(c => new Comment
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId,
                }).ToList()
            };
        }
    }
}
