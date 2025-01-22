using System.Globalization;
using Timesheet.Common;

namespace Timesheet.Data
{
    public class TimesheetWeek
    {
        public int Year { get; init; }
        public int WeekOfYear { get; init; }

        public List<TimesheetDay> Days { get; set; }

        public TimeSpan TotalWorkingTimeInPresence => TimeSpan.FromSeconds(Days.Where(x => x.WorkingTimeInPresence.HasValue).Select(x => x.WorkingTimeInPresence!.Value.TotalSeconds).Sum());
        public TimeSpan TotalMobileWork => TimeSpan.FromSeconds(Days.Where(x => x.MobileWork.HasValue).Select(x => x.MobileWork!.Value.TotalSeconds).Sum());
        public TimeSpan TotalWorkingTime => TimeSpan.FromSeconds(Days.Where(x => x.TotalWorkingTime.HasValue).Select(x => x.TotalWorkingTime!.Value.TotalSeconds).Sum());
        public TimeSpan OvertimeHours => TimeSpan.FromSeconds(Days.Where(x => x.OvertimeHours.HasValue).Select(x => x.OvertimeHours!.Value.TotalSeconds).Sum());

        public double? MobileWorkShare => TotalWorkingTime.TotalSeconds > 0 ? TotalMobileWork.TotalSeconds / TotalWorkingTime.TotalSeconds : null;
        public double? PresenceWorkShare => TotalWorkingTime.TotalSeconds > 0 ? TotalWorkingTimeInPresence.TotalSeconds / TotalWorkingTime.TotalSeconds : null;

        public TimesheetWeek(int year, int weekOfYear)
        {
            Year = year;
            WeekOfYear = weekOfYear;
        }
    }
}
