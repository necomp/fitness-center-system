using System.ComponentModel.DataAnnotations;

namespace fitcensys.Models
{
    public class ServiceDefinition
    {
        [Key]
        public int ServiceDefinitionID { get; set; }

        [Required(ErrorMessage = "Hizmet adı gereklidir.")]
        [StringLength(50)]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Bu hizmeti hangi salonlar veriyor?
        public ICollection<GymService> GymServices { get; set; }

        // Bu hizmeti hangi eğitmenler veriyor?
        public ICollection<TrainerService> TrainerServices { get; set; }
    }
}
