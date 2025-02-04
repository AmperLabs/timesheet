using Timesheet.Common;

namespace Timesheet.Entities
{
    public class YearRecord
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public TimeSpan? InitialOvertimeHours { get; set; }

        public int? VacationEntitlement { get; set; }
        public static string GenerateKey(int year, string userId)
        {
            var normalizedUserId = userId.NormalizeEmail();

            return $"{normalizedUserId}-{year}";
        }
    }
}
