using TodoListApp.WebApi.DTO;
using TodoListApp.WebApi.Models;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid TodoListId { get; set; }
    public List<TagDto>? Tags { get; set; }
    public List<CommentDto>? Comments { get; set; }

    public static TaskItemDto FromEntity(TaskItem task)
    {
        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            DueDate = task.DueDate,
            TodoListId = task.TodoListId,
            Tags = task.Tags?.Select(t => new TagDto { Id = t.Id, Name = t.Name }).ToList(),
            Comments = task.Comments?.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId,
                UserName = c.User?.UserName
            }).ToList()
        };
    }
}
