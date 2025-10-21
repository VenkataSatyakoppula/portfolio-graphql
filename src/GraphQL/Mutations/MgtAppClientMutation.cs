using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppClientsPayload")]
    public class DeleteManyMgtAppClientsPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppClient documents deleted.")]
        public int deletedCount { get; set; }
    }
    
    [GraphQLName("UpdateManyMgtAppClientsPayload")]
    public class UpdateManyMgtAppClientsPayload
    {
        [GraphQLName("modifiedCount")]
        [GraphQLDescription("Number of MgtAppClient documents modified.")]
        public int modifiedCount { get; set; }
    }
    
    [ExtendObjectType("Mutation")]
    public class MgtAppClientMutation
    {
        // Client mutations
        [GraphQLName("insertOneMgtappClient")]
        public async Task<MgtAppClient> InsertOneMgtAppClient(MgtAppClientInsertInput input, [Service] MongoDbContext ctx)
        {
            var id = ObjectId.GenerateNewId().ToString();
            var doc = new MgtAppClient
            {
                _id = id,
                clientname = input.clientname
            };
            await ctx.Clients.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappClient")]
        public async Task<MgtAppClient?> UpdateOneMgtAppClient([GraphQLName("query")] MgtAppClientQueryInput query, MgtAppClientUpdateInput update, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query);

            var updates = new List<UpdateDefinition<MgtAppClient>>();
            if (update.clientname != null)
            {
                updates.Add(Builders<MgtAppClient>.Update.Set(x => x.clientname, update.clientname));
            }

            if (!updates.Any())
            {
                throw new GraphQLException("No update fields provided.");
            }

            var combinedUpdate = Builders<MgtAppClient>.Update.Combine(updates);
            var options = new FindOneAndUpdateOptions<MgtAppClient> { ReturnDocument = ReturnDocument.After };
            var result = await ctx.Clients.FindOneAndUpdateAsync(filter, combinedUpdate, options);
            return result;
        }

        [GraphQLName("deleteOneMgtappClient")]
        public async Task<MgtAppClient?> DeleteOneMgtAppClient([GraphQLName("query")] MgtAppClientQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id) && string.IsNullOrWhiteSpace(query.clientname) && query.clientnameQuery == null && (query.and == null || !query.and.Any()) && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("Delete requires a filter (provide _id, clientname, clientnameQuery, and/or logical groups).");
            }
            var filter = BuildFilter(query);
            var deleted = await ctx.Clients.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappClients")]
        public async Task<DeleteManyMgtAppClientsPayload> DeleteManyMgtAppClients([GraphQLName("query")] MgtAppClientQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id) && string.IsNullOrWhiteSpace(query.clientname) && query.clientnameQuery == null && (query.and == null || !query.and.Any()) && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, clientname, clientnameQuery, and/or logical groups).");
            }
            var filter = BuildFilter(query);
            var result = await ctx.Clients.DeleteManyAsync(filter);
            return new DeleteManyMgtAppClientsPayload { deletedCount = (int)result.DeletedCount };
        }

        [GraphQLName("updateManyMgtappClients")]
        public async Task<UpdateManyMgtAppClientsPayload> UpdateManyMgtAppClients([GraphQLName("query")] MgtAppClientQueryInput query, MgtAppClientUpdateInput update, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query);

            var updates = new List<UpdateDefinition<MgtAppClient>>();
            if (update.clientname != null)
            {
                updates.Add(Builders<MgtAppClient>.Update.Set(x => x.clientname, update.clientname));
            }

            if (!updates.Any())
            {
                throw new GraphQLException("No update fields provided.");
            }

            var combinedUpdate = Builders<MgtAppClient>.Update.Combine(updates);
            var result = await ctx.Clients.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppClientsPayload { modifiedCount = (int)result.ModifiedCount };
        }

        private static FilterDefinition<MgtAppClient> BuildFilter(MgtAppClientQueryInput query)
        {
            var filters = new List<FilterDefinition<MgtAppClient>>();

            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppClient>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.clientname))
            {
                filters.Add(Builders<MgtAppClient>.Filter.Eq(x => x.clientname, query.clientname));
            }

            if (query.clientnameQuery != null)
            {
                var q = query.clientnameQuery;
                var fq = new List<FilterDefinition<MgtAppClient>>();
                if (q.eq != null) fq.Add(Builders<MgtAppClient>.Filter.Eq(x => x.clientname, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppClient>.Filter.Ne(x => x.clientname, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppClient>.Filter.In(x => x.clientname, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppClient>.Filter.Nin(x => x.clientname, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppClient>.Filter.Regex(x => x.clientname, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppClient>.Filter.And(fq));
            }

            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(BuildFilter).ToArray();
                filters.Add(Builders<MgtAppClient>.Filter.And(andFilters));
            }

            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(BuildFilter).ToArray();
                filters.Add(Builders<MgtAppClient>.Filter.Or(orFilters));
            }

            if (!filters.Any())
            {
                return Builders<MgtAppClient>.Filter.Empty;
            }

            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppClient>.Filter.And(filters);
        }
    }
}