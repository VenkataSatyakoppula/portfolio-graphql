using portfolio_graphql.Models;
using portfolio_graphql.GraphQL.Resolvers;
using portfolio_graphql.GraphQL.Types.MgtAppProfileTypes;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;
using portfolio_graphql.GraphQL.Types.MgtAppPositionTypes;
using portfolio_graphql.GraphQL.Types.MgtAppGroupTypes;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Types
{
    public class MgtAppTicketType : ObjectType<MgtAppTicket>
    {
        protected override void Configure(IObjectTypeDescriptor<MgtAppTicket> descriptor)
        {
            descriptor.Name("MgtAppTicket");

            descriptor.Field(f => f._id).Type<NonNullType<StringType>>();
            descriptor.Field("profileid").Type<MgtAppProfileType>().ResolveWith<MgtAppTicketResolvers>(r => r.GetProfile(default!, default!, default));
            descriptor.Field("ticketcreatedby").Type<MgtAppUserType>().ResolveWith<MgtAppTicketResolvers>(r => r.GetCreatedBy(default!, default!, default));
            descriptor.Field("ticketassignedto").Type<MgtAppUserType>().ResolveWith<MgtAppTicketResolvers>(r => r.GetAssignedTo(default!, default!, default));
            descriptor.Field("positionid").Type<MgtAppPositionType>().ResolveWith<MgtAppTicketResolvers>(r => r.GetPosition(default!, default!, default));
            descriptor.Field("groupid").Type<MgtAppGroupType>().ResolveWith<MgtAppTicketResolvers>(r => r.GetGroup(default!, default!, default));

            descriptor.Field(f => f.ticketcreateddate).Type<StringType>();
            descriptor.Field(f => f.timesheetweek).Type<StringType>();
            descriptor.Field(f => f.tickettype).Type<StringType>();
            descriptor.Field(f => f.ticketstatus).Type<StringType>();
            descriptor.Field(f => f.ticketcategory).Type<StringType>();
        }
    }
}