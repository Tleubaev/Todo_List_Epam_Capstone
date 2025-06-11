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
                PasswordHash = dto.Password,
                Role = UserRoles.User
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new User { Id = user.Id, UserName = user.UserName, Role = user.Role });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.UserName == dto.UserName && u.PasswordHash == dto.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(new User { Id = user.Id, UserName = user.UserName });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                return NotFound("No user with that email found");
            }

            _logger.LogInformation("Password reset requested for email {Email}", dto.Email);

            return Ok("Password reset instructions sent (imitation)");
        }
    }
}
