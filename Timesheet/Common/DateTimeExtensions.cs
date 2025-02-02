using System.Globalization;

namespace Timesheet.Common
{
    public static class DateTimeExtensions
    {
        public static bool IsSameDay(this DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.DayOfYear == date2.DayOfYear;
        }

        public static string GetDayName(this DayOfWeek value)
        {
            return DateTimeFormatInfo.CurrentInfo.GetDayName(value);
        }

        public static string GetDayName(this DayOfWeek value, CultureInfo cultureInfo)
        {
            return cultureInfo.DateTimeFormat.GetDayName(value);
        }

        public static string GetAbbreviatedDayName(this DayOfWeek value)
        {
            return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(value);
        }

        public static string GetAbbreviatedDayName(this DayOfWeek value, CultureInfo cultureInfo)
        {
            return cultureInfo.DateTimeFormat.GetAbbreviatedDayName(value);
        }
    }
}
