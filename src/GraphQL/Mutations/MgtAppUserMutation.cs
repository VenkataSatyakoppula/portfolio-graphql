using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppUsersPayload")]
    public class DeleteManyMgtAppUsersPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppUser documents deleted.")]
        public int deletedCount { get; set; }
    }

    [GraphQLName("UpdateManyMgtAppUsersPayload")]
    public class UpdateManyMgtAppUsersPayload
    {
        [GraphQLName("modifiedCount")]
        [GraphQLDescription("Number of MgtAppUser documents modified.")]
        public int modifiedCount { get; set; }
    }

    [ExtendObjectType("Mutation")]
    public class MgtAppUserMutation
    {
        [GraphQLName("insertOneMgtappUser")]
        public async Task<MgtAppUser> InsertOneMgtAppUser(MgtAppUserInsertInput input, [Service] MongoDbContext ctx)
        {
            var id = ObjectId.GenerateNewId().ToString();
            var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, input.clientid)).FirstOrDefaultAsync();
            if (client == null) throw new GraphQLException("Invalid clientid: client not found.");
            var role = await ctx.Roles.Find(Builders<MgtAppRole>.Filter.Eq(x => x._id, input.roleid)).FirstOrDefaultAsync();
            if (role == null) throw new GraphQLException("Invalid roleid: role not found.");

            var doc = new MgtAppUser
            {
                _id = id,
                username = input.username,
                useremail = input.useremail,
                clientid = client._id,
                roleid = role._id
            };
            await ctx.Users.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappUser")]
        public async Task<MgtAppUser?> UpdateOneMgtAppUser([GraphQLName("query")] MgtAppUserQueryInput query, MgtAppUserUpdateInput update, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query);

            var updates = new List<UpdateDefinition<MgtAppUser>>();
            if (update.username != null)
            {
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.username, update.username));
            }
            if (update.useremail != null)
            {
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.useremail, update.useremail));
            }
            if (update.clientid != null)
            {
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, update.clientid)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid: client not found.");
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.clientid, client._id));
            }
            if (update.roleid != null)
            {
                var role = await ctx.Roles.Find(Builders<MgtAppRole>.Filter.Eq(x => x._id, update.roleid)).FirstOrDefaultAsync();
                if (role == null) throw new GraphQLException("Invalid roleid: role not found.");
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.roleid, role._id));
            }

            if (!updates.Any())
            {
                throw new GraphQLException("No update fields provided.");
            }

            var combinedUpdate = Builders<MgtAppUser>.Update.Combine(updates);
            var options = new FindOneAndUpdateOptions<MgtAppUser> { ReturnDocument = ReturnDocument.After };
            var result = await ctx.Users.FindOneAndUpdateAsync(filter, combinedUpdate, options);
            return result;
        }

        [GraphQLName("deleteOneMgtappUser")]
        public async Task<MgtAppUser?> DeleteOneMgtAppUser([GraphQLName("query")] MgtAppUserQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id) && query.usernameQuery == null && query.useremailQuery == null && (query.and == null || !query.and.Any()) && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("Delete requires a filter (provide _id, clientid, roleid, usernameQuery, useremailQuery, and/or logical groups).");
            }
            var filter = BuildFilter(query);
            var deleted = await ctx.Users.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappUsers")]
        public async Task<DeleteManyMgtAppUsersPayload> DeleteManyMgtAppUsers([GraphQLName("query")] MgtAppUserQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id) && query.usernameQuery == null && query.useremailQuery == null && (query.and == null || !query.and.Any()) && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, clientid, roleid, usernameQuery, useremailQuery, and/or logical groups).");
            }
            var filter = BuildFilter(query);
            var result = await ctx.Users.DeleteManyAsync(filter);
            return new DeleteManyMgtAppUsersPayload { deletedCount = (int)result.DeletedCount };
        }

        [GraphQLName("updateManyMgtappUsers")]
        public async Task<UpdateManyMgtAppUsersPayload> UpdateManyMgtAppUsers([GraphQLName("query")] MgtAppUserQueryInput query, MgtAppUserUpdateInput update, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query);

            var updates = new List<UpdateDefinition<MgtAppUser>>();
            if (update.username != null)
            {
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.username, update.username));
            }
            if (update.useremail != null)
            {
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.useremail, update.useremail));
            }
            if (update.clientid != null)
            {
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, update.clientid)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid: client not found.");
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.clientid, client._id));
            }
            if (update.roleid != null)
            {
                var role = await ctx.Roles.Find(Builders<MgtAppRole>.Filter.Eq(x => x._id, update.roleid)).FirstOrDefaultAsync();
                if (role == null) throw new GraphQLException("Invalid roleid: role not found.");
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.roleid, role._id));
            }

            if (!updates.Any())
            {
                throw new GraphQLException("No update fields provided.");
            }

            var combinedUpdate = Builders<MgtAppUser>.Update.Combine(updates);
            var result = await ctx.Users.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppUsersPayload { modifiedCount = (int)result.ModifiedCount };
        }

        private static FilterDefinition<MgtAppUser> BuildFilter(MgtAppUserQueryInput query)
        {
            var filters = new List<FilterDefinition<MgtAppUser>>();

            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppUser>.Filter.Eq(x => x._id, query._id));
            }
            if (query.usernameQuery != null)
            {
                var q = query.usernameQuery;
                var fq = new List<FilterDefinition<MgtAppUser>>();
                if (q.eq != null) fq.Add(Builders<MgtAppUser>.Filter.Eq(x => x.username, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppUser>.Filter.Ne(x => x.username, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppUser>.Filter.In(x => x.username, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppUser>.Filter.Nin(x => x.username, q.nin));
                if (q.regex != null)
                {
                    var regex = new System.Text.RegularExpressions.Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppUser>.Filter.Regex(x => x.username, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppUser>.Filter.And(fq));
            }
            if (query.useremailQuery != null)
            {
                var q = query.useremailQuery;
                var fq = new List<FilterDefinition<MgtAppUser>>();
                if (q.eq != null) fq.Add(Builders<MgtAppUser>.Filter.Eq(x => x.useremail, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppUser>.Filter.Ne(x => x.useremail, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppUser>.Filter.In(x => x.useremail, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppUser>.Filter.Nin(x => x.useremail, q.nin));
                if (q.regex != null)
                {
                    var regex = new System.Text.RegularExpressions.Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppUser>.Filter.Regex(x => x.useremail, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppUser>.Filter.And(fq));
            }
            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(BuildFilter).ToArray();
                filters.Add(Builders<MgtAppUser>.Filter.And(andFilters));
            }
            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(BuildFilter).ToArray();
                filters.Add(Builders<MgtAppUser>.Filter.Or(orFilters));
            }
            if (!filters.Any())
            {
                return Builders<MgtAppUser>.Filter.Empty;
            }
            if (filters.Count == 1)
            {
                return filters[0];
            }
            return Builders<MgtAppUser>.Filter.And(filters);
        }
    }
}