namespace ServiCar.Domain.Entities
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public int BusinessId { get; set; }
        public ICollection<Business> Businesses { get; set; }
        public ICollection<Point> Points { get; set; }
    }
}
