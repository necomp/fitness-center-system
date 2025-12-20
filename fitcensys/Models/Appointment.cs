using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitcensys.Models
{

    public enum AppointmentStatus
    {
        Pending,    // Onay Bekliyor
        Confirmed,  // Onaylandı
        Completed,  // Tamamlandı (Geçmiş)
        Cancelled,  // İptal Edildi
        NoShow      // Üye Gelmedi
    }
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        // Alan kişi
        [Display(Name = "Üye")]
        public string MemberID { get; set; } // IdentityUser ID'si string'dir (Guid)
        [ForeignKey("MemberID")]
        public ApplicationUser? Member { get; set; }

        // Alınan eğitmen
        [Display(Name = "Eğitmen")]
        public int TrainerID { get; set; }
        public Trainer? Trainer { get; set; }

        // Alınan hizmet (GymService'e bağlıyoruz, ServiceDefinition'a değil.)
        [Display(Name = "Alınacak Hizmet")]
        public int GymServiceID { get; set; }
        public GymService? GymService { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; } // Tarih

        [Required]
        public TimeSpan StartTime { get; set; } // Başlangıç Saati

        [Required]
        public TimeSpan EndTime { get; set; } // Bitiş Saati (Hesaplanıp kaydedilecek)

        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        // SNAPSHOT ALANI: Randevu alındığı andaki fiyat.
        // Hizmet fiyatı değişse bile bu değişmemeli.
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceSnapshot { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
