using ServiCar.Domain.Enums;

namespace ServiCar.Domain.Entities
{
    public class Point : BaseModel
    {
        public string PointName { get; set; }
        public bool IsAppointmentAvailable { get; set; }
        public PointStatus PointStatusId { get; set; }
        public int LocationId { get; set; }
        public int BusinessId { get; set; }
        public int WorkingTimeId { get; set; }
        public int UserId { get; set; }

        public Location Location { get; set; }
        public Business Business { get; set; }
        public WorkingTime WorkingTime { get; set; }
        public User User { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
