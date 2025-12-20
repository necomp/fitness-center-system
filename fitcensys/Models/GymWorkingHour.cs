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

        [Display(Name = "Gün")]
        [Required(ErrorMessage = "Lütfen bir gün seçiniz.")]
        public DayOfWeek Day { get; set; } // Enum: Sunday=0, Monday=1...

        [Display(Name = "Açılış Saati")]
        [Required(ErrorMessage = "Açılış saati zorunludur.")]
        [DataType(DataType.Time)]
        public TimeSpan OpeningTime { get; set; }

        [Display(Name = "Kapanış Saati")]
        [Required(ErrorMessage = "Kapanış saati zorunludur.")]
        [DataType(DataType.Time)]
        public TimeSpan ClosingTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Kural 1: Bitiş saati, Başlangıçtan önce veya eşit olamaz.
            if (ClosingTime <= OpeningTime)
            {
                // Hatanın hangi alanda görüneceğini (EndTime) belirtiyoruz
                yield return new ValidationResult(
                    "Bitiş saati, başlangıç saatinden sonra olmalıdır.",
                    new[] { nameof(ClosingTime) }
                );
            }
        }
    }
}
