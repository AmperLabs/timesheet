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
    }
}
