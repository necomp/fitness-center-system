using System.ComponentModel.DataAnnotations;

namespace fitcensys.Models
{
    public class TrainerService
    {
        // bu tablonun primary key i yok 
        // TrainerID ve ServiceID Composite key oluşturur
        public int TrainerID { get; set; }
        public Trainer Trainer { get; set; }

        public int ServiceDefinitionID { get; set; }
        public ServiceDefinition ServiceDefinition { get; set; }
    }

}
