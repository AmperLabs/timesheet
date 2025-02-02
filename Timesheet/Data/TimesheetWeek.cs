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
        public TimeSpan TotalMobileWork => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.MobileWork.HasValue).Select(x => x.MobileWork!.Value.TotalSeconds).Sum() + BookedDays.Where(x => x.PresenceType == PresenceType.MobileOnly).Select(x => x.TotalWorkingTime!.Value.TotalSeconds).Sum() );
        public TimeSpan TotalWorkingTime => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.TotalWorkingTime.HasValue).Select(x => x.TotalWorkingTime!.Value.TotalSeconds).Sum());
        public TimeSpan OvertimeHours => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.OvertimeHours.HasValue).Select(x => x.OvertimeHours!.Value.TotalSeconds).Sum());
        public TimeSpan UnbookedMobileWork => BookedMobileWork.HasValue ? TotalMobileWork - BookedMobileWork.Value : TotalMobileWork;

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
