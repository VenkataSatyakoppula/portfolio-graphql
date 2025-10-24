using HotChocolate;
using HotChocolate.Types;
using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.Services;
using portfolio_graphql.Models;
using portfolio_graphql.GraphQL.Types.MgtAppTicketTypes;
using portfolio_graphql.GraphQL.Queries;

namespace portfolio_graphql.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class MgtAppTicketMutation
    {
        [GraphQLName("insertOneMgtappTicket")]
        public async Task<MgtAppTicket?> InsertOneMgtAppTicket(
            [Service] MongoDbContext dbContext,
            MgtappTicketInsertInput data)
        {
            var collection = dbContext.Tickets;
            var doc = new MgtAppTicket
            {
                _id = ObjectId.GenerateNewId().ToString(),
                profileid = data.profileid?.link,
                ticketcreatedby = data.ticketcreatedby?.link,
                ticketassignedto = data.ticketassignedto?.link,
                positionid = data.positionid?.link,
                groupid = data.groupid?.link,
                ticketcreateddate = data.ticketcreateddate,
                timesheetweek = data.timesheetweek,
                tickettype = data.tickettype,
                ticketstatus = data.ticketstatus,
                ticketcategory = data.ticketcategory
            };
            await collection.InsertOneAsync(doc);
            return doc;
        }
        [GraphQLName("updateOneMgtappTicket")]
        public async Task<MgtAppTicket?> UpdateOneMgtAppTicket(
            [Service] MongoDbContext dbContext,
            portfolio_graphql.GraphQL.Types.MgtappTicketQueryInput query,
            MgtappTicketUpdateInput set)
        {
            var collection = dbContext.Tickets;
            var filter = MgtAppTicketQuery.BuildFilter(query, dbContext);
            var updates = new List<UpdateDefinition<MgtAppTicket>>();

            if (set.profileid != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.profileid, set.profileid.link));
            if (set.ticketcreatedby != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcreatedby, set.ticketcreatedby.link));
            if (set.ticketassignedto != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketassignedto, set.ticketassignedto.link));
            if (set.positionid != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.positionid, set.positionid.link));
            if (set.groupid != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.groupid, set.groupid.link));

            if (set.ticketcreateddate != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcreateddate, set.ticketcreateddate));
            if (set.timesheetweek != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.timesheetweek, set.timesheetweek));
            if (set.tickettype != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.tickettype, set.tickettype));
            if (set.ticketstatus != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketstatus, set.ticketstatus));
            if (set.ticketcategory != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcategory, set.ticketcategory));

            if (updates.Count == 0) return await collection.Find(filter).FirstOrDefaultAsync();

            var update = Builders<MgtAppTicket>.Update.Combine(updates);
            return await collection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<MgtAppTicket> { ReturnDocument = ReturnDocument.After });
        }
        [GraphQLName("updateManyMgtappTickets")]
        public async Task<UpdateManyMgtAppTicketsPayload> UpdateManyMgtAppTicket(
            [Service] MongoDbContext dbContext,
            portfolio_graphql.GraphQL.Types.MgtappTicketQueryInput query,
            MgtappTicketUpdateInput set)
        {
            var collection = dbContext.Tickets;
            var filter = MgtAppTicketQuery.BuildFilter(query, dbContext);
            var updates = new List<UpdateDefinition<MgtAppTicket>>();

            if (set.profileid != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.profileid, set.profileid.link));
            if (set.ticketcreatedby != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcreatedby, set.ticketcreatedby.link));
            if (set.ticketassignedto != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketassignedto, set.ticketassignedto.link));
            if (set.positionid != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.positionid, set.positionid.link));
            if (set.groupid != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.groupid, set.groupid.link));

            if (set.ticketcreateddate != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcreateddate, set.ticketcreateddate));
            if (set.timesheetweek != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.timesheetweek, set.timesheetweek));
            if (set.tickettype != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.tickettype, set.tickettype));
            if (set.ticketstatus != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketstatus, set.ticketstatus));
            if (set.ticketcategory != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcategory, set.ticketcategory));

            if (updates.Count == 0)
            {
                return new UpdateManyMgtAppTicketsPayload { matchedCount = 0, modifiedCount = 0 };
            }

            var update = Builders<MgtAppTicket>.Update.Combine(updates);
            var result = await collection.UpdateManyAsync(filter, update);
            return new UpdateManyMgtAppTicketsPayload { matchedCount = (int)result.MatchedCount, modifiedCount = (int)result.ModifiedCount };
        }

        [GraphQLName("deleteManyMgtappTickets")]
        public async Task<DeleteManyMgtAppTicketsPayload> DeleteManyMgtAppTicket(
            [Service] MongoDbContext dbContext,
            portfolio_graphql.GraphQL.Types.MgtappTicketQueryInput query)
        {
            var collection = dbContext.Tickets;
            var filter = MgtAppTicketQuery.BuildFilter(query, dbContext);
            var result = await collection.DeleteManyAsync(filter);
            return new DeleteManyMgtAppTicketsPayload { deletedCount = (int)result.DeletedCount };
        }
    }
}