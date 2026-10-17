using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models

{

    public class Result

    {

        [Key]

        public int ResultID { get; set; }

        [Required]

        public int EnrolmentID { get; set; }

        [Required]

        public int ParticipantID { get; set; }

        [Required]

        public int EventID { get; set; }

        [Required]

        public int CategoryID { get; set; }

        public TimeSpan? FinishTime { get; set; }

        public int? Position { get; set; }

        public int? GenderPosition { get; set; }

        public int? AgeGroupPosition { get; set; }

        public int? OverallPosition { get; set; }

        public bool IsDisqualified { get; set; } = false;

        [StringLength(200)]

        public string? DisqualificationReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation

        [ForeignKey("EnrolmentID")]

        public virtual EventEnrolment? Enrolment { get; set; }

        [ForeignKey("ParticipantID")]

        public virtual User? Participant { get; set; }

        [ForeignKey("EventID")]

        public virtual Event? Event { get; set; }

        [ForeignKey("CategoryID")]

        public virtual Category? Category { get; set; }

    }

}
 