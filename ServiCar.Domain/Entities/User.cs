

using Microsoft.AspNetCore.Identity;

namespace ServiCar.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsCompanyWorker { get; set; } = false;
        public int? BusinessId { get; set; }

        public Business Business { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Point> Points { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
