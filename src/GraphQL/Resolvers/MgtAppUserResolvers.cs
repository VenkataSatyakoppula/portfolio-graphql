using portfolio_graphql.Models;
using MongoDB.Driver;
using portfolio_graphql.Services;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using portfolio_graphql.GraphQL.DataLoaders;

namespace portfolio_graphql.GraphQL.Resolvers
{
    [ExtendObjectType(typeof(MgtAppUser))]
    public class MgtAppUserResolvers
    {
        private readonly MongoDbContext _dbContext;

        public MgtAppUserResolvers(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MgtAppClient?> GetClientAsync([Parent] MgtAppUser user, ClientByIdDataLoader clientById)
        {
            if (string.IsNullOrEmpty(user.clientid)) return null;
            return await clientById.LoadAsync(user.clientid);
        }

        public async Task<MgtAppRole?> GetRoleAsync([Parent] MgtAppUser user, RoleByIdDataLoader roleById)
        {
            if (string.IsNullOrEmpty(user.roleid)) return null;
            return await roleById.LoadAsync(user.roleid);
        }
    }
}
