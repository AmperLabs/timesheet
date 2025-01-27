using System.Globalization;
using Timesheet.Common;

namespace Timesheet.Data
{
    public class TimesheetWeek
    {
        public string UserId { get; set; }
        public int Year { get; init; }
        public int WeekOfYear { get; init; }

        public List<TimesheetDay> BookedDays { get; set; }

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

        public TimeSpan TotalWorkingTimeInPresence => TimeSpan.FromSeconds(BookedDays.Where(x => x.WorkingTimeInPresence.HasValue).Select(x => x.WorkingTimeInPresence!.Value.TotalSeconds).Sum());
        public TimeSpan TotalMobileWork => TimeSpan.FromSeconds(BookedDays.Where(x => x.MobileWork.HasValue).Select(x => x.MobileWork!.Value.TotalSeconds).Sum());
        public TimeSpan TotalWorkingTime => TimeSpan.FromSeconds(BookedDays.Where(x => x.TotalWorkingTime.HasValue).Select(x => x.TotalWorkingTime!.Value.TotalSeconds).Sum());
        public TimeSpan OvertimeHours => TimeSpan.FromSeconds(BookedDays.Where(x => x.OvertimeHours.HasValue).Select(x => x.OvertimeHours!.Value.TotalSeconds).Sum());
        public TimeSpan UnbookedMobileWork => BookedMobileWork.HasValue ? TotalMobileWork - BookedMobileWork.Value : TotalMobileWork;

        public double? MobileWorkShare => TotalWorkingTime.TotalSeconds > 0 ? TotalMobileWork.TotalSeconds / TotalWorkingTime.TotalSeconds : null;
        public double? PresenceWorkShare => TotalWorkingTime.TotalSeconds > 0 ? TotalWorkingTimeInPresence.TotalSeconds / TotalWorkingTime.TotalSeconds : null;

        public TimesheetWeek(int year, int weekOfYear)
        {
            Year = year;
            WeekOfYear = weekOfYear;
        }
    }
}
