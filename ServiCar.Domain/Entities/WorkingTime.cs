namespace ServiCar.Domain.Entities
{
    public class WorkingTime : BaseModel
    {
        public string Name { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public ICollection<Point> Points { get; set; }
    }
}
