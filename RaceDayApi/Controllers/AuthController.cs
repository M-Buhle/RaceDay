using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
namespace RaceDayAPI.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class AuthController : ControllerBase
   {
       private readonly RaceDayDbContext _context;
       private readonly ILogger<AuthController> _logger;
       public AuthController(RaceDayDbContext context, ILogger<AuthController> logger)
       {
           _context = context;
           _logger = logger;
       }
       [HttpPost("register")]
       public async Task<IActionResult> Register([FromBody] RegisterDto dto)
       {
           if (!ModelState.IsValid)
               return BadRequest(ModelState);
           if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
               return Conflict(new { message = "Email already exists" });
           if (dto.Role != "Organiser" && dto.Role != "Participant")
               return BadRequest(new { message = "Role must be 'Organiser' or 'Participant'" });
           var user = new User
           {
               Email = dto.Email,
               PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
               FirstName = dto.FirstName,
               LastName = dto.LastName,
               IDNumber = dto.IDNumber,
               PhoneNumber = dto.PhoneNumber,
               DateOfBirth = dto.DateOfBirth,
               Gender = dto.Gender,
               Role = dto.Role
           };
           _context.Users.Add(user);
           await _context.SaveChangesAsync();
           return CreatedAtAction(nameof(Register), new { userId = user.UserID, email = user.Email, role = user.Role });
       }
       [HttpPost("login")]
       public async Task<IActionResult> Login([FromBody] LoginDto dto)
       {
           var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
           if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
               return Unauthorized(new { message = "Invalid email or password" });
           if (!user.IsActive)
               return Unauthorized(new { message = "Account is deactivated" });
           // Store session
           HttpContext.Session.SetInt32("UserId", user.UserID);
           HttpContext.Session.SetString("UserRole", user.Role);
           HttpContext.Session.SetString("UserEmail", user.Email);
           return Ok(new
           {
               message = "Login successful",
               userId = user.UserID,
               email = user.Email,
               role = user.Role
           });
       }
       [HttpPost("logout")]
       public IActionResult Logout()
       {
           HttpContext.Session.Clear();
           return Ok(new { message = "Logged out successfully" });
       }
   }
   public class RegisterDto
   {
       public string Email { get; set; } = string.Empty;
       public string Password { get; set; } = string.Empty;
       public string FirstName { get; set; } = string.Empty;
       public string LastName { get; set; } = string.Empty;
       public string IDNumber { get; set; } = string.Empty;
       public string? PhoneNumber { get; set; }
       public DateTime? DateOfBirth { get; set; }
       public string? Gender { get; set; }
       public string Role { get; set; } = "Participant";
   }
   public class LoginDto
   {
       public string Email { get; set; } = string.Empty;
       public string Password { get; set; } = string.Empty;
   }
}