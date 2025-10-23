using portfolio_graphql.Models;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppTimesheetsTypes
{
    public class MgtAppTimesheetsType : ObjectType<MgtAppTimesheets>
    {
        protected override void Configure(IObjectTypeDescriptor<MgtAppTimesheets> descriptor)
        {
            descriptor.Name("MgtAppTimesheets");

            descriptor.Field(t => t._id).Type<NonNullType<StringType>>();
            descriptor.Field(t => t.timesheetmonth).Type<StringType>();
            descriptor.Field(t => t.timesheetinfo).Type<ListType<ObjectType<TimesheetInfo>>>();

            descriptor.Field("clientid")
                .Type<ObjectType<MgtAppClient>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppTimesheetsResolvers>(r => r.GetClientAsync(default!, default!));

            descriptor.Field("employeeid")
                .Type<ObjectType<MgtAppEmployee>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppTimesheetsResolvers>(r => r.GetEmployeeAsync(default!, default!));
        }
    }
}