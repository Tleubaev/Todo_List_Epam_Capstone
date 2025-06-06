using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models
{
    /// <summary>
    /// Модель списка задач.
    /// </summary>
    public class ToDoList
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        public Guid UserId { get; set; }

        // Навигационные свойства
        public User User { get; set; } = default!;
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
