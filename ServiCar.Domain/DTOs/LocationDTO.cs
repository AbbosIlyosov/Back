namespace ServiCar.Domain.DTOs
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class LocationCreateDTO
    {
        public string City { get; set; }
        public string District { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
