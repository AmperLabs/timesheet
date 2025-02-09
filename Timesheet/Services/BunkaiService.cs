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
        private string _kataCollectionName = "kata";

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

        public async Task<List<Bunkai>> GetBunkais()
        {
            var collection = _database.GetCollection<Bunkai>(_bunkaiCollectionName);

            return collection.AsQueryable().ToList();
        }

        public async Task<Bunkai?> GetBunkaiById(string id)
        {
            var collection = _database.GetCollection<Bunkai>(_bunkaiCollectionName);

            var record = await collection.AsQueryable()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return record;
        }

        public async Task<Bunkai?> GetBunkaiByKataName(string kataName)
        {
            var collection = _database.GetCollection<Bunkai>(_bunkaiCollectionName);

            var record = await collection.AsQueryable()
                .Where(x => x.KataName.ToLower() == kataName.ToLower())
                .FirstOrDefaultAsync();

            return record;
        }
    
        public async Task CreateOrUpdateBunkai(Bunkai bunkai)
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

        public async Task DeleteBunkaiById(string id)
        {
            var collection = _database.GetCollection<Bunkai>(_bunkaiCollectionName);

            await collection.DeleteOneAsync(x => x.Id == id);
        }


        public async Task<List<Kata>> GetKatas()
        {
            var collection = _database.GetCollection<Kata>(_kataCollectionName);

            return collection.AsQueryable().ToList();
        }

        public async Task CreateOrUpdateKata(Kata kata)
        {
            var collection = _database.GetCollection<Kata>(_kataCollectionName);

            if (string.IsNullOrEmpty(kata.Id))
            {
                kata.Id = Guid.NewGuid().ToString();
                await collection.InsertOneAsync(kata);
            }
            else
            {
                await collection.ReplaceOneAsync(x => x.Id == kata.Id, kata);
            }
        }

        public async Task DeleteKataById(string id)
        {
            var collection = _database.GetCollection<Kata>(_kataCollectionName);

            await collection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
