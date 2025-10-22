using portfolio_graphql.Models;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppProfileTypes
{
    public class MgtAppProfileType : ObjectType<MgtAppProfile>
    {
        protected override void Configure(IObjectTypeDescriptor<MgtAppProfile> descriptor)
        {
            descriptor.Name("MgtAppProfile");

            // Keep _id non-null since it is always present on documents
            descriptor.Field(p => p._id).Type<NonNullType<StringType>>();

            // All other scalar fields optional
            descriptor.Field(p => p.resume).Type<StringType>();
            descriptor.Field(p => p.profilevisastatus).Type<StringType>();
            descriptor.Field(p => p.profilerate).Type<StringType>();
            descriptor.Field(p => p.profilelastname).Type<StringType>();
            descriptor.Field(p => p.profilefirstname).Type<StringType>();
            descriptor.Field(p => p.profileemail).Type<StringType>();
            descriptor.Field(p => p.profiletype).Type<StringType>();
            descriptor.Field(p => p.profileexpirydate).Type<StringType>();
            descriptor.Field(p => p.profiledob).Type<StringType>();
            descriptor.Field(p => p.profilestatus).Type<StringType>();
            descriptor.Field(p => p.profilephone).Type<StringType>();
            descriptor.Field(p => p.profilevendor).Type<StringType>();
            descriptor.Field(p => p.profilecomments).Type<StringType>();

            // Optional availability arrays
            descriptor.Field(p => p.profilemanageravail).Type<ListType<StringType>>();
            descriptor.Field(p => p.profileavail).Type<ListType<StringType>>();

            // Object fields via resolvers
            descriptor.Field("clientid")
                .Type<ObjectType<MgtAppClient>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppProfileResolvers>(r => r.GetClientAsync(default!, default!));

            descriptor.Field("positionid")
                .Type<ObjectType<MgtAppPosition>>()
                .ResolveWith<portfolio_graphql.GraphQL.Resolvers.MgtAppProfileResolvers>(r => r.GetPositionAsync(default!, default!));
        }
    }
}