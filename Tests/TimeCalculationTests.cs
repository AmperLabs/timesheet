using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheet.Common;
using Timesheet.Data;
using Shouldly;

namespace Tests
{
    public class TimeCalculationTests
    {
        #region Daily calculations
        [Fact]
        public void PresenceOnlyWithRegularWorkingTime()
        {
            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday

            var regularWorkingTime = day.DailyRegularWorkingTime;
            var expectedBreak = TimeSpan.FromHours(1);

            day.PresenceType = PresenceType.PresenceOnly;
            day.StartOfWork = new TimeOnly(8,0,0);
            day.EndOfWork = TimeOnly.FromTimeSpan(day.StartOfWork.Value.ToTimeSpan() + regularWorkingTime + expectedBreak);

            day.IsMobileWorkAllowed.ShouldBe(false);
            day.IsPresenceWorkAllowed.ShouldBe(true);
            day.StartOfWork.ShouldNotBeNull();
            day.EndOfWork.ShouldNotBeNull();
            day.MobileWork.ShouldBeNull();


            day.NetWorkingTimeInPresence.ShouldBe(regularWorkingTime);
            day.TotalWorkingTime.ShouldBe(regularWorkingTime);
            day.OvertimeHours.ShouldBe(TimeSpan.Zero);
            day.IsMaximumDialyWorkingTimeExceeded.ShouldBe(false);
        }

        [Fact]
        public void PresenceOnlyWithRegularWorkingTimePlus30MinutesOvertime()
        {
            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday

            var regularWorkingTime = day.DailyRegularWorkingTime;
            var expectedBreak = TimeSpan.FromHours(1);
            var overtime = TimeSpan.FromHours(0.5);

            day.PresenceType = PresenceType.PresenceOnly;
            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = TimeOnly.FromTimeSpan(day.StartOfWork.Value.ToTimeSpan() + regularWorkingTime + expectedBreak + overtime);

            day.IsMobileWorkAllowed.ShouldBe(false);
            day.IsPresenceWorkAllowed.ShouldBe(true);
            day.StartOfWork.ShouldNotBeNull();
            day.EndOfWork.ShouldNotBeNull();
            day.MobileWork.ShouldBeNull();

            day.NetWorkingTimeInPresence.ShouldBe(regularWorkingTime + overtime);
            day.TotalWorkingTime.ShouldBe(regularWorkingTime + overtime);
            day.OvertimeHours.ShouldBe(overtime);
            day.IsMaximumDialyWorkingTimeExceeded.ShouldBe(false);
        }

        [Fact]
        public void PresenceOnlyWithRegularWorkingTimePlus3HoursOvertime()
        {
            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday

            var regularWorkingTime = day.DailyRegularWorkingTime;
            var expectedBreak = TimeSpan.FromHours(1);
            var overtime = TimeSpan.FromHours(3);

            day.PresenceType = PresenceType.PresenceOnly;
            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = TimeOnly.FromTimeSpan(day.StartOfWork.Value.ToTimeSpan() + regularWorkingTime + expectedBreak + overtime);

            day.IsMobileWorkAllowed.ShouldBe(false);
            day.IsPresenceWorkAllowed.ShouldBe(true);
            day.StartOfWork.ShouldNotBeNull();
            day.EndOfWork.ShouldNotBeNull();
            day.MobileWork.ShouldBeNull();

            day.NetWorkingTimeInPresence.ShouldBe(regularWorkingTime + overtime);
            day.TotalWorkingTime.ShouldBe(regularWorkingTime + overtime);
            day.OvertimeHours.ShouldBe(overtime);
            day.IsMaximumDialyWorkingTimeExceeded.ShouldBe(true);
        }

        [Fact]
        public void PartlyMobile_7hPresence_1hMobile()
        {
            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday

            var presenceTime = TimeSpan.FromHours(7);
            var expectedBreak = TimeSpan.FromHours(1);
            var mobileWork = TimeSpan.FromHours(1);

            day.PresenceType = PresenceType.MobilePartly;
            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = TimeOnly.FromTimeSpan(day.StartOfWork.Value.ToTimeSpan() + presenceTime + expectedBreak);
            day.MobileWork = mobileWork;

            day.IsMobileWorkAllowed.ShouldBe(true);
            day.IsPresenceWorkAllowed.ShouldBe(true);
            day.StartOfWork.ShouldNotBeNull();
            day.EndOfWork.ShouldNotBeNull();
            day.MobileWork.ShouldNotBeNull();

            day.NetWorkingTimeInPresence.ShouldBe(presenceTime);
            day.TotalWorkingTime.ShouldBe(presenceTime + mobileWork);
            day.OvertimeHours.ShouldBe(TimeSpan.Zero);
            day.IsMaximumDialyWorkingTimeExceeded.ShouldBe(false);
        }

