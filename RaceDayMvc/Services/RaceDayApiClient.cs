using System.Net.Http.Headers;
using System.Text.Json;
namespace RaceDayMvc.Services
{
   public class RaceDayApiClient
   {
       private readonly HttpClient _httpClient;
       private readonly IHttpContextAccessor _httpContextAccessor;
       public RaceDayApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
       {
           _httpClient = httpClient;
           _httpContextAccessor = httpContextAccessor;
       }
       // Helper to add the API session cookie to requests
       private void AttachSessionCookie()
       {
           var apiCookie = _httpContextAccessor.HttpContext?.Session.GetString("ApiSessionCookie");
           if (!string.IsNullOrEmpty(apiCookie))
           {
               _httpClient.DefaultRequestHeaders.Remove("Cookie");
               _httpClient.DefaultRequestHeaders.Add("Cookie", apiCookie);
           }
       }
       public async Task<HttpResponseMessage> GetAsync(string endpoint)
       {
           AttachSessionCookie();
           return await _httpClient.GetAsync(endpoint);
       }
       public async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
       {
           AttachSessionCookie();
           var json = JsonSerializer.Serialize(data);
           var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
           return await _httpClient.PostAsync(endpoint, content);
       }
       public async Task<HttpResponseMessage> UploadFileAsync(string endpoint, IFormFile file)
       {
           AttachSessionCookie();
           using var content = new MultipartFormDataContent();
           using var stream = file.OpenReadStream();
           var fileContent = new StreamContent(stream);
           fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
           content.Add(fileContent, "file", file.FileName);
           return await _httpClient.PostAsync(endpoint, content);
       }
   }
}