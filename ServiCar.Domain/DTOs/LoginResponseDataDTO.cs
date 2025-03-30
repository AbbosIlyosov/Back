namespace ServiCar.Domain.DTOs
{
    public class LoginResponseDataDTO
    {
        public string AccessToken { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int UserId { get; set; }

        public List<string> Roles { get; set; }
    }
}
