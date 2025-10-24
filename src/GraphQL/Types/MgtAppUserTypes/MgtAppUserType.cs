using portfolio_graphql.Models;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppUserTypes
{
    public class MgtAppUserType : ObjectType<MgtAppUser>
    {
        protected override void Configure(IObjectTypeDescriptor<MgtAppUser> descriptor)
        {
            descriptor.Name("MgtAppUser");

            // Expose base fields
            descriptor.Field(u => u._id).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.username).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.useremail).Type<NonNullType<StringType>>();

            // Expose object fields via resolvers using DataLoaders
            descriptor.Field("clientid")
                .Type<ObjectType<MgtAppClient>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppUserResolvers>(r => r.GetClientAsync(default!, default!));

            descriptor.Field("roleid")
                .Type<ObjectType<MgtAppRole>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppUserResolvers>(r => r.GetRoleAsync(default!, default!));

            descriptor.Field("groupid")
                .Type<ObjectType<MgtAppGroup>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppUserResolvers>(r => r.GetGroupAsync(default!, default!));
        }
    }
}