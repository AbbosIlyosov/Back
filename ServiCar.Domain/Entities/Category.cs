namespace ServiCar.Domain.Entities
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public ICollection<Point> Points { get; set; }
    }
}
