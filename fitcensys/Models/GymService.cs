using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitcensys.Models
{
    public class GymService
    {
        [Key]
        public int GymServiceID { get; set; } // köprü ama kendi özellikleride bulunuyor bu yüzden surrogate key ekledik

        // ait olduğu salon
        [Display(Name="Spor Salonu")]
        public int GymID { get; set; }
        public Gym? Gym { get; set; }

        [Display(Name ="Hizmet Adı")]
        public int ServiceDefinitionID { get; set; }
        public ServiceDefinition? ServiceDefinition { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")] // SQL para birimi hassasiyeti için
        public decimal Price { get; set; } // mevcut salondaki ücret

        [Required]
        public TimeSpan Duration { get; set; } // Bu salondaki seans süresi

        // Grup dersleri için kapasite (Birebir ders ise 1 yazılır)
        public int Capacity { get; set; } = 1;

        // Bu spesifik salon hizmeti için alınmış randevular
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
