using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RaceDayAPI.Models
{
   public class Event
   {
       [Key]
       public int EventID { get; set; }
       [Required]
       public int OrganiserID { get; set; }
       [Required]
       [StringLength(100)]
       public string EventName { get; set; } = string.Empty;
       public string? EventDescription { get; set; }
       [Required]
       [StringLength(20)]
       public string EventType { get; set; } = string.Empty; // Running, Walking, Cycling
       [Required]
       public DateTime EventDate { get; set; }
       public DateTime? RegistrationStartDate { get; set; }
       public DateTime? RegistrationEndDate { get; set; }
       public int? MaxParticipants { get; set; }
       [Column(TypeName = "decimal(10,2)")]
       public decimal? EntryFee { get; set; }
       [StringLength(200)]
       public string? Location { get; set; }
       [StringLength(50)]
       public string? City { get; set; }
       [StringLength(50)]
       public string? Province { get; set; }
       public string? RouteDetails { get; set; }
       public bool IsActive { get; set; } = true;
       public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       public DateTime? UpdatedAt { get; set; }
       // Navigation properties
       [ForeignKey("OrganiserID")]
       public virtual User? Organiser { get; set; }
       public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
       public virtual ICollection<EventEnrolment> Enrolments { get; set; } = new List<EventEnrolment>();
   }
}