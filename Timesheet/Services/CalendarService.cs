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
        public async Task<TimesheetDay?> GetTimesheetDayByDate(DateTime date)
        {
            var id = DayRecord.GenerateKey(date);

            var collection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);

            var record = await collection.AsQueryable()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

            if (record == null)
                return null;

            return _mapper.DayRecordToTimesheetDay(record);
        }

        public async Task<List<TimesheetDay>> GetTimesheetDaysForCalendarweek(int year, int weekNumber)
        {
            var monday = DateOnly.FromDateTime(GetDateForDayOfCalendarWeek(year, weekNumber, DayOfWeek.Monday));
            var sunday = DateOnly.FromDateTime(GetDateForDayOfCalendarWeek(year, weekNumber, DayOfWeek.Sunday));

            var daysCollection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);

            var dayRecords = await daysCollection.AsQueryable()
                .Where(x => x.Date >= monday && x.Date <= sunday)
                .ToListAsync();

            return dayRecords.Select(x => _mapper.DayRecordToTimesheetDay(x))
                .OrderBy(x => x.DateTime)
                .ToList() ?? new List<TimesheetDay>();

            //var totalOverTimeInTicks = dayRecords
            //    .Where(x => x.OvertimeHours.HasValue)
            //    .Sum(x => x.OvertimeHours.Value.Ticks);

            //var totalOvertime = TimeSpan.FromTicks(totalOverTimeInTicks);
        }

        public async Task CreateTimesheetDay(TimesheetDay timesheetDay)
        {
            var record = _mapper.TimesheetDayToDayRecord(timesheetDay);
            record.Id = DayRecord.GenerateKey(timesheetDay.Date);

            var collection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);
            await collection.InsertOneAsync(record);
        }

        public async Task UpdateTimesheetDay(TimesheetDay timesheetDay)
        {
            var id = DayRecord.GenerateKey(timesheetDay.Date);

            var record = _mapper.TimesheetDayToDayRecord(timesheetDay);
            record.Id = id;

            var collection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);
            
            var result = await collection.ReplaceOneAsync(x => x.Id == id, record);
        }

        public async Task CreateOrUpdateTimesheetDay(TimesheetDay timesheetDay)
        {
            var id = DayRecord.GenerateKey(timesheetDay.Date);

            var record = _mapper.TimesheetDayToDayRecord(timesheetDay);
            record.Id = id;

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

        public async Task DeleteTimesheetDay(DateTime date)
        {
            var id = DayRecord.GenerateKey(date);

            var collection = _database.GetCollection<DayRecord>(_timesheetDaysCollectionName);

            await collection.DeleteOneAsync(x => x.Id == id);
        }
        #endregion

        #region Weekly bookings
        public async Task<TimesheetWeek> GetTimesheetWeek(int year, int weekNumber)
        {
            var days = await GetTimesheetDaysForCalendarweek(year, weekNumber);

            var id = WeekRecord.GenerateKey(year, weekNumber);

            var collection = _database.GetCollection<WeekRecord>(_timesheetWeeksCollectionName);

            var record = await collection.AsQueryable()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

            var week = new TimesheetWeek(year, weekNumber);

            if (record != null)
            {
                //week = _mapper.WeekRecordToTimesheetWeek(record);
            }

            week.Days = days;

            return week;
        }
        #endregion

        #region Yearly bookings
        #endregion
    }
}
