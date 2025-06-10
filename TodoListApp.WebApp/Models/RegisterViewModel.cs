using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    [StringLength(100)]
    public string UserName { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = default!;
}
