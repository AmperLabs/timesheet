using Timesheet.Common;

namespace Timesheet.Entities
{
    public class DayRecord
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public TimeSpan DailyRegularWorkingTime { get; set; }
        public TimeSpan MaximumDailyWorkingTime { get; set; }
        public DateOnly Date { get; set; }
        public PresenceType PresenceType { get; set; }
        public TimeOnly? StartOfWork { get; set; }
        public TimeOnly? EndOfWork { get; set; }
        public TimeSpan? PartlyMobileWork { get; set; }
        public string? Comment { get; set; }

        public static string GenerateKey(DateTime date, string userId)
        {
            return GenerateKey(DateOnly.FromDateTime(date), userId);
        }

        public static string GenerateKey(DateOnly date, string userId)
        {
            var normalizedUserId = userId.NormalizeEmail();
            var normalizedDate = date.ToString("yyyyMMdd");

            return $"{normalizedUserId}-{normalizedDate}";
        }
    }
}
