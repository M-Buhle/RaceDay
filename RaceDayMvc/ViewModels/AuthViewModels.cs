using System.ComponentModel.DataAnnotations;
namespace RaceDayMvc.ViewModels
{
   public class LoginViewModel
   {
       [Required, EmailAddress]
       public string Email { get; set; } = string.Empty;
       [Required, DataType(DataType.Password)]
       public string Password { get; set; } = string.Empty;
   }
   public class RegisterViewModel
   {
       [Required]
       public string FirstName { get; set; } = string.Empty;
       [Required]
       public string LastName { get; set; } = string.Empty;
       [Required, EmailAddress]
       public string Email { get; set; } = string.Empty;
       [Required, DataType(DataType.Password)]
       public string Password { get; set; } = string.Empty;
       [Required]
       public string IDNumber { get; set; } = string.Empty;
       public string? PhoneNumber { get; set; }
       [Required]
       public string Role { get; set; } = "Participant"; // Organiser or Participant
   }
}