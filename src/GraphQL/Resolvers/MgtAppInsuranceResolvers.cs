using System.Threading.Tasks;
using HotChocolate;
using portfolio_graphql.Models;
using portfolio_graphql.GraphQL.DataLoaders;

namespace portfolio_graphql.GraphQL.Resolvers
{
    [ExtendObjectType(typeof(MgtAppInsurance))]
    public class MgtAppInsuranceResolvers
    {
        public async Task<MgtAppClient?> GetClientAsync([Parent] MgtAppInsurance insurance, ClientByIdDataLoader clientById)
        {
            if (string.IsNullOrWhiteSpace(insurance.clientid)) return null;
            return await clientById.LoadAsync(insurance.clientid);
        }

        public async Task<MgtAppEmployee?> GetEmployeeAsync([Parent] MgtAppInsurance insurance, EmployeeByIdDataLoader employeeById)
        {
            if (string.IsNullOrWhiteSpace(insurance.employeeid)) return null;
            return await employeeById.LoadAsync(insurance.employeeid);
        }
    }
}