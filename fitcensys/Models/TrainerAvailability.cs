using System.ComponentModel.DataAnnotations;

namespace fitcensys.Models
{
    public class TrainerAvailability
    {
        [Key]
        public int TrainerAvailabilityID { get; set; }

        public int TrainerID { get; set; }
        public Trainer Trainer { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
    }
}
