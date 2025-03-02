using System.Globalization;
using Timesheet.Common;

namespace Timesheet.Data
{
    public class TimesheetWeek
    {
        public string UserId { get; set; }
        public int Year { get; init; }
        public int WeekOfYear { get; init; }

        public List<TimesheetDay> BookedDays { get; set; } = new List<TimesheetDay>();

        private TimeSpan? _bookedMobileWork = null;
        public TimeSpan? BookedMobileWork
        {
            get => _bookedMobileWork;
            set
            {
                if(_bookedMobileWork != value)
                {
                    _bookedMobileWork = value;
                    IsDirty = true;
                }
            }
        }
        
        private DateOnly? _bookedForDate = null;
        public DateOnly? BookedForDate
        {
            get => _bookedForDate;
            set
            {
                if (_bookedForDate != value)
                {
                    _bookedForDate = value;
                    IsDirty = true;
                }
            }
        }

        public bool IsDirty { get; private set; }
        public void ResetDirtyFlag()
        {
            IsDirty = false;
        }

        public TimeSpan TotalWorkingTimeInPresence => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.NetWorkingTimeInPresence.HasValue).Select(x => x.NetWorkingTimeInPresence!.Value.TotalSeconds).Sum());
        public TimeSpan TotalMobileWork => BookedDays == null ? TimeSpan.Zero : TotalPartlyMobileWork + TotalFullyMobileWork;
        public TimeSpan TotalPartlyMobileWork => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.PartlyMobileWork.HasValue).Select(x => x.PartlyMobileWork!.Value.TotalSeconds).Sum());
        public TimeSpan TotalFullyMobileWork => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.PresenceType == PresenceType.MobileOnly).Select(x => x.TotalWorkingTime!.Value.TotalSeconds).Sum());

        public int TotalFullyMobileWorkDays => BookedDays == null ? 0 : BookedDays.Where(x => x.PresenceType == PresenceType.MobileOnly).Count();
        public TimeSpan TotalWorkingTime => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.TotalWorkingTime.HasValue).Select(x => x.TotalWorkingTime!.Value.TotalSeconds).Sum());
        public TimeSpan OvertimeHours => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.OvertimeHours != null).Select(x => x.OvertimeHours.TotalSeconds).Sum());
        public TimeSpan UnbookedMobileWork => BookedMobileWork.HasValue ? TotalPartlyMobileWork - BookedMobileWork.Value : TotalPartlyMobileWork;

        public double? MobileWorkShare => TotalWorkingTime.TotalSeconds > 0 ? TotalMobileWork.TotalSeconds / TotalWorkingTime.TotalSeconds : null;
        public double? PresenceWorkShare => TotalWorkingTime.TotalSeconds > 0 ? TotalWorkingTimeInPresence.TotalSeconds / TotalWorkingTime.TotalSeconds : null;

        public TimesheetWeek()
        {
            Year = DateTime.Today.Year;
            WeekOfYear = ISOWeek.GetWeekOfYear(DateTime.Today);
        }

        public static TimesheetWeek FromCalendarWeek(int year, int weekOfYear)
        {
            return new TimesheetWeek
            {
                Year = year,
                WeekOfYear = weekOfYear
            };
        }
    }
}
