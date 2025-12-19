using System.ComponentModel.DataAnnotations;

namespace fitcensys.Models
{
    public class GymWorkingHour
    {
        [Key]
        public int GymWorkingHourID { get; set; }

        [Display(Name = "Salon")]
        public int GymID { get; set; }
        public Gym? Gym { get; set; }

        [Required]
        public DayOfWeek Day { get; set; } // Enum: Sunday=0, Monday=1...

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan OpeningTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan ClosingTime { get; set; }
    }
}
