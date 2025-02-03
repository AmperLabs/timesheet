﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Timesheet.Common;

namespace Timesheet.Data
{
    public class TimesheetYear
    {
        public string UserId { get; set; }
        public int Year { get; set; }

        public TimeSpan? InitialOvertimeHours { get; set; } = TimeSpan.Zero;

        public bool IsDirty { get; private set; }
        public void ResetDirtyFlag()
        {
            IsDirty = false;
        }

        public List<TimesheetDay> BookedDays { get; set; } = new List<TimesheetDay>();

        public TimeSpan TotalWorkingTimeInPresence => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.NetWorkingTimeInPresence.HasValue).Select(x => x.NetWorkingTimeInPresence!.Value.TotalSeconds).Sum());
        public TimeSpan TotalMobileWork => BookedDays == null ? TimeSpan.Zero : TotalPartlyMobileWork + TotalFullyMobileWork;
        public TimeSpan TotalPartlyMobileWork => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.PartlyMobileWork.HasValue).Select(x => x.PartlyMobileWork!.Value.TotalSeconds).Sum());
        public TimeSpan TotalFullyMobileWork => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.PresenceType == PresenceType.MobileOnly).Select(x => x.TotalWorkingTime!.Value.TotalSeconds).Sum());

        public TimeSpan TotalWorkingTime => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.TotalWorkingTime.HasValue).Select(x => x.TotalWorkingTime!.Value.TotalSeconds).Sum());
        public TimeSpan OvertimeHours => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.OvertimeHours.HasValue).Select(x => x.OvertimeHours!.Value.TotalSeconds).Sum());
        //public TimeSpan UnbookedMobileWork => BookedMobileWork.HasValue ? TotalPartlyMobileWork - BookedMobileWork.Value : TotalPartlyMobileWork;

        // TODO: sinnvoll implementieren
        public TimeSpan UnbookedMobileWork => TimeSpan.Zero;

        public double? MobileWorkShare => TotalWorkingTime.TotalSeconds > 0 ? TotalMobileWork.TotalSeconds / TotalWorkingTime.TotalSeconds : null;
        public double? PresenceWorkShare => TotalWorkingTime.TotalSeconds > 0 ? TotalWorkingTimeInPresence.TotalSeconds / TotalWorkingTime.TotalSeconds : null;

        public TimesheetYear()
        {
            Year = DateTime.Today.Year;
        }

        public static TimesheetYear FromYear(int year)
        {
            return new TimesheetYear
            {
                Year = year
            };
        }

        public static TimesheetYear FromDate(DateTime date)
        {
            return new TimesheetYear
            {
                Year = date.Year
            };
        }
    }
}
