using Shouldly;
using System;
using Timesheet.Common;
using Timesheet.Data;
using Timesheet.Services;

namespace Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(2020, 53, 2019, 12, 30)]
        [InlineData(2021, 52, 2021, 1, 4)]
        [InlineData(2022, 52, 2022, 1, 3)]
        [InlineData(2023, 52, 2023, 1, 2)]
        [InlineData(2024, 52, 2024, 1, 1)]
        [InlineData(2025, 52, 2024, 12, 30)]
        public void TestISOWeek(int year, int expectedWeeks, int firstMondayYear, int firstMondayMonth, int firstMondayDay)
        {
            var startOfYear = System.Globalization.ISOWeek.GetYearStart(year);
            var endOfYear = System.Globalization.ISOWeek.GetYearEnd(year);
            var weeksInYear = System.Globalization.ISOWeek.GetWeeksInYear(year);
            var firstMonday = System.Globalization.ISOWeek.ToDateTime(year, 1, DayOfWeek.Monday);

            var expectedMonday = new DateTime(firstMondayYear, firstMondayMonth, firstMondayDay);

            Assert.True(weeksInYear == expectedWeeks);
            Assert.True(firstMonday == expectedMonday);
        }


        [Theory]
        [InlineData(1, 0, 0, "01:00")]
        [InlineData(14, 34, 27, "14:34")]
        [InlineData(44, 34, 27, "44:34")]
        public void CanFormatTimespanWithMoreThan24Hours(int h, int m, int s, string expectedString)
        {
            var timeSpan = new TimeSpan(h, m, s);

            var formattedTimespan = timeSpan.ToLongHoursString();

            formattedTimespan.ShouldBe(expectedString);
        }

    }
}