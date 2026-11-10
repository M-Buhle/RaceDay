using Microsoft.AspNetCore.Mvc;
using RaceDayMvc.Services;
using RaceDayMvc.ViewModels;
using System.Text.Json;
namespace RaceDayMvc.Controllers
{
   public class AuthController : Controller
   {
       private readonly RaceDayApiClient _api;
       public AuthController(RaceDayApiClient api)
       {
           _api = api;
       }
       [HttpGet]
       public IActionResult Login()
       {
           return View();
       }
       [HttpPost]
       public async Task<IActionResult> Login(LoginViewModel model)
       {
           if (!ModelState.IsValid) return View(model);
           var response = await _api.PostAsync("api/auth/login", new { Email = model.Email, Password = model.Password });
           if (!response.IsSuccessStatusCode)
           {
               ModelState.AddModelError("", "Invalid email or password.");
               return View(model);
           }
           // Extract the session cookie from the API response
           if (response.Headers.TryGetValues("Set-Cookie", out var cookieValues))
           {
               var sessionCookie = cookieValues.FirstOrDefault(c => c.StartsWith(".AspNetCore.Session="));
               if (sessionCookie != null)
               {
                   // Save the API cookie into the MVC session
                   HttpContext.Session.SetString("ApiSessionCookie", sessionCookie.Split(';')[0]);
               }
           }
           // Read the response to get the user's role
           var jsonResponse = await response.Content.ReadAsStringAsync();
           var authData = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
           string role = authData.GetProperty("role").GetString() ?? "Participant";
           int userId = authData.GetProperty("userId").GetInt32();
           // Save user details in MVC Session
           HttpContext.Session.SetInt32("UserId", userId);
           HttpContext.Session.SetString("Role", role);
           HttpContext.Session.SetString("Email", model.Email);
           // Redirect based on role
           if (role == "Organiser")
               return RedirectToAction("Index", "Organiser");
           return RedirectToAction("Index", "Participant");
       }
       [HttpGet]
       public IActionResult Register()
       {
           return View();
       }
       [HttpPost]
       public async Task<IActionResult> Register(RegisterViewModel model)
       {
           if (!ModelState.IsValid) return View(model);
           var response = await _api.PostAsync("api/auth/register", model);
           if (!response.IsSuccessStatusCode)
           {
               ModelState.AddModelError("", "Registration failed. Email might already exist.");
               return View(model);
           }
           TempData["SuccessMessage"] = "Registration successful! Please log in.";
           return RedirectToAction(nameof(Login));
       }
       [HttpPost]
       public IActionResult Logout()
       {
           HttpContext.Session.Clear(); // Clear MVC session
           return RedirectToAction("Index", "Home");
       }
   }
}