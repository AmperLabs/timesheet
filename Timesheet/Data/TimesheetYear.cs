using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Timesheet.Data
{
    public class TimesheetYear
    {
        public string UserId { get; set; }
        public int Year { get; set; }

        public int NumberOfWeeksInYear { get; set; }

        public int NumberOfLeaveDays { get; set; } = 30;

        public TimeSpan InitialOvertimeHours { get; set; } = TimeSpan.Zero;
    
    }
}
