using portfolio_graphql.Models;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes
{
    public class MgtAppEmployeeType : ObjectType<MgtAppEmployee>
    {
        protected override void Configure(IObjectTypeDescriptor<MgtAppEmployee> descriptor)
        {
            descriptor.Name("MgtAppEmployee");

            descriptor.Field(e => e._id).Type<NonNullType<StringType>>();

            descriptor.Field("clientid")
                .Type<ObjectType<MgtAppClient>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppEmployeeResolvers>(r => r.GetClientAsync(default!, default!));

            descriptor.Field("employeeuserid")
                .Type<ObjectType<MgtAppUser>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppEmployeeResolvers>(r => r.GetEmployeeUserAsync(default!, default!));

            descriptor.Field("employeemanagerid")
                .Type<ObjectType<MgtAppUser>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppEmployeeResolvers>(r => r.GetEmployeeManagerAsync(default!, default!));

            descriptor.Field(e => e.employeephone).Type<StringType>();
            descriptor.Field(e => e.employeefirstname).Type<StringType>();
            descriptor.Field(e => e.employeesalaryrate).Type<StringType>();
            descriptor.Field(e => e.employeeworkemail).Type<StringType>();
            descriptor.Field(e => e.employeeexpirydate).Type<StringType>();
            descriptor.Field(e => e.employeelastname).Type<StringType>();
            descriptor.Field(e => e.employeedob).Type<StringType>();
            descriptor.Field(e => e.employeevisastatus).Type<StringType>();
            descriptor.Field(e => e.employeeemail).Type<StringType>();
            descriptor.Field(e => e.employeevendor).Type<StringType>();
            descriptor.Field(e => e.employeetype).Type<StringType>();
            descriptor.Field(e => e.employeebillrate).Type<StringType>();
            descriptor.Field(e => e.employeesubstatus).Type<StringType>();
            descriptor.Field(e => e.employeestatus).Type<StringType>();
        }
    }
}