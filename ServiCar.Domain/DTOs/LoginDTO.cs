namespace ServiCar.Domain.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public DateTime tokenExpiry { get; set; }
        public UserDTO User { get; set; }
    }
}
