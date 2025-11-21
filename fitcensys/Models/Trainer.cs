using System.ComponentModel.DataAnnotations;

namespace fitcensys.Models
{
    public enum Gender
    {
        Male,       // Veritabanına 0 olarak kaydeder
        Female,     // Veritabanına 1 olarak kaydeder       
    }
    public class Trainer
    {
        [Key]
        public int TrainerID { get; set; }

        [Required(ErrorMessage ="Ad alanı boş bırakılamaz.")]
        [Display(Name ="Ad")]
        [StringLength(50,MinimumLength =3,ErrorMessage ="Ad 3-50 karakter aralığında olmalı.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Soyad alanı boş bırakılamaz.")]
        [Display(Name = "Ad")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Ad 3-50 karakter aralığında olmalı.")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        
        [Required]
        [Display(Name = "Cinsiyet")]
        public Gender Gender{ get; set; }

        [Display(Name = "Biografi")]
        [StringLength(150)]
        public string? Biography { get; set; }

        [EmailAddress]
        [Display(Name = "E-Posta Adresi")]
        public string EmailAddress { get; set; }
        
        [Phone]
        [Display(Name = "Telefon Numarası")]
        public string Phone { get; set; }


        //Bir antrenör bir salona ait (Salona özgü bir uygulama sonuçta antrenörler için değil))
        public int GymID { get; set; }       
        public Gym Gym { get; set; }

        //public ICollection<TrainerSpeciality> Specialities { get; set;}
        //public ICollection<Appointment> Appointments { get; set; }
        //public ICollection<TrainerAvailability> Availabilities { get; set; }


    }
}
