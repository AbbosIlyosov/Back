namespace ServiCar.Domain.DTOs
{
    public  class TokenDTO
    {
        public string AccessToken { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}
