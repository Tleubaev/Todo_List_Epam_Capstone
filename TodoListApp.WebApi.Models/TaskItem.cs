using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        public Guid TodoListId { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

        public TodoList? TodoList { get; set; }
        public ICollection<Tag>? Tags { get; set; } = new List<Tag>();
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();

        public Guid AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }
    }
}
