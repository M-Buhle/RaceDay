using Microsoft.AspNetCore.Mvc;
using RaceDayMvc.Services;
using System.Text.Json;
namespace RaceDayMvc.Controllers
{
   public class ParticipantController : Controller
   {
       private readonly RaceDayApiClient _api;
       public ParticipantController(RaceDayApiClient api)
       {
           _api = api;
       }
       public async Task<IActionResult> Index()
       {
           // 1. Check if user is logged in and is a Participant
           if (HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("Login", "Auth");
           if (HttpContext.Session.GetString("Role") != "Participant") return RedirectToAction("Index", "Home");
           // 2. Fetch events from the API
           var response = await _api.GetAsync("api/events");
           // 3. Pass data to the view
           var json = await response.Content.ReadAsStringAsync();
           var events = JsonSerializer.Deserialize<List<JsonElement>>(json) ?? new List<JsonElement>();
           return View(events);
       }
       // 1. Show the event details page
       public async Task<IActionResult> Details(int id)
       {
           if (HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("Login", "Auth");
           var response = await _api.GetAsync($"api/events/{id}");
           if (!response.IsSuccessStatusCode) return NotFound();
           var json = await response.Content.ReadAsStringAsync();
           var ev = JsonSerializer.Deserialize<JsonElement>(json);
           return View(ev);
       }
       // 2. Handle the button click to enrol
       [HttpPost]
       public async Task<IActionResult> Enrol(int eventId, int categoryId)
       {
           if (HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("Login", "Auth");
           // Send request to API
           var response = await _api.PostAsync("api/enrolments", new { EventID = eventId, CategoryID = categoryId });
           if (response.IsSuccessStatusCode)
           {
               TempData["Success"] = "Successfully enrolled in the event!";
           }
           else
           {
               TempData["Error"] = "Failed to enrol. You might already be enrolled.";
           }
           return RedirectToAction(nameof(Index));
       }
   }
}