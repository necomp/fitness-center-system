using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitcensys.Models
{
    public class TrainerService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainerServiceID { get; set; }
       
        [Display(Name="Eğitmen")]
        public int TrainerID { get; set; }
        public Trainer? Trainer { get; set; }

        [Display(Name ="Hizmet Adı")]
        public int ServiceDefinitionID { get; set; }
        public ServiceDefinition? ServiceDefinition { get; set; }
    }

}
