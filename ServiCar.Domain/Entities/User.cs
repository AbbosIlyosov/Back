

using Microsoft.AspNetCore.Identity;

namespace ServiCar.Domain.Entities
{
    public class User: IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsCompanyWorker { get; set; } = false;
    }
}
