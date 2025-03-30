using ServiCar.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiCar.Domain.Entities
{
    public class Appointment: BaseModel
    {
        public DateTime AppointmentTime { get; set; }
        public AppointmentStatus AppointmentStatusId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Order Number must be greater than or equal to 1.")]
        public int OrderNumber { get; set; }
        public int UserId { get; set; }
        public int PointId { get; set; }
        public User User { get; set; }
        public Point Point { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
