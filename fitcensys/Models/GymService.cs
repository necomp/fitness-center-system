using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitcensys.Models
{
    public class GymService
    {
        [Key]
        public int GymServiceID { get; set; }

        // ait olduğu salon
        public int GymID { get; set; }
        public Gym Gym { get; set; }

        public int ServiceDefinitionID { get; set; }
        public ServiceDefinition ServiceDefinition { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")] // SQL para birimi hassasiyeti için
        public decimal Price { get; set; } // mevcut salondaki ücret

        [Required]
        public TimeSpan Duration { get; set; } // Bu salondaki seans süresi

        // Grup dersleri için kapasite (Birebir ders ise 1 yazılır)
        public int Capacity { get; set; } = 1;

        // Bu spesifik salon hizmeti için alınmış randevular
        //public ICollection<Appointment> Appointments { get; set; }
    }
}