        [Fact]
        public void PartlyMobile_7hPresence_2hMobile()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            var presenceTime = TimeSpan.FromHours(7);
            var expectedBreak = TimeSpan.FromHours(1);
            var mobileWork = TimeSpan.FromHours(2);

            day.PresenceType = PresenceType.MobilePartly;
            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = TimeOnly.FromTimeSpan(day.StartOfWork.Value.ToTimeSpan() + presenceTime + expectedBreak);
            day.MobileWork = mobileWork;

            day.IsMobileWorkAllowed.ShouldBe(true);
            day.IsPresenceWorkAllowed.ShouldBe(true);
            day.StartOfWork.ShouldNotBeNull();
            day.EndOfWork.ShouldNotBeNull();
            day.MobileWork.ShouldNotBeNull();

            day.NetWorkingTimeInPresence.ShouldBe(presenceTime);
            day.TotalWorkingTime.ShouldBe(presenceTime + mobileWork);
            day.OvertimeHours.ShouldBe(presenceTime + mobileWork - regularWorkingTime);
            day.IsMaximumDialyWorkingTimeExceeded.ShouldBe(false);
        }

        [Fact]
        public void PartlyMobile_7hPresence_4hMobile()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            var presenceTime = TimeSpan.FromHours(7);
            var expectedBreak = TimeSpan.FromHours(1);
            var mobileWork = TimeSpan.FromHours(4);

            day.PresenceType = PresenceType.MobilePartly;
            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = TimeOnly.FromTimeSpan(day.StartOfWork.Value.ToTimeSpan() + presenceTime + expectedBreak);
            day.MobileWork = mobileWork;

            day.IsMobileWorkAllowed.ShouldBe(true);
            day.IsPresenceWorkAllowed.ShouldBe(true);
            day.StartOfWork.ShouldNotBeNull();
            day.EndOfWork.ShouldNotBeNull();
            day.MobileWork.ShouldNotBeNull();

