using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.DataLoaders;

namespace portfolio_graphql.GraphQL.Resolvers
{
    [ExtendObjectType(typeof(MgtAppProfile))]
    public class MgtAppProfileResolvers
    {
        public async Task<MgtAppClient?> GetClientAsync([Parent] MgtAppProfile profile, ClientByIdDataLoader loader)
        {
            if (string.IsNullOrWhiteSpace(profile.clientid))
            {
                return null;
            }
            return await loader.LoadAsync(profile.clientid);
        }

        public async Task<MgtAppPosition?> GetPositionAsync([Parent] MgtAppProfile profile, PositionByIdDataLoader loader)
        {
            if (string.IsNullOrWhiteSpace(profile.positionid))
            {
                return null;
            }
            return await loader.LoadAsync(profile.positionid);
        }
    }
}