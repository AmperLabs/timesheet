using Microsoft.IdentityModel.Tokens;
using Timesheet.Common;

namespace Timesheet.Data
{
    public class TimesheetYearStatistics
    {
        private List<TimesheetDay> _days;

        public TimesheetYearStatistics(List<TimesheetDay> days)
        {
            _days = days;
        }

        public int NumberOfWorkdays
        {
            get
            {
                var workdayTypes = new List<PresenceType> { PresenceType.PresenceOnly, PresenceType.MobilePartly, PresenceType.MobileOnly };
                return _days.Where(x => workdayTypes.Any(t => t == x.PresenceType)).Count();
            }
        }

        public int NumberOfWorkdaysInPresence
        {
            get
            {
                return _days.Where(x => x.PresenceType == PresenceType.PresenceOnly).Count();
            }
        }

        public int NumberOfWorkdaysMobilePartly
        {
            get
            {
                return _days.Where(x => x.PresenceType == PresenceType.MobilePartly).Count();
            }
        }

        public int NumberOfWorkdaysMobileOnly
        {
            get
            {
                return _days.Where(x => x.PresenceType == PresenceType.MobileOnly).Count();
            }
        }

        public int NumberOfVacationDays
        {
            get
            {
                return _days.Where(x => x.PresenceType == PresenceType.Vacation).Count();
            }
        }

        public int NumberOfPublicHolidayDays
        {
            get
            {
                return _days.Where(x => x.PresenceType == PresenceType.PublicHoliday).Count();
            }
        }

        public int NumberOfIllnessDays
        {
            get
            {
                return _days.Where(x => x.PresenceType == PresenceType.Illness).Count();
            }
        }
    }
}
