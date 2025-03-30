using ServiCar.Domain.Enums;

namespace ServiCar.Domain.Entities
{
    public class Business : BaseModel
    {
        public string Name { get; set; }
        public int? ImageId { get; set; }
        public string AboutUs { get; set; }
        public int PointsCount { get; set; } // auto update after each point insert for this business
        public BusinessStatus BusinessStatusId { get; set; } = BusinessStatus.Active;
        public Image Image { get; set; }
        public ICollection<Point> Points { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<User> Workers { get; set; }
    }
}
