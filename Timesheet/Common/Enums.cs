using System.ComponentModel;

namespace Timesheet.Common
{
    public enum PresenceType
    {
        [Description("Undefiniert")]
        Undefined,
        [Description("Nur vor Ort")]
        PresenceOnly,
        [Description("Anteilig Mobil")]
        MobilePartly,
        [Description("Nur Mobil")]
        MobileOnly,
        [Description("Urlaub")]
        Vacation,
        [Description("Feiertag")]
        PublicHoliday,
        [Description("Krank")]
        Illness,
        [Description("Abbau Überstunden")]
        ReduceOverhours
    }
}
