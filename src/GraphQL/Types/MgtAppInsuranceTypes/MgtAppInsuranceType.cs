using portfolio_graphql.Models;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppInsuranceTypes
{
    public class MgtAppInsuranceType : ObjectType<MgtAppInsurance>
    {
        protected override void Configure(IObjectTypeDescriptor<MgtAppInsurance> descriptor)
        {
            descriptor.Name("MgtAppInsurance");

            descriptor.Field(i => i._id).Type<NonNullType<StringType>>();
            descriptor.Field(i => i.insurancestatus).Type<StringType>();
            descriptor.Field(i => i.insurancesubstatus).Type<StringType>();

            descriptor.Field("clientid")
                .Type<ObjectType<MgtAppClient>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppInsuranceResolvers>(r => r.GetClientAsync(default!, default!));

            descriptor.Field("employeeid")
                .Type<ObjectType<MgtAppEmployee>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppInsuranceResolvers>(r => r.GetEmployeeAsync(default!, default!));
        }
    }
}