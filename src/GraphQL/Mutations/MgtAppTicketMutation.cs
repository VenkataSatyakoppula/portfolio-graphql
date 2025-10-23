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
        public async Task<MgtAppTicket?> InsertOneMgtAppTicket(
            [Service] MongoDbContext dbContext,
            MgtAppTicketInsertInput input)
        {
            var collection = dbContext.Tickets;
            var doc = new MgtAppTicket
            {
                _id = ObjectId.GenerateNewId().ToString(),
                profileid = input.profileid?.link,
                ticketcreatedby = input.ticketcreatedby?.link,
                ticketassignedto = input.ticketassignedto?.link,
                ticketcreateddate = input.ticketcreateddate,
                timesheetweek = input.timesheetweek,
                tickettype = input.tickettype,
                ticketstatus = input.ticketstatus,
                ticketcategory = input.ticketcategory
            };
            await collection.InsertOneAsync(doc);
            return doc;
        }

        public async Task<MgtAppTicket?> UpdateOneMgtAppTicket(
            [Service] MongoDbContext dbContext,
            portfolio_graphql.GraphQL.Types.MgtAppTicketQueryInput query,
            MgtAppTicketSetInput set)
        {
            var collection = dbContext.Tickets;
            var filter = MgtAppTicketQuery.BuildFilter(query, dbContext);
            var updates = new List<UpdateDefinition<MgtAppTicket>>();

            if (set.profileid != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.profileid, set.profileid.link));
            if (set.ticketcreatedby != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcreatedby, set.ticketcreatedby.link));
            if (set.ticketassignedto != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketassignedto, set.ticketassignedto.link));

            if (set.ticketcreateddate != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcreateddate, set.ticketcreateddate));
            if (set.timesheetweek != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.timesheetweek, set.timesheetweek));
            if (set.tickettype != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.tickettype, set.tickettype));
            if (set.ticketstatus != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketstatus, set.ticketstatus));
            if (set.ticketcategory != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcategory, set.ticketcategory));

            if (updates.Count == 0) return await collection.Find(filter).FirstOrDefaultAsync();

            var update = Builders<MgtAppTicket>.Update.Combine(updates);
            return await collection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<MgtAppTicket> { ReturnDocument = ReturnDocument.After });
        }

        public async Task<long> UpdateManyMgtAppTicket(
            [Service] MongoDbContext dbContext,
            portfolio_graphql.GraphQL.Types.MgtAppTicketQueryInput query,
            MgtAppTicketSetInput set)
        {
            var collection = dbContext.Tickets;
            var filter = MgtAppTicketQuery.BuildFilter(query, dbContext);
            var updates = new List<UpdateDefinition<MgtAppTicket>>();

            if (set.profileid != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.profileid, set.profileid.link));
            if (set.ticketcreatedby != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcreatedby, set.ticketcreatedby.link));
            if (set.ticketassignedto != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketassignedto, set.ticketassignedto.link));

            if (set.ticketcreateddate != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcreateddate, set.ticketcreateddate));
            if (set.timesheetweek != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.timesheetweek, set.timesheetweek));
            if (set.tickettype != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.tickettype, set.tickettype));
            if (set.ticketstatus != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketstatus, set.ticketstatus));
            if (set.ticketcategory != null) updates.Add(Builders<MgtAppTicket>.Update.Set(x => x.ticketcategory, set.ticketcategory));

            if (updates.Count == 0) return 0;

            var update = Builders<MgtAppTicket>.Update.Combine(updates);
            var result = await collection.UpdateManyAsync(filter, update);
            return result.ModifiedCount;
        }

        public async Task<MgtAppTicket?> DeleteOneMgtAppTicket(
            [Service] MongoDbContext dbContext,
            portfolio_graphql.GraphQL.Types.MgtAppTicketQueryInput query)
        {
            var collection = dbContext.Tickets;
            var filter = MgtAppTicketQuery.BuildFilter(query, dbContext);
            return await collection.FindOneAndDeleteAsync(filter);
        }

        public async Task<long> DeleteManyMgtAppTicket(
            [Service] MongoDbContext dbContext,
            portfolio_graphql.GraphQL.Types.MgtAppTicketQueryInput query)
        {
            var collection = dbContext.Tickets;
            var filter = MgtAppTicketQuery.BuildFilter(query, dbContext);
            var result = await collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }
    }
}