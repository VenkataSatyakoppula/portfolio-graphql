using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using MongoDB.Driver;
using portfolio_graphql.Models;
using portfolio_graphql.Services;

namespace portfolio_graphql.GraphQL.DataLoaders
{
    public sealed class PositionByIdDataLoader : BatchDataLoader<string, MgtAppPosition>
    {
        private readonly IMongoCollection<MgtAppPosition> _positions;

        public PositionByIdDataLoader(IBatchScheduler scheduler, MongoDbContext dbContext, DataLoaderOptions? options = null)
            : base(scheduler, options ?? new DataLoaderOptions())
        {
            _positions = dbContext.Positions;
        }

        protected override async Task<IReadOnlyDictionary<string, MgtAppPosition>> LoadBatchAsync(
            IReadOnlyList<string> keys,
            CancellationToken cancellationToken)
        {
            var filter = Builders<MgtAppPosition>.Filter.In(p => p._id, keys);
            var items = await _positions.Find(filter).ToListAsync(cancellationToken);

            var result = new Dictionary<string, MgtAppPosition>(items.Count);
            foreach (var item in items)
            {
                result[item._id] = item;
            }
            return result;
        }
    }
}