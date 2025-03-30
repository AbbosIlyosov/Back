using System.ComponentModel.DataAnnotations;

namespace ServiCar.Domain.Entities
{
    public class Review: BaseModel
    {
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int PointId { get; set; }
        public int AppointmentId { get; set; }

        public User User { get; set; }
        public Point Point { get; set; }
        public Appointment Appointment { get; set; }

        public ICollection<Image> Images { get; set; }

    }
}
