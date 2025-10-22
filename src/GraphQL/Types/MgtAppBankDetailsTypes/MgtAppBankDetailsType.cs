using portfolio_graphql.Models;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppBankDetailsTypes
{
    public class MgtAppBankDetailsType : ObjectType<MgtAppBankDetails>
    {
        protected override void Configure(IObjectTypeDescriptor<MgtAppBankDetails> descriptor)
        {
            descriptor.Name("MgtAppBankDetails");

            descriptor.Field(b => b._id).Type<NonNullType<StringType>>();
            descriptor.Field(b => b.bankstatus).Type<StringType>();
            descriptor.Field(b => b.bankroutingno).Type<StringType>();
            descriptor.Field(b => b.bankaccountno).Type<StringType>();

            // Expose nested client and employee via resolvers
            descriptor.Field("clientid")
                .Type<ObjectType<MgtAppClient>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppBankDetailsResolvers>(r => r.GetClientAsync(default!, default!));

            descriptor.Field("employeeid")
                .Type<ObjectType<MgtAppEmployee>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppBankDetailsResolvers>(r => r.GetEmployeeAsync(default!, default!));
        }
    }
}