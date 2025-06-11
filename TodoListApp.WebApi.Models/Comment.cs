using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; } = default!;

        [Required]
        public Guid TaskItemId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public TaskItem? TaskItem { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}
