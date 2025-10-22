using System.Threading.Tasks;
using HotChocolate;
using portfolio_graphql.Models;
using portfolio_graphql.GraphQL.DataLoaders;

namespace portfolio_graphql.GraphQL.Resolvers
{
    [ExtendObjectType(typeof(MgtAppEmployee))]
    public class MgtAppEmployeeResolvers
    {
        public async Task<MgtAppClient?> GetClientAsync([Parent] MgtAppEmployee employee, ClientByIdDataLoader clientById)
        {
            if (string.IsNullOrWhiteSpace(employee.clientid)) return null;
            return await clientById.LoadAsync(employee.clientid);
        }

        public async Task<MgtAppUser?> GetEmployeeUserAsync([Parent] MgtAppEmployee employee, UserByIdDataLoader userById)
        {
            if (string.IsNullOrWhiteSpace(employee.employeeuserid)) return null;
            return await userById.LoadAsync(employee.employeeuserid);
        }

        public async Task<MgtAppUser?> GetEmployeeManagerAsync([Parent] MgtAppEmployee employee, UserByIdDataLoader userById)
        {
            if (string.IsNullOrWhiteSpace(employee.employeemanagerid)) return null;
            return await userById.LoadAsync(employee.employeemanagerid);
        }
    }
}