using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Database
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Пример фиксированных Id для тестовых данных
            var testUserId = Guid.Parse("cfa7e3c5-2b24-4d5e-8c91-8d3a1a07e0f3");
            var testListId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var testTaskId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = testUserId,
                    UserName = "testuser",
                    Email = "test@test.kz",
                    PasswordHash = "123"
                }
            );

            modelBuilder.Entity<TodoList>().HasData(
                new TodoList
                {
                    Id = testListId,
                    Title = "List1",
                    Description = "First List",
                    UserId = testUserId
                }
            );

            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = testTaskId,
                    Title = "Task1",
                    Description = "First Task",
                    TodoListId = testListId,
                    IsCompleted = false
                }
            );
        }
    }
}
