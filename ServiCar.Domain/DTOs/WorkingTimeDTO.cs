namespace ServiCar.Domain.DTOs
{
    public class WorkingTimeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

    public class WorkingTimeCreateDTO
    {
        public string Name { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
