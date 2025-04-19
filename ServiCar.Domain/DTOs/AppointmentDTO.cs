using ServiCar.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiCar.Domain.DTOs
{
    public class AppointmentCreateDTO
    {
        public DateTime AppointmentTime { get; set; }
        public int PointId { get; set; }
    }

    public class AppointmentUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public AppointmentStatus? StatusId { get; set; }
        public int? PointId { get; set; }
    }

    public class AppointmentDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string AppointmentTimeString => AppointmentTime.ToLongDateString();
        public AppointmentStatus StatusId { get; set; } = AppointmentStatus.Created;
        public int UserId { get; set; }
        public int PointId { get; set; }
        public int OrderNumber { get; set; }
        public string Status => GetStatusDescription(StatusId);
        public string ServiceType { get; set; }
        public string Address { get; set; }
        public string Point { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Avatar { get; set; }
        public bool HasReview { get; set; }

        private string GetStatusDescription(AppointmentStatus pointStatus)
        {
            var type = typeof(AppointmentStatus);
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

    public class AppointmentFilterDTO
    {
        public DateTime Date { get; set; }
        public int PointId { get; set; }
    }
}
