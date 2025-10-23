using portfolio_graphql.Models;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppImmigrationTypes
{
    public class MgtAppImmigrationType : ObjectType<MgtAppImmigration>
    {
        protected override void Configure(IObjectTypeDescriptor<MgtAppImmigration> descriptor)
        {
            descriptor.Name("MgtAppImmigration");

            descriptor.Field(i => i._id).Type<NonNullType<StringType>>();
            descriptor.Field(i => i.immigrationstatus).Type<StringType>();
            descriptor.Field(i => i.immigrationsubstatus).Type<StringType>();

            descriptor.Field("clientid")
                .Type<ObjectType<MgtAppClient>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppImmigrationResolvers>(r => r.GetClientAsync(default!, default!));

            descriptor.Field("employeeid")
                .Type<ObjectType<MgtAppEmployee>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppImmigrationResolvers>(r => r.GetEmployeeAsync(default!, default!));
        }
    }
}