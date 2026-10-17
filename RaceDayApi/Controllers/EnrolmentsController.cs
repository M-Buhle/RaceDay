using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
namespace RaceDayAPI.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class EnrolmentsController : ControllerBase
   {
       private readonly RaceDayDbContext _context;
       public EnrolmentsController(RaceDayDbContext context) => _context = context;
       private bool IsParticipant() => HttpContext.Session.GetString("UserRole") == "Participant";
       private bool IsOrganiser() => HttpContext.Session.GetString("UserRole") == "Organiser";
       private bool IsAuthenticated() => HttpContext.Session.GetInt32("UserId") != null;
       private int GetUserId() => HttpContext.Session.GetInt32("UserId") ?? 0;
       // POST: api/enrolments (Participant only)
       [HttpPost]
       public async Task<IActionResult> Create([FromBody] EnrolDto dto)
       {
           if (!IsAuthenticated()) return Unauthorized();
           if (!IsParticipant()) return StatusCode(403, new { message = "Only participants can enrol" });
           var userId = GetUserId();
           if (await _context.EventEnrolments.AnyAsync(e => e.EventID == dto.EventID && e.ParticipantID == userId))
               return Conflict(new { message = "Already enrolled in this event" });
           var ev = await _context.Events.FindAsync(dto.EventID);
           if (ev == null) return NotFound(new { message = "Event not found" });
           var enrolment = new EventEnrolment
           {
               EventID = dto.EventID,
               CategoryID = dto.CategoryID,
               ParticipantID = userId,
               RegistrationNumber = $"REG-{DateTime.UtcNow.Year}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
               EnrolmentDate = DateTime.UtcNow
           };
           _context.EventEnrolments.Add(enrolment);
           await _context.SaveChangesAsync();
           return CreatedAtAction(nameof(GetById), new { id = enrolment.EnrolmentID }, enrolment);
       }
       // GET: api/enrolments/5
       [HttpGet("{id}")]
       public async Task<IActionResult> GetById(int id)
       {
           if (!IsAuthenticated()) return Unauthorized();
           var enrolment = await _context.EventEnrolments
               .Include(e => e.Event)
               .Include(e => e.Category)
               .FirstOrDefaultAsync(e => e.EnrolmentID == id);
           if (enrolment == null) return NotFound();
           // Only owner or organiser can view
           if (enrolment.ParticipantID != GetUserId() && !IsOrganiser())
               return StatusCode(403);
           return Ok(enrolment);
       }
       // GET: api/enrolments/my-enrolments
       [HttpGet("my-enrolments")]
       public async Task<IActionResult> GetMyEnrolments()
       {
           if (!IsAuthenticated()) return Unauthorized();
           var userId = GetUserId();
           return Ok(await _context.EventEnrolments
               .Include(e => e.Event)
               .Where(e => e.ParticipantID == userId)
               .ToListAsync());
       }
       // GET: api/enrolments/event/5
       [HttpGet("event/{eventId}")]
       public async Task<IActionResult> GetByEvent(int eventId)
       {
           if (!IsAuthenticated()) return Unauthorized();
           if (!IsOrganiser()) return StatusCode(403);
           return Ok(await _context.EventEnrolments
               .Include(e => e.Participant)
               .Where(e => e.EventID == eventId)
               .ToListAsync());
       }
   }
   public class EnrolDto
   {
       public int EventID { get; set; }
       public int CategoryID { get; set; }
   }
}