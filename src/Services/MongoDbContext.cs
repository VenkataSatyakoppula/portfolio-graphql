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
    }
}


