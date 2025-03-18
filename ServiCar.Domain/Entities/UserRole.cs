using Microsoft.AspNetCore.Identity;

namespace ServiCar.Domain.Entities
{
    public class UserRole: IdentityUserRole<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
