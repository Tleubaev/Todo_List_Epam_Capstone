using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models
{
    public class Tag
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; } = default!;

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
