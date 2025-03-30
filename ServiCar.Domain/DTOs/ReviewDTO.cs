using System.ComponentModel.DataAnnotations;

namespace ServiCar.Domain.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public int PointId { get; set; }
        public string Point { get; set; }
        public int AppointmentId { get; set; }
    }

    public class ReviewCreateDTO
    {
        [MinLength(1, ErrorMessage = "Comment cannot be empty.")]
        //[RegularExpression(@"\S+", ErrorMessage = "Comment cannot be empty or whitespace.")]
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "UserId is not valid.")]
        public int UserId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "PointId is not valid.")]
        public int PointId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "AppointmentId is not valid.")]
        public int AppointmentId { get; set; }
    }
}
