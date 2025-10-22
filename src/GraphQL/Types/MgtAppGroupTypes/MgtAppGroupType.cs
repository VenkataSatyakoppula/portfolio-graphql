using portfolio_graphql.Models;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppGroupTypes
{
    public class MgtAppGroupType : ObjectType<MgtAppGroup>
    {
        protected override void Configure(IObjectTypeDescriptor<MgtAppGroup> descriptor)
        {
            descriptor.Name("MgtAppGroup");

            descriptor.Field(g => g._id).Type<NonNullType<StringType>>();
            descriptor.Field(g => g.groupname).Type<NonNullType<StringType>>();

            descriptor.Field("clientid")
                .Type<ObjectType<MgtAppClient>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppGroupResolvers>(r => r.GetClientAsync(default!, default!));
        }
    }
}