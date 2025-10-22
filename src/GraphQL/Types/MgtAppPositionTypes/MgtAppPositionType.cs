using portfolio_graphql.Models;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppPositionTypes
{
    public class MgtAppPositionType : ObjectType<MgtAppPosition>
    {
        protected override void Configure(IObjectTypeDescriptor<MgtAppPosition> descriptor)
        {
            descriptor.Name("MgtAppPosition");

            descriptor.Field(p => p._id).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.jobtitle).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.experience).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.skillset).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.billingrate).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.status).Type<NonNullType<StringType>>();

            descriptor.Field("clientid")
                .Type<ObjectType<MgtAppClient>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppPositionResolvers>(r => r.GetClientAsync(default!, default!));

            descriptor.Field("groupid")
                .Type<ObjectType<MgtAppGroup>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppPositionResolvers>(r => r.GetGroupAsync(default!, default!));
        }
    }
}