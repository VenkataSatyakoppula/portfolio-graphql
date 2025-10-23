using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using MongoDB.Driver;
using portfolio_graphql.Models;
using portfolio_graphql.Services;

namespace portfolio_graphql.GraphQL.DataLoaders
{
    public sealed class ProfileByIdDataLoader : BatchDataLoader<string, MgtAppProfile>
    {
        private readonly IMongoCollection<MgtAppProfile> _profiles;

        public ProfileByIdDataLoader(IBatchScheduler scheduler, MongoDbContext dbContext, DataLoaderOptions? options = null)
            : base(scheduler, options ?? new DataLoaderOptions())
        {
            _profiles = dbContext.Profiles;
        }

        protected override async Task<IReadOnlyDictionary<string, MgtAppProfile>> LoadBatchAsync(
            IReadOnlyList<string> keys,
            CancellationToken cancellationToken)
        {
            var filter = Builders<MgtAppProfile>.Filter.In(p => p._id, keys);
            var items = await _profiles.Find(filter).ToListAsync(cancellationToken);

            var result = new Dictionary<string, MgtAppProfile>(items.Count);
            foreach (var item in items)
            {
                result[item._id] = item;
            }
            return result;
        }
    }
}