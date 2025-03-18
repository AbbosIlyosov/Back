namespace ServiCar.Domain.Entities
{
    public class Image: BaseModel
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}
