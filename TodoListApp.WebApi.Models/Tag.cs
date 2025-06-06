using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models
{
    /// <summary>
    /// Модель тега.
    /// </summary>
    public class Tag
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; } = default!;

        // Навигационные свойства
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
