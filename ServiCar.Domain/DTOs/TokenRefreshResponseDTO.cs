namespace ServiCar.Domain.DTOs
{
    public class TokenRefreshResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
