using MongoDB.Driver;
using System.Globalization;

namespace Timesheet.Entities
{
    public class WeekRecord
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public static string GenerateKey(DateTime date)
        {
            return GenerateKey(date.Year, ISOWeek.GetWeekOfYear(date));
        }

        public static string GenerateKey(DateOnly date)
        {
            return GenerateKey(date.Year, ISOWeek.GetWeekOfYear(date.ToDateTime(new TimeOnly(0, 0, 0))));
        }

        public static string GenerateKey(int year, int weekNumber)
        {
            return $"{year}{weekNumber:D2}";
        }
    }
}
