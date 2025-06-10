namespace TodoListApp.WebApi.Models;

public class LoginUserDto
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}
