using System.ComponentModel.DataAnnotations;

namespace RaceDayAPI.Models

{

    public class User

    {

        [Key]

        public int UserID { get; set; }

        [Required]

        [EmailAddress]

        [StringLength(100)]

        public string Email { get; set; } = string.Empty;

        [Required]

        public string PasswordHash { get; set; } = string.Empty;

        [Required]

        [StringLength(50)]

        public string FirstName { get; set; } = string.Empty;

        [Required]

        [StringLength(50)]

        public string LastName { get; set; } = string.Empty;

        [Required]

        [StringLength(13)]

        public string IDNumber { get; set; } = string.Empty;

        [StringLength(15)]

        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]

        public string? Gender { get; set; }

        [Required]

        [StringLength(20)]

        public string Role { get; set; } = "Participant"; // Organiser or Participant

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

    }

}
 