using System.Globalization;
using Timesheet.Common;

namespace Timesheet.Data
{
    public class TimesheetDay
    {
        public string UserId { get; set; }
        public bool IsDirty { get; private set; }
        public void ResetDirtyFlag()
        {
            IsDirty = false;
        }

        public TimeSpan DailyRegularWorkingTime { get; set; } = TimeSpan.FromHours(8);
        public TimeSpan MaximumDailyWorkingTime { get; set; } = TimeSpan.FromHours(10);

        public DateOnly Date { get; set; }
        public DateTime DateTime => Date.ToDateTime(new TimeOnly(0, 0, 0));
        public int Year => Date.Year;
        public int Month => Date.Month;
        public int CalendarWeek => ISOWeek.GetWeekOfYear(DateTime);

        private PresenceType _presenceType = PresenceType.Undefined;
        public PresenceType PresenceType
        {
            get => _presenceType;
            set
            {
                if (_presenceType != value)
                {
                    _presenceType = value;
                    IsDirty = true;
                }
            }
        }

        private TimeOnly? _startOfWork = null;
        public TimeOnly? StartOfWork
        {
            get => _startOfWork;
            set
            {
                if (_startOfWork != value)
                {
                    _startOfWork = value;
                    IsDirty = true;
                }
            }
        }
        
        private TimeOnly? _endOfWork = null;
        public TimeOnly? EndOfWork
        {
            get => _endOfWork;
            set
            {
                if (_endOfWork != value)
                {
                    _endOfWork = value;
                    IsDirty = true;
                }
            }        
        }

        private TimeSpan?_mobileWork = null;
        public TimeSpan? MobileWork
        {
            get => _mobileWork;
            set
            {
                if (_mobileWork != value)
                {
                    _mobileWork = value;
                    IsDirty = true;
                }
            }
        }

        public TimeSpan? WorkingTimeInPresence
        {
            get
            {
                var earliestPossibleStart = new TimeOnly(6, 0, 0);

                var start = StartOfWork;

                if (start.HasValue && start < earliestPossibleStart)
                    start = earliestPossibleStart;

                var timeIncludingBreaks = EndOfWork - start;

                // TODO: Pausenabzüge prüfen
                if(timeIncludingBreaks.HasValue)
                {
                    if (timeIncludingBreaks < TimeSpan.FromHours(4))
                        timeIncludingBreaks -= TimeSpan.FromMinutes(15);
                    else if (timeIncludingBreaks < TimeSpan.FromHours(6))
                        timeIncludingBreaks -= TimeSpan.FromMinutes(30);
                    else
                        timeIncludingBreaks -= TimeSpan.FromMinutes(60);
                }

                return timeIncludingBreaks;
            }
        }
        public TimeSpan? TotalWorkingTime
        {
            get
            {
                switch (PresenceType)
                {
                    case PresenceType.PresenceOnly:
                        return WorkingTimeInPresence;
                    case PresenceType.MobileOnly:
                        return DailyRegularWorkingTime;
                    case PresenceType.MobilePartly:
                        if (WorkingTimeInPresence == null && MobileWork == null)
                            return null;

                        var total = TimeSpan.Zero;
                        if (WorkingTimeInPresence != null)
                            total += WorkingTimeInPresence.Value;

                        if (MobileWork != null)
                            total += MobileWork.Value;
                        
                        return total;
                    case PresenceType.Vacation:
                        return TimeSpan.Zero;
                    case PresenceType.PublicHoliday:
                        return TimeSpan.Zero;
                    case PresenceType.Illness:
                        return TimeSpan.Zero;
                    default:
                        return null;
                }
            }
        }

        public TimeSpan? OvertimeHours => TotalWorkingTime - DailyRegularWorkingTime;
        public bool? IsMaximumDialyWorkingTimeExceeded => TotalWorkingTime > MaximumDailyWorkingTime;      

        public bool IsMobileWorkAllowed
        {
            get
            {
                switch(PresenceType)
                {
                    case PresenceType.MobileOnly:
                        return true;
                    case PresenceType.MobilePartly:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsPresenceWorkAllowed
        {
            get
            {
                switch (PresenceType)
                {
                    case PresenceType.PresenceOnly:
                        return true;
                    case PresenceType.MobilePartly:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public TimesheetDay(DateOnly? date = null)
        {
            if (date == null)
                date = DateOnly.FromDateTime(DateTime.Today);

            Date = date.Value;
        }

        public static TimesheetDay FromDateTime(DateTime date)
        {
            return new TimesheetDay(DateOnly.FromDateTime(date));
        }
    }
}
