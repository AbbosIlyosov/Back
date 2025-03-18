using ServiCar.Domain.Enums;

namespace ServiCar.Domain.Entities
{
    public class Appointment: BaseModel
    {
        public DateTime AppointmentTime { get; set; }
        public AppointmentStatus AppointmentStatusId { get; set; }
        public int OrderNumber { get; set; }
        public User CreatedBy { get; set; }
        public int PointId { get; set; }

    }
}
