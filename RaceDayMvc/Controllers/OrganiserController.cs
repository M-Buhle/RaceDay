using Microsoft.AspNetCore.Mvc;
using RaceDayMvc.Services;
using System.Text.Json;
namespace RaceDayMvc.Controllers
{
   public class OrganiserController : Controller
   {
       private readonly RaceDayApiClient _api;
       public OrganiserController(RaceDayApiClient api)
       {
           _api = api;
       }
       public async Task<IActionResult> Index()
       {
           // 1. Check if user is logged in and is an Organiser
           if (HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("Login", "Auth");
           if (HttpContext.Session.GetString("Role") != "Organiser") return RedirectToAction("Index", "Home");
           // 2. Fetch events from the API
           var response = await _api.GetAsync("api/events");
           // 3. Pass data to the view
           var json = await response.Content.ReadAsStringAsync();
           var events = JsonSerializer.Deserialize<List<JsonElement>>(json) ?? new List<JsonElement>();
           return View(events);
       }
       [HttpGet]

        public IActionResult CreateEvent()

        {

            if (HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("Login", "Auth");

            if (HttpContext.Session.GetString("Role") != "Organiser") return RedirectToAction("Index", "Home");

            return View();

        }

        [HttpPost]

        public async Task<IActionResult> CreateEvent(string eventName, string eventDescription, DateTime eventDate, string location, string city, string province, string eventType, decimal entryFee, IFormFile banner)

        {

            if (HttpContext.Session.GetString("Role") != "Organiser") return RedirectToAction("Index", "Home");

            var newEvent = new

            {

                EventName = eventName,

                EventDescription = eventDescription,

                EventDate = eventDate,

                Location = location,

                City = city,

                Province = province,

                EventType = eventType,

                EntryFee = entryFee,

                IsActive = true

            };

            // Send to API

            var response = await _api.PostAsync("api/events", newEvent);

            if (response.IsSuccessStatusCode)

            {

                TempData["Success"] = "Event created successfully!";

                return RedirectToAction(nameof(Index));

            }

            else

            {

                var error = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("", "Failed to create event: " + error);

                return View();

            }

        }
 
   }
}