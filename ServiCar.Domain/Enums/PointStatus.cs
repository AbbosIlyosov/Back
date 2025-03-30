using System.ComponentModel;

namespace ServiCar.Domain.Enums
{
    public enum PointStatus
    {
        [Description("Check")]
        Check = 1,
        [Description("Hide")]
        Hide = 2,
        [Description("Visible")]
        Visible = 3
    }
}
