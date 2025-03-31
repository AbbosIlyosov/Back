namespace ServiCar.Domain.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsCompanyWorker { get; set; }
    }

    public class UserRegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }

    public class UserUpdateDTO : UserDTO
    {
        
    }
}
