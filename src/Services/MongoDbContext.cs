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
        public IMongoCollection<MgtAppImmigration> Immigrations => _database.GetCollection<MgtAppImmigration>("mgtapp-immigration");
        public IMongoCollection<MgtAppInsurance> Insurances => _database.GetCollection<MgtAppInsurance>("mgtapp-insurance");
        public IMongoCollection<MgtAppTimesheets> Timesheets => _database.GetCollection<MgtAppTimesheets>("mgtapp-timesheets");
        public IMongoCollection<MgtAppTicket> Tickets => _database.GetCollection<MgtAppTicket>("mgtapp-tickets");

        // Create common indexes for tickets used by filters to avoid collection scans
        public void EnsureTicketIndexes()
        {
            var keys = Builders<MgtAppTicket>.IndexKeys;
            var models = new List<CreateIndexModel<MgtAppTicket>>
            {
                new CreateIndexModel<MgtAppTicket>(keys.Ascending(x => x.ticketstatus)),
                new CreateIndexModel<MgtAppTicket>(keys.Ascending(x => x.tickettype)),
                new CreateIndexModel<MgtAppTicket>(keys.Ascending(x => x.timesheetweek)),
                new CreateIndexModel<MgtAppTicket>(keys.Ascending(x => x.ticketcreateddate)),
                new CreateIndexModel<MgtAppTicket>(keys.Ascending(x => x.profileid)),
                new CreateIndexModel<MgtAppTicket>(keys.Ascending(x => x.ticketcreatedby)),
                new CreateIndexModel<MgtAppTicket>(keys.Ascending(x => x.ticketassignedto))
            };
            Tickets.Indexes.CreateMany(models);
        }
    }
}


