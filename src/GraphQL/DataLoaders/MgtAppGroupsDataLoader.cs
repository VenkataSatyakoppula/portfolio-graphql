using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using MongoDB.Driver;
using portfolio_graphql.Models;
using portfolio_graphql.Services;

namespace portfolio_graphql.GraphQL.DataLoaders
{
    public sealed class GroupByIdDataLoader : BatchDataLoader<string, MgtAppGroup>
    {
        private readonly IMongoCollection<MgtAppGroup> _groups;

        public GroupByIdDataLoader(IBatchScheduler scheduler, MongoDbContext dbContext, DataLoaderOptions? options = null)
            : base(scheduler, options ?? new DataLoaderOptions())
        {
            _groups = dbContext.Groups;
        }

        protected override async Task<IReadOnlyDictionary<string, MgtAppGroup>> LoadBatchAsync(
            IReadOnlyList<string> keys,
            CancellationToken cancellationToken)
        {
            var filter = Builders<MgtAppGroup>.Filter.In(g => g._id, keys);
            var items = await _groups.Find(filter).ToListAsync(cancellationToken);

            var result = new Dictionary<string, MgtAppGroup>(items.Count);
            foreach (var item in items)
            {
                result[item._id] = item;
            }
            return result;
        }
    }
}