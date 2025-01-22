namespace Timesheet.Common
{
    public static class DateTimeExtensions
    {
        public static bool IsSameDay(this DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.DayOfYear == date2.DayOfYear;
        }
    }
}
