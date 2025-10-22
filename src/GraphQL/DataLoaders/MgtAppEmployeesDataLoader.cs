using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using MongoDB.Driver;
using portfolio_graphql.Models;
using portfolio_graphql.Services;

namespace portfolio_graphql.GraphQL.DataLoaders
{
    public sealed class EmployeeByIdDataLoader : BatchDataLoader<string, MgtAppEmployee>
    {
        private readonly IMongoCollection<MgtAppEmployee> _employees;

        public EmployeeByIdDataLoader(IBatchScheduler scheduler, MongoDbContext dbContext, DataLoaderOptions? options = null)
            : base(scheduler, options ?? new DataLoaderOptions())
        {
            _employees = dbContext.Employees;
        }

        protected override async Task<IReadOnlyDictionary<string, MgtAppEmployee>> LoadBatchAsync(
            IReadOnlyList<string> keys,
            CancellationToken cancellationToken)
        {
            var filter = Builders<MgtAppEmployee>.Filter.In(e => e._id, keys);
            var items = await _employees.Find(filter).ToListAsync(cancellationToken);

            var result = new Dictionary<string, MgtAppEmployee>(items.Count);
            foreach (var item in items)
            {
                result[item._id] = item;
            }
            return result;
        }
    }
}