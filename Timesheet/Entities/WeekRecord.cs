using MongoDB.Driver;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Timesheet.Common;

namespace Timesheet.Entities
{
    public class WeekRecord
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public TimeSpan? BookedMobileWork { get; set; }
        public DateOnly? BookedForDate { get; set; }

        public static string GenerateKey(DateTime date, string userId)
        {
            return GenerateKey(date.Year, ISOWeek.GetWeekOfYear(date), userId);
        }
        public static string GenerateKey(DateOnly date, string userId)
        {
            return GenerateKey(date.Year, ISOWeek.GetWeekOfYear(date.ToDateTime(new TimeOnly(0, 0, 0))), userId);
        }
        public static string GenerateKey(int year, int weekNumber, string userId)
        {
            var normalizedUserId = userId.NormalizeEmail();

            return $"{normalizedUserId}-{year}{weekNumber:D2}";
        }
    }
}
