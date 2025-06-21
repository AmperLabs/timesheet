using Riok.Mapperly.Abstractions;
using Timesheet.Entities;

namespace Timesheet.Data
{
    [Mapper]
    public partial class TimesheetMapper
    {
        [MapperIgnoreSource(nameof(TimesheetDay.DateTime))]
        [MapperIgnoreSource(nameof(TimesheetDay.NetWorkingTimeInPresence))]
        [MapperIgnoreSource(nameof(TimesheetDay.TotalWorkingTime))]
        [MapperIgnoreSource(nameof(TimesheetDay.OvertimeHours))]
        [MapperIgnoreSource(nameof(TimesheetDay.IsMaximumDailyWorkingTimeExceeded))]
        [MapperIgnoreTarget(nameof(DayRecord.Id))]
        public partial DayRecord TimesheetDayToDayRecord(TimesheetDay timesheetDay);

        [MapperIgnoreSource(nameof(dayRecord.Id))]
        public partial TimesheetDay DayRecordToTimesheetDay(DayRecord dayRecord);

        [MapperIgnoreSource(nameof(TimesheetWeek.TotalWorkingTimeInPresence))]
        [MapperIgnoreSource(nameof(TimesheetWeek.TotalMobileWork))]
        [MapperIgnoreSource(nameof(TimesheetWeek.TotalWorkingTime))]
        [MapperIgnoreSource(nameof(TimesheetWeek.OvertimeHours))]
        [MapperIgnoreSource(nameof(TimesheetWeek.UnbookedMobileWork))]
        [MapperIgnoreSource(nameof(TimesheetWeek.MobileWorkShare))]
        [MapperIgnoreSource(nameof(TimesheetWeek.PresenceWorkShare))]
        public partial WeekRecord TimesheetWeekToWeekRecord(TimesheetWeek timesheetWeek);

        [MapperIgnoreSource(nameof(WeekRecord.Id))]
        public partial TimesheetWeek WeekRecordToTimesheetWeek(WeekRecord weekRecord);


        [MapperIgnoreSource(nameof(TimesheetYear.TotalWorkingTimeInPresence))]
        [MapperIgnoreSource(nameof(TimesheetYear.TotalMobileWork))]
        [MapperIgnoreSource(nameof(TimesheetYear.TotalWorkingTime))]
        [MapperIgnoreSource(nameof(TimesheetYear.OvertimeHours))]
        [MapperIgnoreSource(nameof(TimesheetYear.UnbookedMobileWork))]
        [MapperIgnoreSource(nameof(TimesheetYear.MobileWorkShare))]
        [MapperIgnoreSource(nameof(TimesheetYear.PresenceWorkShare))]
        public partial YearRecord TimesheetYearToYearRecord(TimesheetYear timesheetYear);

        [MapperIgnoreSource(nameof(YearRecord.Id))]
        public partial TimesheetYear YearRecordToTimesheetYear(YearRecord yearRecord);
    }
}
