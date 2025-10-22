using portfolio_graphql.Models;
using portfolio_graphql.GraphQL.DataLoaders;
using HotChocolate;
using HotChocolate.Types;
using System.Threading.Tasks;

namespace portfolio_graphql.GraphQL.Resolvers
{
    [ExtendObjectType(typeof(MgtAppGroup))]
    public class MgtAppGroupResolvers
    {
        public async Task<MgtAppClient?> GetClientAsync([Parent] MgtAppGroup group, ClientByIdDataLoader clientById)
        {
            if (string.IsNullOrEmpty(group.clientid)) return null;
            return await clientById.LoadAsync(group.clientid);
        }
    }
}