using System.ComponentModel.DataAnnotations;

namespace fitcensys.Models
{
    public class Gym
    {
        [Key]
        public int GymID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        
        //public ICollection<GymWorkingHour> WorkingHours { get; set; }

        //public ICollection<GymService> Services { get; set; }

        //public ICollection<Trainer> Trainers { get; set; }
    }
}
