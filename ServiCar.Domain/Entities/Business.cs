using ServiCar.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiCar.Domain.Models
{
    public class Business : BaseModel
    {
        public string Name { get; set; }
        public Image? Image { get; set; }
        public string AboutUs { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int PointsCount { get; set; } // auto update after each point insert for this business
    }
}
