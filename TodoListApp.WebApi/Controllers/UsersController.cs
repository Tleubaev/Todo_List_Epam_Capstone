using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly TodoDbContext _db;
        private readonly ILogger<UsersController> _logger;

        public UsersController(TodoDbContext db, ILogger<UsersController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Registration endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.UserName == dto.UserName || u.Email == dto.Email))
            {
                return Conflict("UserName or Email already exists");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = dto.Password // For demo! Use hashing in production!
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok(new { user.Id, user.UserName, user.Email });
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.UserName == dto.UserName && u.PasswordHash == dto.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Для простоты вернем Id и UserName, можно добавить JWT
            return Ok(new User { Id = user.Id, UserName = user.UserName });
        }

        // Forgot password endpoint (imitation)
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                return NotFound("No user with that email found");
            }

            // Здесь обычно генерируется токен и отправляется email
            _logger.LogInformation("Password reset requested for email {Email}", dto.Email);

            // Имитация — просто вернем сообщение
            return Ok("Password reset instructions sent (imitation)");
        }
    }
}
