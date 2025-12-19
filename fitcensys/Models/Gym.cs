using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace fitcensys.Models
{
    public class Gym
    {
        [Key]
        [Display(Name="Gym Adı")]
        public int GymID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        
        public ICollection<GymWorkingHour> WorkingHours { get; set; } = new List<GymWorkingHour>();

        public ICollection<GymService> GymServices { get; set; } = new List<GymService>();

        public ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
    }
}
