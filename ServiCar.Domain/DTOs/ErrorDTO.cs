using System.Net;

namespace ServiCar.Domain.DTOs
{
    public class ErrorDTO
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
