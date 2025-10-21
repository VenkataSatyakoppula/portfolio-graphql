// Add DataLoader implementations for batching client and role lookups by ID
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using MongoDB.Driver;
using portfolio_graphql.Models;
using portfolio_graphql.Services;

namespace portfolio_graphql.GraphQL.DataLoaders
{
    public sealed class ClientByIdDataLoader : BatchDataLoader<string, MgtAppClient>
    {
        private readonly IMongoCollection<MgtAppClient> _clients;

        public ClientByIdDataLoader(IBatchScheduler scheduler, MongoDbContext dbContext, DataLoaderOptions? options = null)
            : base(scheduler, options ?? new DataLoaderOptions())
        {
            _clients = dbContext.Clients;
        }

        protected override async Task<IReadOnlyDictionary<string, MgtAppClient>> LoadBatchAsync(
            IReadOnlyList<string> keys,
            CancellationToken cancellationToken)
        {
            var filter = Builders<MgtAppClient>.Filter.In(c => c._id, keys);
            var items = await _clients.Find(filter).ToListAsync(cancellationToken);

            var result = new Dictionary<string, MgtAppClient>(items.Count);
            foreach (var item in items)
            {
                result[item._id] = item;
            }
            return result;
        }
    }

    public sealed class RoleByIdDataLoader : BatchDataLoader<string, MgtAppRole>
    {
        private readonly IMongoCollection<MgtAppRole> _roles;

        public RoleByIdDataLoader(IBatchScheduler scheduler, MongoDbContext dbContext, DataLoaderOptions? options = null)
            : base(scheduler, options ?? new DataLoaderOptions())
        {
            _roles = dbContext.Roles;
        }

        protected override async Task<IReadOnlyDictionary<string, MgtAppRole>> LoadBatchAsync(
            IReadOnlyList<string> keys,
            CancellationToken cancellationToken)
        {
            var filter = Builders<MgtAppRole>.Filter.In(r => r._id, keys);
            var items = await _roles.Find(filter).ToListAsync(cancellationToken);

            var result = new Dictionary<string, MgtAppRole>(items.Count);
            foreach (var item in items)
            {
                result[item._id] = item;
            }
            return result;
        }
    }
}