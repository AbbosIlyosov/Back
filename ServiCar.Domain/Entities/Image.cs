namespace ServiCar.Domain.Entities
{
    public class Image : BaseModel
    {
        public string FileName { get; set; } = $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid()}.jpg";
        public byte[] FileData { get; set; }
        public int? ReviewId { get; set; }

        public Business Business { get; set; }
        public Review Review { get; set; }
    }
}
