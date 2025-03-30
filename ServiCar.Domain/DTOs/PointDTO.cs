using ServiCar.Domain.Entities;
using ServiCar.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiCar.Domain.DTOs
{
    public class PointDTO
    {
        public int Id { get; set; }
        public string PointName { get; set; }
        public bool IsAppointmentAvailable { get; set; }
        [NotMapped]
        public PointStatus PointStatusId { get; set; }
        public string Status => GetStatusDescription(PointStatusId);
        public CategoryDTO Category { get; set; }
        public LocationDTO Location { get; set; }
        public BusinessDTO Business { get; set; }
        public WorkingTimeDTO WorkingTime { get; set; }
        public UserDTO User { get; set; }

        private string GetStatusDescription(PointStatus pointStatus)
        {
            var type = typeof(PointStatus);
            var memberInfo = type.GetMember(pointStatus.ToString());

            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            // Fall back to the enum name if no description attribute is found
            return pointStatus.ToString();
        }
    }

    public class PointFilterDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "The filter property LocationId is required.")]
        public int LocationId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The filter property CategoryId is required.")]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The filter property BusinessId is required.")]
        public int BusinessId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The filter property StatusId is required.")]
        public PointStatus StatusId { get; set; }
    }

    public class CreatePointDTO
    {
        [MinLength(1, ErrorMessage = "PointName cannot be empty.")]
        //[RegularExpression(@"\S+", ErrorMessage = "PointName cannot be empty or whitespace.")]
        public string PointName { get; set; }
        public bool IsAppointmentAvailable { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The PointStatusId field is required.")]
        public PointStatus PointStatusId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The CategoryId field is required.")]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The LocationId field is required.")]
        public int LocationId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The BusinessId field is required.")]
        public int BusinessId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The WorkingTimeId field is required.")]
        public int WorkingTimeId { get; set; }

        //public CategoryCreateDTO Category { get; set; }
        //public LocationCreateDTO Location { get; set; }
        //public BusinessCreateDTO Business { get; set; }
        //public WorkingTimeCreateDTO WorkingTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The UserId field is required.")]
        public int UserId { get; set; }
    }

    public class UpdatePointDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "The Id field is required.")]
        public int Id { get; set; }

        [RegularExpression(@"\S+", ErrorMessage = "PointName cannot be empty or whitespace.")]
        public string? PointName { get; set; }
        public bool? IsAppointmentAvailable { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The PointStatusId should be greater than 0.")]
        public PointStatus? PointStatusId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The CategoryId should be greater than 0.")]
        public int? CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The LocationId should be greater than 0.")]
        public int? LocationId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The BusinessId should be greater than 0.")]
        public int? BusinessId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The WorkingTimeId should be greater than 0.")]
        public int? WorkingTimeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The UserId should be greater than 0.")]
        public int? UserId { get; set; }
    }
}
