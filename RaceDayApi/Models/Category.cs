using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RaceDayAPI.Models
{
   public class Category
   {
       [Key]
       public int CategoryID { get; set; }
       [Required]
       public int EventID { get; set; }
       [Required]
       [StringLength(50)]
       public string CategoryName { get; set; } = string.Empty;
       [Column(TypeName = "decimal(6,2)")]
       public decimal Distance { get; set; }
       [StringLength(20)]
       public string? AgeGroup { get; set; }
       [StringLength(10)]
       public string? GenderRestriction { get; set; }
       [Column(TypeName = "decimal(10,2)")]
       public decimal? EntryFee { get; set; }
       public int? MaxParticipants { get; set; }
       public bool IsActive { get; set; } = true;
       public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       [ForeignKey("EventID")]
       public virtual Event? Event { get; set; }
   }
}