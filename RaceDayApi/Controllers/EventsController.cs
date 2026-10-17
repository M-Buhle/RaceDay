using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
namespace RaceDayAPI.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class EventsController : ControllerBase
   {
       private readonly RaceDayDbContext _context;
       public EventsController(RaceDayDbContext context)
       {
           _context = context;
       }
       private bool IsOrganiser() => HttpContext.Session.GetString("UserRole") == "Organiser";
       private bool IsAuthenticated() => HttpContext.Session.GetInt32("UserId") != null;
       private int GetUserId() => HttpContext.Session.GetInt32("UserId") ?? 0;
       // GET: api/events (public)
       [HttpGet]
       public async Task<IActionResult> GetAll([FromQuery] string? type, [FromQuery] string? city)
       {
           var query = _context.Events.Where(e => e.IsActive).AsQueryable();
           if (!string.IsNullOrEmpty(type))
               query = query.Where(e => e.EventType == type);
           if (!string.IsNullOrEmpty(city))
               query = query.Where(e => e.City == city);
           return Ok(await query.ToListAsync());
       }
       // GET: api/events/5
       [HttpGet("{id}")]
       public async Task<IActionResult> GetById(int id)
       {
           var ev = await _context.Events
               .Include(e => e.Categories)
               .FirstOrDefaultAsync(e => e.EventID == id);
           if (ev == null) return NotFound(new { message = "Event not found" });
           return Ok(ev);
       }
       // POST: api/events (Organiser only)
       [HttpPost]
       public async Task<IActionResult> Create([FromBody] Event model)
       {
           if (!IsAuthenticated()) return Unauthorized(new { message = "Not logged in" });
           if (!IsOrganiser()) return StatusCode(403, new { message = "Only organisers can create events" });
           model.OrganiserID = GetUserId();
           model.CreatedAt = DateTime.UtcNow;
           _context.Events.Add(model);
           await _context.SaveChangesAsync();
           return CreatedAtAction(nameof(GetById), new { id = model.EventID }, model);
       }
       // PUT: api/events/5
       [HttpPut("{id}")]
       public async Task<IActionResult> Update(int id, [FromBody] Event model)
       {
           if (!IsAuthenticated()) return Unauthorized();
           if (!IsOrganiser()) return StatusCode(403, new { message = "Only organisers can update events" });
           var ev = await _context.Events.FindAsync(id);
           if (ev == null) return NotFound();
           if (ev.OrganiserID != GetUserId()) return StatusCode(403, new { message = "You can only edit your own events" });
           ev.EventName = model.EventName;
           ev.EventDescription = model.EventDescription;
           ev.EventDate = model.EventDate;
           ev.Location = model.Location;
           ev.City = model.City;
           ev.Province = model.Province;
           ev.EntryFee = model.EntryFee;
           ev.UpdatedAt = DateTime.UtcNow;
           await _context.SaveChangesAsync();
           return Ok(ev);
       }
       // DELETE: api/events/5
       [HttpDelete("{id}")]
       public async Task<IActionResult> Delete(int id)
       {
           if (!IsAuthenticated()) return Unauthorized();
           if (!IsOrganiser()) return StatusCode(403, new { message = "Only organisers can delete events" });
           var ev = await _context.Events.FindAsync(id);
           if (ev == null) return NotFound();
           if (ev.OrganiserID != GetUserId()) return StatusCode(403);
           _context.Events.Remove(ev);
           await _context.SaveChangesAsync();
           return NoContent();
       }
   }
}