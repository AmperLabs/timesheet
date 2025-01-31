using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Globalization;
using System.Security.Authentication;
using Timesheet.Common;
using Timesheet.Data;
using Timesheet.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Timesheet.Services
{
    public class CalendarService
    {
        private string _databaseName = "";
        private string _timesheetDaysCollectionName = "days";
        private string _timesheetWeeksCollectionName = "weeks";

        private MongoClient _client;
        private IMongoDatabase _database;
        private TimesheetMapper _mapper;

        public CalendarService(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDb:ConnectionString"];
            _databaseName = configuration["MongoDb:DatabaseName"];

            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings =new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            
            _client = new MongoClient(settings);
            _database = _client.GetDatabase(_databaseName);

            _mapper = new TimesheetMapper();
        }

        #region ISOWeek Methods
        public int GetCalendarWeekForDate(DateTime date)
        {
            return ISOWeek.GetWeekOfYear(date);
        }

        public DateTime GetDateForDayOfCalendarWeek(int year, int calendarWeek, DayOfWeek dayOfWeek)
        {
            return ISOWeek.ToDateTime(year, calendarWeek, dayOfWeek);
        }
        #endregion

        #region Daily bookings
        public async Task<TimesheetDay?> GetTimesheetDayByDate(DateTime date, string userId)
        {
            var id = DayRecord.GenerateKey(date, userId);

            var collection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);

            var record = await collection.AsQueryable()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

            if (record == null)
                return null;

            return _mapper.DayRecordToTimesheetDay(record);
        }

        public async Task<List<TimesheetDay>> GetTimesheetDaysByDateRange(DateTime dateFrom, DateTime dateTo, string userId)
        {
            var collection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);

            var records = await collection.AsQueryable()
            .Where(x => x.UserId == userId && x.Date >= DateOnly.FromDateTime(dateFrom) && x.Date <= DateOnly.FromDateTime(dateTo))
            .ToListAsync();

            if (records == null)
                return new List<TimesheetDay>();

            return records.Select(x => _mapper.DayRecordToTimesheetDay(x)).OrderBy(x => x.DateTime).ToList();
        }

        //public async Task<List<TimesheetDay>> GetTimesheetDaysForCalendarweek(int year, int weekNumber)
        //{
        //    var monday = DateOnly.FromDateTime(GetDateForDayOfCalendarWeek(year, weekNumber, DayOfWeek.Monday));
        //    var sunday = DateOnly.FromDateTime(GetDateForDayOfCalendarWeek(year, weekNumber, DayOfWeek.Sunday));

        //    var daysCollection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);

        //    var dayRecords = await daysCollection.AsQueryable()
        //        .Where(x => x.Date >= monday && x.Date <= sunday)
        //        .ToListAsync();

        //    return dayRecords.Select(x => _mapper.DayRecordToTimesheetDay(x))
        //        .OrderBy(x => x.DateTime)
        //        .ToList() ?? new List<TimesheetDay>();

        //    //var totalOverTimeInTicks = dayRecords
        //    //    .Where(x => x.OvertimeHours.HasValue)
        //    //    .Sum(x => x.OvertimeHours.Value.Ticks);

        //    //var totalOvertime = TimeSpan.FromTicks(totalOverTimeInTicks);
        //}

        public async Task CreateTimesheetDay(TimesheetDay timesheetDay, string userId)
        {
            var record = _mapper.TimesheetDayToDayRecord(timesheetDay);
            record.Id = DayRecord.GenerateKey(timesheetDay.Date, userId);
            record.UserId = userId;

            var collection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);
            await collection.InsertOneAsync(record);
        }

        public async Task UpdateTimesheetDay(TimesheetDay timesheetDay, string userId)
        {
            var id = DayRecord.GenerateKey(timesheetDay.Date, userId);

            var record = _mapper.TimesheetDayToDayRecord(timesheetDay);
            record.Id = id;
            record.UserId = userId;

            var collection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);
            
            var result = await collection.ReplaceOneAsync(x => x.Id == id, record);
        }

        public async Task CreateOrUpdateTimesheetDay(TimesheetDay timesheetDay, string userId)
        {
            var id = DayRecord.GenerateKey(timesheetDay.Date, userId);

            var record = _mapper.TimesheetDayToDayRecord(timesheetDay);
            record.Id = id;
            record.UserId = userId;

            var collection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);

            if (collection.AsQueryable().Where(x => x.Id == id).Count() > 0)
            {
                var result = await collection.ReplaceOneAsync(x => x.Id == id, record);
            }
            else
            {
                await collection.InsertOneAsync(record);
            }
        }

        public async Task DeleteTimesheetDay(DateTime date, string userId)
        {
            var id = DayRecord.GenerateKey(date, userId);

            var collection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);

            await collection.DeleteOneAsync(x => x.Id == id);
        }
        #endregion

        #region Weekly bookings
        public async Task<TimesheetWeek?> GetTimesheetWeek(int year, int weekNumber, string userId)
        {
            var id = WeekRecord.GenerateKey(year, weekNumber, userId);

            var collection = _database.GetCollection<WeekRecord>(_timesheetWeeksCollectionName);

            var record = await collection.AsQueryable()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

            var week = record == null ? TimesheetWeek.FromCalendarWeek(year, weekNumber) : _mapper.WeekRecordToTimesheetWeek(record);

            var from = GetDateForDayOfCalendarWeek(year, weekNumber, DayOfWeek.Monday);
            var to = GetDateForDayOfCalendarWeek(year, weekNumber, DayOfWeek.Sunday);

            var days = await GetTimesheetDaysByDateRange(from, to, userId);

            week.BookedDays = days ?? new List<TimesheetDay>();

            return week;
        }

        public async Task CreateOrUpdateTimesheetWeek(TimesheetWeek timesheetWeek, string userId)
        {
            var id = WeekRecord.GenerateKey(timesheetWeek.Year, timesheetWeek.WeekOfYear, userId);

            var record = _mapper.TimesheetWeekToWeekRecord(timesheetWeek);
            record.Id = id;
            record.UserId = userId;

            var collection = _database.GetCollection<WeekRecord>(_timesheetWeeksCollectionName);

            if (collection.AsQueryable().Where(x => x.Id == id).Count() > 0)
            {
                var result = await collection.ReplaceOneAsync(x => x.Id == id, record);
            }
            else
            {
                await collection.InsertOneAsync(record);
            }
        }

        public async Task DeleteTimesheetDay(int year, int weekNumber, string userId)
        {
            var id = WeekRecord.GenerateKey(year, weekNumber, userId);

            var collection = _database.GetCollection<WeekRecord>(_timesheetWeeksCollectionName);

            await collection.DeleteOneAsync(x => x.Id == id);
        }
        #endregion

        #region Yearly bookings
        #endregion
    }
}
