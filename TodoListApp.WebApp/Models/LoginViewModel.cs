using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required]
    public string UserName { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public string? ReturnUrl { get; set; }
}
