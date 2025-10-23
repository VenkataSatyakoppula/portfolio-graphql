using System.Threading.Tasks;
using HotChocolate;
using portfolio_graphql.Models;
using portfolio_graphql.GraphQL.DataLoaders;

namespace portfolio_graphql.GraphQL.Resolvers
{
    [ExtendObjectType(typeof(MgtAppImmigration))]
    public class MgtAppImmigrationResolvers
    {
        public async Task<MgtAppClient?> GetClientAsync([Parent] MgtAppImmigration immigration, ClientByIdDataLoader clientById)
        {
            if (string.IsNullOrWhiteSpace(immigration.clientid)) return null;
            return await clientById.LoadAsync(immigration.clientid);
        }

        public async Task<MgtAppEmployee?> GetEmployeeAsync([Parent] MgtAppImmigration immigration, EmployeeByIdDataLoader employeeById)
        {
            if (string.IsNullOrWhiteSpace(immigration.employeeid)) return null;
            return await employeeById.LoadAsync(immigration.employeeid);
        }
    }
}