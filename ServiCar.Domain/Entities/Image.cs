namespace ServiCar.Domain.Entities
{
    public class Image : BaseModel
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public int ReviewId { get; set; }

        public Business Business { get; set; }
        public Review Review { get; set; }
    }
}
