namespace ServiCar.Domain.DTOs
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
