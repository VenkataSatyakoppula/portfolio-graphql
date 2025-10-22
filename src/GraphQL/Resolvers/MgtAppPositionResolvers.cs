using portfolio_graphql.Models;
using portfolio_graphql.GraphQL.DataLoaders;
using HotChocolate;
using HotChocolate.Types;
using System.Threading.Tasks;

namespace portfolio_graphql.GraphQL.Resolvers
{
    [ExtendObjectType(typeof(MgtAppPosition))]
    public class MgtAppPositionResolvers
    {
        public async Task<MgtAppClient?> GetClientAsync([Parent] MgtAppPosition position, ClientByIdDataLoader clientById)
        {
            if (string.IsNullOrEmpty(position.clientid)) return null;
            return await clientById.LoadAsync(position.clientid);
        }

        public async Task<MgtAppGroup?> GetGroupAsync([Parent] MgtAppPosition position, GroupByIdDataLoader groupById)
        {
            if (string.IsNullOrEmpty(position.groupid)) return null;
            return await groupById.LoadAsync(position.groupid);
        }
    }
}