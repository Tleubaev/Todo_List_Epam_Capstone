using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models
{
    /// <summary>
    /// Модель комментария к задаче.
    /// </summary>
    public class Comment
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; } = default!;

        [Required]
        public Guid TaskItemId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public TaskItem TaskItem { get; set; } = default!;
        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}
