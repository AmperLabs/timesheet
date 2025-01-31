using Riok.Mapperly.Abstractions;
using Timesheet.Entities;

namespace Timesheet.Data
{
    [Mapper]
    public partial class TimesheetMapper
    {
        [MapperIgnoreSource(nameof(TimesheetDay.DateTime))]
        [MapperIgnoreSource(nameof(TimesheetDay.WorkingTimeInPresence))]
        [MapperIgnoreSource(nameof(TimesheetDay.TotalWorkingTime))]
        [MapperIgnoreSource(nameof(TimesheetDay.OvertimeHours))]
        [MapperIgnoreSource(nameof(TimesheetDay.IsMaximumDialyWorkingTimeExceeded))]
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
    }
}
