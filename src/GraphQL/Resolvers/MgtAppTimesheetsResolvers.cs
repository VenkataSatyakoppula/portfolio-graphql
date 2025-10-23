using System.Threading.Tasks;
using HotChocolate;
using portfolio_graphql.Models;
using portfolio_graphql.GraphQL.DataLoaders;

namespace portfolio_graphql.GraphQL.Resolvers
{
    [ExtendObjectType(typeof(MgtAppTimesheets))]
    public class MgtAppTimesheetsResolvers
    {
        public async Task<MgtAppClient?> GetClientAsync([Parent] MgtAppTimesheets timesheet, ClientByIdDataLoader clientById)
        {
            if (string.IsNullOrWhiteSpace(timesheet.clientid)) return null;
            return await clientById.LoadAsync(timesheet.clientid);
        }

        public async Task<MgtAppEmployee?> GetEmployeeAsync([Parent] MgtAppTimesheets timesheet, EmployeeByIdDataLoader employeeById)
        {
            if (string.IsNullOrWhiteSpace(timesheet.employeeid)) return null;
            return await employeeById.LoadAsync(timesheet.employeeid);
        }
    }
}