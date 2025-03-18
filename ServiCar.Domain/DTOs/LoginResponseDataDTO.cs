namespace ServiCar.Domain.DTOs
{
    public class LoginResponseDataDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDTO User { get; set; }
    }
}
