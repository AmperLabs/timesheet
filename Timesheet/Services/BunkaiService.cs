using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Security.Authentication;
using Timesheet.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Timesheet.Services
{
    public class BunkaiService
    {
        private string _databaseName = "";
        private string _bunkaiCollectionName = "bunkai";

        private MongoClient _client;
        private IMongoDatabase _database;

        public BunkaiService(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDb:ConnectionString"];
            _databaseName = configuration["MongoDb:DatabaseName"];

            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            _client = new MongoClient(settings);
            _database = _client.GetDatabase(_databaseName);
        }

        public async Task<List<Bunkai>> GetAll()
        {
            var collection = _database.GetCollection<Bunkai>(_bunkaiCollectionName);

            return collection.AsQueryable().ToList();
        }

        public async Task<Bunkai?> GetByKataName(string kataName)
        {
            var collection = _database.GetCollection<Bunkai>(_bunkaiCollectionName);

            var record = await collection.AsQueryable()
            .Where(x => x.KataName.ToLower() == kataName.ToLower())
            .FirstOrDefaultAsync();

            return record;
        }
    
        public async Task CreateOrUpdate(Bunkai bunkai)
        {
            var collection = _database.GetCollection<Bunkai>(_bunkaiCollectionName);

            if(string.IsNullOrEmpty(bunkai.Id))
            {
                bunkai.Id = Guid.NewGuid().ToString();
                await collection.InsertOneAsync(bunkai);
            }
            else
            {
                await collection.ReplaceOneAsync(x => x.Id == bunkai.Id, bunkai);
            }
        }
    }
}
