using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RaceDayAPI.Models
{
   public class WeatherLog
   {
       [Key]
       public int WeatherLogID { get; set; }
       [Required]
       public int EventID { get; set; }
       [Required]
       public DateTime ForecastDate { get; set; }
       [Column(TypeName = "decimal(4,1)")]
       public decimal? Temperature { get; set; }
       [StringLength(50)]
       public string? Condition { get; set; }
       [Column(TypeName = "decimal(4,1)")]
       public decimal? WindSpeed { get; set; }
       [StringLength(20)]
       public string? WindDirection { get; set; }
       [Column(TypeName = "decimal(4,1)")]
       public decimal? Humidity { get; set; }
       [Column(TypeName = "decimal(4,2)")]
       public decimal? Precipitation { get; set; }
       public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       [ForeignKey("EventID")]
       public virtual Event? Event { get; set; }
   }
}