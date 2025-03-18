using System.ComponentModel.DataAnnotations;

namespace ServiCar.Domain.Entities
{
    public class Review: BaseModel
    {
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public ICollection<string> Images { get; set; }
        public User User { get; set; }
        public int PointId { get; set; }
        public Appointment Appointment { get; set; }
    }
}
