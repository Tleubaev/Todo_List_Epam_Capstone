using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string PasswordHash { get; set; } = default!;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = UserRoles.User;

        public ICollection<TodoList> TodoLists { get; set; } = new List<TodoList>();
    }
}
