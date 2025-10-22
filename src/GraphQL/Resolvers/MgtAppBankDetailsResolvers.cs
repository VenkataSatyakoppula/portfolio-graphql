using System.Threading.Tasks;
using HotChocolate;
using portfolio_graphql.Models;
using portfolio_graphql.GraphQL.DataLoaders;

namespace portfolio_graphql.GraphQL.Resolvers
{
    [ExtendObjectType(typeof(MgtAppBankDetails))]
    public class MgtAppBankDetailsResolvers
    {
        public async Task<MgtAppClient?> GetClientAsync([Parent] MgtAppBankDetails bank, ClientByIdDataLoader clientById)
        {
            if (string.IsNullOrWhiteSpace(bank.clientid)) return null;
            return await clientById.LoadAsync(bank.clientid);
        }

        public async Task<MgtAppEmployee?> GetEmployeeAsync([Parent] MgtAppBankDetails bank, EmployeeByIdDataLoader employeeById)
        {
            if (string.IsNullOrWhiteSpace(bank.employeeid)) return null;
            return await employeeById.LoadAsync(bank.employeeid);
        }
    }
}