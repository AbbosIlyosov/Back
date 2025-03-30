namespace ServiCar.Domain.Entities
{
    public class Location: BaseModel
    {
        public string City { get; set; }
        public string District { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public ICollection<Point> Points { get; set; }
    }
}