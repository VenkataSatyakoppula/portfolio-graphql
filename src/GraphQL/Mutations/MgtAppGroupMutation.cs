using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.GraphQL.Types.MgtAppGroupTypes;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Queries; // for MgtAppClientQuery
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppGroupsPayload")]
    public class DeleteManyMgtAppGroupsPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppGroup documents deleted.")]
        public int deletedCount { get; set; }
    }

    [GraphQLName("UpdateManyMgtAppGroupsPayload")]
    public class UpdateManyMgtAppGroupsPayload
    {
        [GraphQLName("modifiedCount")]
        [GraphQLDescription("Number of MgtAppGroup documents modified.")]
        public int modifiedCount { get; set; }
    }

    [ExtendObjectType("Mutation")]
    public class MgtAppGroupMutation
    {
        [GraphQLName("insertOneMgtappGroup")]
        public async Task<MgtAppGroup> InsertOneMgtAppGroup(MgtappGroupInsertInput data, [Service] MongoDbContext ctx)
        {
            var id = ObjectId.GenerateNewId().ToString();
            if (data.clientid == null || string.IsNullOrWhiteSpace(data.clientid.link))
            {
                throw new GraphQLException("clientid.link is required.");
            }
            // validate client via link
            var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, data.clientid.link)).FirstOrDefaultAsync();
            if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");

            var doc = new MgtAppGroup
            {
                _id = id,
                groupname = data.groupname,
                clientid = client._id
            };
            await ctx.Groups.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappGroup")]
        public async Task<MgtAppGroup?> UpdateOneMgtAppGroup([GraphQLName("query")] MgtappGroupQueryInput query, MgtappGroupUpdateInput set, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppGroup>>();
            if (set.groupname != null)
            {
                updates.Add(Builders<MgtAppGroup>.Update.Set(x => x.groupname, set.groupname));
            }
            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required for update.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppGroup>.Update.Set(x => x.clientid, client._id));
            }

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppGroup>.Update.Combine(updates);
            var options = new FindOneAndUpdateOptions<MgtAppGroup> { ReturnDocument = ReturnDocument.After };
            var result = await ctx.Groups.FindOneAndUpdateAsync(filter, combinedUpdate, options);
            return result;
        }

        [GraphQLName("deleteOneMgtappGroup")]
        public async Task<MgtAppGroup?> DeleteOneMgtAppGroup([GraphQLName("query")] MgtappGroupQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id) && string.IsNullOrWhiteSpace(query.groupname) && query.groupnameQuery == null && (query.clientid == null) && (query.and == null || !query.and.Any()) && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("Delete requires a filter (provide _id, groupname, groupnameQuery, clientid, and/or logical groups).");
            }
            var filter = BuildFilter(query, ctx);
            var deleted = await ctx.Groups.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappGroups")]
        public async Task<DeleteManyMgtAppGroupsPayload> DeleteManyMgtAppGroups([GraphQLName("query")] MgtappGroupQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id) && string.IsNullOrWhiteSpace(query.groupname) && query.groupnameQuery == null && (query.clientid == null) && (query.and == null || !query.and.Any()) && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, groupname, groupnameQuery, clientid, and/or logical groups).");
            }
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Groups.DeleteManyAsync(filter);
            return new DeleteManyMgtAppGroupsPayload { deletedCount = (int)result.DeletedCount };
        }

        [GraphQLName("updateManyMgtappGroups")]
        public async Task<UpdateManyMgtAppGroupsPayload> UpdateManyMgtAppGroups([GraphQLName("query")] MgtappGroupQueryInput query, MgtappGroupUpdateInput set, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppGroup>>();
            if (set.groupname != null)
            {
                updates.Add(Builders<MgtAppGroup>.Update.Set(x => x.groupname, set.groupname));
            }
            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required for update.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppGroup>.Update.Set(x => x.clientid, client._id));
            }

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppGroup>.Update.Combine(updates);
            var result = await ctx.Groups.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppGroupsPayload { modifiedCount = (int)result.ModifiedCount };
        }

        private static FilterDefinition<MgtAppGroup> BuildFilter(MgtappGroupQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppGroup>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppGroup>>();

            // Primitive field filters
            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppGroup>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.groupname))
            {
                filters.Add(Builders<MgtAppGroup>.Filter.Eq(x => x.groupname, query.groupname));
            }

            // StringQueryInput for groupname
            if (query.groupnameQuery != null)
            {
                var q = query.groupnameQuery;
                var fq = new List<FilterDefinition<MgtAppGroup>>();
                if (q.eq != null) fq.Add(Builders<MgtAppGroup>.Filter.Eq(x => x.groupname, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppGroup>.Filter.Ne(x => x.groupname, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppGroup>.Filter.In(x => x.groupname, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppGroup>.Filter.Nin(x => x.groupname, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppGroup>.Filter.Regex(x => x.groupname, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppGroup>.Filter.And(fq));
            }

            // Nested client filters using MgtappClientQueryInput (renamed to clientid)
            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppGroup>.Filter.In(g => g.clientid, clientIds));
            }

            // Logical groups
            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppGroup>.Filter.And(andFilters));
            }

            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppGroup>.Filter.Or(orFilters));
            }

            // No filters means match all
            if (!filters.Any())
            {
                return Builders<MgtAppGroup>.Filter.Empty;
            }

            // Combine accumulated filters
            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppGroup>.Filter.And(filters);
        }
    }
}