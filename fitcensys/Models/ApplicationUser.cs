using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace fitcensys.Models
{
    public class ApplicationUser : IdentityUser
    {
        // IdentityUser Id, UserName, Email, PasswordHash barındırıyor

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        // fiziksel bilgiler (çeşitlendirilebilir)
        // Sistem üyeliği sürecinde her ay ayrı kilo verisi kaydedilebilir.
        public double? Height { get; set; } // cm
        public double? Weight { get; set; } // kg

        // Üyenin Randevuları
        public ICollection<Appointment> Appointments { get; set; }
    }
}
