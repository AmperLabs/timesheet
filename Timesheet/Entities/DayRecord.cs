using Timesheet.Common;

namespace Timesheet.Entities
{
    public class DayRecord
    {
        public string Id { get; set; }
        public TimeSpan DailyRegularWorkingTime { get; set; }
        public TimeSpan MaximumDailyWorkingTime { get; set; }
        public DateOnly Date { get; set; }
        public PresenceType PresenceType { get; set; }
        public TimeOnly? StartOfWork { get; set; }
        public TimeOnly? EndOfWork { get; set; }
        public TimeSpan? MobileWork { get; set; }

        public static string GenerateKey(DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        public static string GenerateKey(DateOnly date)
        {
            return date.ToString("yyyyMMdd");
        }
    }
}
