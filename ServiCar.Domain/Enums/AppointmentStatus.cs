using System.ComponentModel;

namespace ServiCar.Domain.Enums
{
    public enum AppointmentStatus
    {
        [Description("Created")]
        Created = 1,
        [Description("Cancelled")]
        Cancelled = 2,
        [Description("Completed")]
        Completed = 3,
        [Description("Accepted")]
        Accepted = 4,
        [Description("Rejected")]
        Rejected = 5
    }
}
