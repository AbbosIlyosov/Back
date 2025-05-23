﻿using ServiCar.Domain.Entities;
using ServiCar.Domain.Enums;
using System.ComponentModel;

namespace ServiCar.Domain.DTOs
{
    public class BusinessDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int? ImageId { get; set; }
        public string AboutUs { get; set; }
        public BusinessStatus StatusId { get; set; }
        public string Status => GetStatusDescription(StatusId);
        public int PointsCount { get; set; }

        public byte[]? Image { get; set; }

        private string GetStatusDescription(BusinessStatus pointStatus)
        {
            var type = typeof(BusinessStatus);
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

    public class BusinessGridInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
    }

    public class BusinessCreateDTO
    {
        public string Name { get; set; }
        public string AboutUs { get; set; }
        public byte[] Image { get; set; }
        public ICollection<CategoryDTO> Categories { get; set; }
    }

    public class BusinessUpdateDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ImageId { get; set; }
        public string? AboutUs { get; set; }
    }

    public class BusinessStatusUpdateDTO
    {
        public int Id { get; set; }
        public BusinessStatus BusinessStatusId { get; set; }
    }

    public class BusinessSelectListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