            day.NetWorkingTimeInPresence.ShouldBe(presenceTime);
            day.TotalWorkingTime.ShouldBe(presenceTime + mobileWork);
            day.OvertimeHours.ShouldBe(presenceTime + mobileWork - regularWorkingTime);
            day.IsMaximumDialyWorkingTimeExceeded.ShouldBe(true);
        }

        [Fact]
        public void MobileOnly()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.MobileOnly;
            
            day.IsMobileWorkAllowed.ShouldBe(true);
            day.IsPresenceWorkAllowed.ShouldBe(false);
            day.StartOfWork.ShouldBeNull();
            day.EndOfWork.ShouldBeNull();
            day.MobileWork.ShouldBeNull();

            day.NetWorkingTimeInPresence.ShouldBeNull();
            day.TotalWorkingTime.ShouldBe(regularWorkingTime);
            day.OvertimeHours.ShouldBe(TimeSpan.Zero);
            day.IsMaximumDialyWorkingTimeExceeded.ShouldBe(false);
        }

        [Fact]
        public void MobileOnlyWithBookedTimes()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.MobileOnly;

            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.StartOfWork = new TimeOnly(9, 0, 0);
            day.MobileWork = TimeSpan.FromHours(1);

            day.IsMobileWorkAllowed.ShouldBe(true);
            day.IsPresenceWorkAllowed.ShouldBe(false);
            //day.StartOfWork.ShouldBeNull();
            //day.EndOfWork.ShouldBeNull();
            //day.MobileWork.ShouldBeNull();

            day.NetWorkingTimeInPresence.ShouldBeNull();
            day.TotalWorkingTime.ShouldBe(regularWorkingTime);
            day.OvertimeHours.ShouldBe(TimeSpan.Zero);
            day.IsMaximumDialyWorkingTimeExceeded.ShouldBe(false);
        }


        [Fact]
        public void BreakTimePresenceOnlyLT4h()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.PresenceOnly;

            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = new TimeOnly(9, 0, 0);

            day.BreakTime.ShouldBe(TimeSpan.FromMinutes(15));
        }

        [Fact]
        public void BreakTimePresenceOnlyGT4hLT6h()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.PresenceOnly;

            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = new TimeOnly(13, 0, 0);

            day.BreakTime.ShouldBe(TimeSpan.FromMinutes(30));
        }

        [Fact]
        public void BreakTimePresenceOnlyGT6h()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.PresenceOnly;

            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = new TimeOnly(15, 0, 0);

            day.BreakTime.ShouldBe(TimeSpan.FromMinutes(60));
        }

        [Fact]
        public void BreakTimeMobilePartlyLT4h()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.MobilePartly;

            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = new TimeOnly(9, 0, 0);

            day.BreakTime.ShouldBe(TimeSpan.FromMinutes(15));
        }

        [Fact]
        public void BreakTimeMobilePartlyGT4hLT6h()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.MobilePartly;

            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = new TimeOnly(13, 0, 0);

            day.BreakTime.ShouldBe(TimeSpan.FromMinutes(30));
        }

        [Fact]
        public void BreakTimeMobilePartlyGT6h()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.MobilePartly;

            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = new TimeOnly(15, 0, 0);

            day.BreakTime.ShouldBe(TimeSpan.FromMinutes(60));
        }


        [Fact]
        public void NegativeOvertime()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.MobilePartly;

            day.StartOfWork = new TimeOnly(8, 0, 0);
            day.EndOfWork = new TimeOnly(16, 0, 0);

            day.OvertimeHours.ShouldBe(TimeSpan.FromHours(-1));
        }

        [Fact]
        public void NegativeOvertimeIllness()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.Illness;

            day.OvertimeHours.ShouldBe(-regularWorkingTime);
        }

        [Fact]
        public void NegativeOvertimeVacation()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.Vacation;

            day.OvertimeHours.ShouldBe(TimeSpan.Zero);
        }

        [Fact]
        public void NegativeOvertimePublicHoliday()
        {
            var regularWorkingTime = TimeSpan.FromHours(8);

            var day = TimesheetDay.FromDateTime(new DateTime(2025, 1, 13)); // Monday
            day.DailyRegularWorkingTime = regularWorkingTime;

            day.PresenceType = PresenceType.Vacation;

            day.OvertimeHours.ShouldBe(TimeSpan.Zero);
        }
        #endregion

        #region Weekly calculations
        [Fact]
        public void CanCalculateWeek()
        {
            var week = TimesheetWeek.FromCalendarWeek(2025, 3);

            // Mon - Total 8 hrs
            week.BookedDays.Add(new TimesheetDay
            {
                Date = new DateOnly(2025, 1, 13),
                PresenceType = PresenceType.PresenceOnly,
                StartOfWork = new TimeOnly(8, 0, 0),
                EndOfWork = new TimeOnly(17, 0, 0)
            });

            // Tue - Total 7 hrs
            week.BookedDays.Add(new TimesheetDay
            {
                Date = new DateOnly(2025, 1, 14),
                PresenceType = PresenceType.MobilePartly,
                StartOfWork = new TimeOnly(8, 0, 0),
                EndOfWork = new TimeOnly(15, 0, 0),
                MobileWork = TimeSpan.FromHours(1)
            });

            // Wed - Total 8 hrs
            week.BookedDays.Add(new TimesheetDay
            {
                Date = new DateOnly(2025, 1, 15),
                PresenceType = PresenceType.MobileOnly
            });

            // Thu - Total 0 hrs
            week.BookedDays.Add(new TimesheetDay
            {
                Date = new DateOnly(2025, 1, 16),
                PresenceType = PresenceType.PublicHoliday
            });

            // Fri - Total 0 hrs
            week.BookedDays.Add(new TimesheetDay
            {
                Date = new DateOnly(2025, 1, 17),
                PresenceType = PresenceType.Vacation
            });


            week.TotalWorkingTime.ShouldBe(TimeSpan.FromHours(8 + 7 + 8));
            week.TotalWorkingTimeInPresence.ShouldBe(TimeSpan.FromHours(8 + 6));
            week.TotalMobileWork.ShouldBe(TimeSpan.FromHours(1 + 8));
        }
        #endregion
    }
}
