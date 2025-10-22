using portfolio_graphql.Models;
using MongoDB.Driver;

namespace portfolio_graphql.Services
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration config)
        {
            var client = new MongoClient(config["Mongo:ConnectionString"]);
            _database = client.GetDatabase(config["Mongo:DatabaseName"]);
        }

        public IMongoCollection<MgtAppClient> Clients => _database.GetCollection<MgtAppClient>("mgtapp-client");
        public IMongoCollection<MgtAppRole> Roles => _database.GetCollection<MgtAppRole>("mgtapp-role");
        public IMongoCollection<MgtAppUser> Users => _database.GetCollection<MgtAppUser>("mgtapp-user");
        public IMongoCollection<MgtAppPosition> Positions => _database.GetCollection<MgtAppPosition>("mgtapp-position");
        public IMongoCollection<MgtAppGroup> Groups => _database.GetCollection<MgtAppGroup>("mgtapp-groups");
        public IMongoCollection<MgtAppProfile> Profiles => _database.GetCollection<MgtAppProfile>("mgtapp-profile");
        public IMongoCollection<MgtAppEmployee> Employees => _database.GetCollection<MgtAppEmployee>("mgtapp-employee");
        public IMongoCollection<MgtAppBankDetails> BankDetails => _database.GetCollection<MgtAppBankDetails>("mgtapp-bankdetails");
    }
}


