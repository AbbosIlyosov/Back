using System.ComponentModel;

namespace ServiCar.Domain.Enums
{
    public enum BusinessStatus
    {
        [Description("Active")]
        Active = 1,
        [Description("Deactivated")]
        Deactivated = 2
    }
}
