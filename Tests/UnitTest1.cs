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
        [InlineData(2025, 1, 2024, 12, 30)]
        [InlineData(2025, 2, 2025, 1, 6)]
        [InlineData(2025, 52, 2025, 12, 22)]
        public void CanGetCalendarWeekMonday(int year, int cw, int expectedY, int expectedM, int expectedD)
        {
            var monday = System.Globalization.ISOWeek.ToDateTime(year, cw, DayOfWeek.Monday);
            var expectedMonday = new DateTime(expectedY, expectedM, expectedD);

            Assert.True(monday == expectedMonday);
        }

        [Fact]
        public void CanMapTimesheetDayToDayRecord()
        {
            var timesheetDay = new TimesheetDay
            {
                DailyRegularWorkingTime = new TimeSpan(8, 0, 0),
                MaximumDailyWorkingTime = new TimeSpan(10, 0, 0),
                Date = new DateOnly(2025, 1, 8),
                PresenceType = PresenceType.MobilePartly,
                StartOfWork = new TimeOnly(07, 0, 0),
                EndOfWork = new TimeOnly(16, 0, 0),
                PartlyMobileWork = new TimeSpan(1, 30, 0)
            };

            var mapper = new TimesheetMapper();
            var dayRecord = mapper.TimesheetDayToDayRecord(timesheetDay);

            var mappedTimesheetDay = mapper.DayRecordToTimesheetDay(dayRecord);

        }
    }
}