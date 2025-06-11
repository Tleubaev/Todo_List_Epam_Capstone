namespace TodoListApp.WebApi.DTO;

public class TodoListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
    public List<TaskItemDto>? Tasks { get; set; }
}
