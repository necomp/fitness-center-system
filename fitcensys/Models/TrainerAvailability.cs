using System.ComponentModel.DataAnnotations;

namespace fitcensys.Models
{
    public class TrainerAvailability
    {
        [Key]
        public int TrainerAvailabilityID { get; set; }

        public int TrainerID { get; set; }
        public Trainer? Trainer { get; set; }

        [Display(Name = "Gün")]
        [Required(ErrorMessage = "Lütfen bir gün seçiniz.")]
        public DayOfWeek Day { get; set; }

        [Display(Name = "Başlangıç Saati")]
        [Required(ErrorMessage = "Başlangıç saati zorunludur.")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "Bitiş Saati")]
        [Required(ErrorMessage = "Bitiş saati zorunludur.")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Kural 1: Bitiş saati, Başlangıçtan önce veya eşit olamaz.
            if (EndTime <= StartTime)
            {
                // Hatanın hangi alanda görüneceğini (EndTime) belirtiyoruz
                yield return new ValidationResult(
                    "Bitiş saati, başlangıç saatinden sonra olmalıdır.",
                    new[] { nameof(EndTime) }
                );
            }
        }
    }
}
