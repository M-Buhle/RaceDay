using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RaceDayAPI.Models
{
   public class EventEnrolment
   {
       [Key]
       public int EnrolmentID { get; set; }
       [Required]
       public int EventID { get; set; }
       [Required]
       public int ParticipantID { get; set; }
       [Required]
       public int CategoryID { get; set; }
       public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;
       [Required]
       [StringLength(50)]
       public string RegistrationNumber { get; set; } = string.Empty;
       public bool IsPaid { get; set; } = false;
       [StringLength(20)]
       public string Status { get; set; } = "Registered";
       public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       public DateTime? UpdatedAt { get; set; }
       // Navigation
       [ForeignKey("EventID")]
       public virtual Event? Event { get; set; }
       [ForeignKey("ParticipantID")]
       public virtual User? Participant { get; set; }
       [ForeignKey("CategoryID")]
       public virtual Category? Category { get; set; }
   }
}