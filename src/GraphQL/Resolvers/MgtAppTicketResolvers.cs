using HotChocolate.Resolvers;
using HotChocolate.Types;
using MongoDB.Bson;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.DataLoaders;

namespace portfolio_graphql.GraphQL.Resolvers
{
    public class MgtAppTicketResolvers
    {
        public async Task<MgtAppProfile?> GetProfile([Parent] MgtAppTicket ticket, IResolverContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(ticket.profileid)) return null;
            var loader = context.DataLoader<ProfileByIdDataLoader>();
            return await loader.LoadAsync(ticket.profileid, cancellationToken);
        }

        public async Task<MgtAppUser?> GetCreatedBy([Parent] MgtAppTicket ticket, IResolverContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(ticket.ticketcreatedby)) return null;
            var loader = context.DataLoader<UserByIdDataLoader>();
            return await loader.LoadAsync(ticket.ticketcreatedby, cancellationToken);
        }

        public async Task<MgtAppUser?> GetAssignedTo([Parent] MgtAppTicket ticket, IResolverContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(ticket.ticketassignedto)) return null;
            var loader = context.DataLoader<UserByIdDataLoader>();
            return await loader.LoadAsync(ticket.ticketassignedto, cancellationToken);
        }

        public async Task<MgtAppPosition?> GetPosition([Parent] MgtAppTicket ticket, IResolverContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(ticket.positionid)) return null;
            var loader = context.DataLoader<PositionByIdDataLoader>();
            return await loader.LoadAsync(ticket.positionid, cancellationToken);
        }

        public async Task<MgtAppGroup?> GetGroup([Parent] MgtAppTicket ticket, IResolverContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(ticket.groupid)) return null;
            var loader = context.DataLoader<GroupByIdDataLoader>();
            return await loader.LoadAsync(ticket.groupid, cancellationToken);
        }
    }
}