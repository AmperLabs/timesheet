using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Timesheet.Common;

namespace Timesheet.Data
{
    public class TimesheetYear
    {
        public string UserId { get; set; }
        public int Year { get; set; }

        private TimeSpan? _initialOvertimeHours = TimeSpan.Zero;
        public TimeSpan? InitialOvertimeHours
        {
            get => _initialOvertimeHours;
            set
            {
                if(value != _initialOvertimeHours)
                {
                    _initialOvertimeHours = value;
                    IsDirty = true;
                }
            }
        }

        private int? _vacationEntitlement = 0;
        public int? VacationEntitlement
        {
            get => _vacationEntitlement;
            set
            {
                if (value != _vacationEntitlement)
                {
                    _vacationEntitlement = value;
                    IsDirty = true;
                }
            }
        }

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

        public int TotalFullyMobileWorkDays => BookedDays == null ? 0 : BookedDays.Where(x => x.PresenceType == PresenceType.MobileOnly).Count();

        public TimeSpan TotalWorkingTime => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.TotalWorkingTime.HasValue).Select(x => x.TotalWorkingTime!.Value.TotalSeconds).Sum());
        public TimeSpan OvertimeHours => BookedDays == null ? TimeSpan.Zero : TimeSpan.FromSeconds(BookedDays.Where(x => x.OvertimeHours != null).Select(x => x.OvertimeHours.TotalSeconds).Sum());

        public TimeSpan TotalOvertimeHours => OvertimeHours + (InitialOvertimeHours ?? TimeSpan.Zero);
        
        //public TimeSpan UnbookedMobileWork => BookedMobileWork.HasValue ? TotalPartlyMobileWork - BookedMobileWork.Value : TotalPartlyMobileWork;

        // TODO: sinnvoll implementieren
        public TimeSpan UnbookedMobileWork => TimeSpan.Zero;

        public double? MobileWorkShare => TotalWorkingTime.TotalSeconds > 0 ? TotalMobileWork.TotalSeconds / TotalWorkingTime.TotalSeconds : null;
        public double? PresenceWorkShare => TotalWorkingTime.TotalSeconds > 0 ? TotalWorkingTimeInPresence.TotalSeconds / TotalWorkingTime.TotalSeconds : null;

        public int NumberOfVacationDaysBooked
        {
            get
            {
                return BookedDays.Where(x => x.PresenceType == PresenceType.Vacation).Count();
            }
        }

        public int NumberOfVacationDaysTaken
        {
            get
            {
                return BookedDays.Where(x => x.PresenceType == PresenceType.Vacation && x.DateTime <= DateTime.Today).Count();
            }
        }

        public int NumberOfVacationDaysNotTaken
        {
            get
            {
                return BookedDays.Where(x => x.PresenceType == PresenceType.Vacation && x.DateTime > DateTime.Today).Count();
            }
        }

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
