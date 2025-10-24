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
using portfolio_graphql.GraphQL.Queries;

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
        public async Task<MgtAppUser> InsertOneMgtAppUser(MgtappUserInsertInput data, [Service] MongoDbContext ctx)
        {
            var id = ObjectId.GenerateNewId().ToString();
            if (data.clientid == null || string.IsNullOrWhiteSpace(data.clientid.link))
            {
                throw new GraphQLException("clientid.link is required.");
            }
            if (data.roleid == null || string.IsNullOrWhiteSpace(data.roleid.link))
            {
                throw new GraphQLException("roleid.link is required.");
            }
            var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, data.clientid.link)).FirstOrDefaultAsync();
            if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
            var role = await ctx.Roles.Find(Builders<MgtAppRole>.Filter.Eq(x => x._id, data.roleid.link)).FirstOrDefaultAsync();
            if (role == null) throw new GraphQLException("Invalid roleid.link: role not found.");

            string? groupId = null;
            if (data.groupid != null)
            {
                if (string.IsNullOrWhiteSpace(data.groupid.link))
                {
                    throw new GraphQLException("groupid.link is required when provided.");
                }
                var group = await ctx.Groups.Find(Builders<MgtAppGroup>.Filter.Eq(x => x._id, data.groupid.link)).FirstOrDefaultAsync();
                if (group == null) throw new GraphQLException("Invalid groupid.link: group not found.");
                groupId = group._id;
            }

            var doc = new MgtAppUser
            {
                _id = id,
                username = data.username,
                useremail = data.useremail,
                clientid = client._id,
                roleid = role._id,
                groupid = groupId
            };
            await ctx.Users.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappUser")]
        public async Task<MgtAppUser?> UpdateOneMgtAppUser([GraphQLName("query")] MgtappUserQueryInput query, MgtappUserUpdateInput set, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var updates = new List<UpdateDefinition<MgtAppUser>>();

            if (set.username != null)
            {
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.username, set.username));
            }
            if (set.useremail != null)
            {
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.useremail, set.useremail));
            }

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.clientid, client._id));
            }

            if (set.roleid != null)
            {
                if (string.IsNullOrWhiteSpace(set.roleid.link))
                {
                    throw new GraphQLException("roleid.link is required when provided.");
                }
                var role = await ctx.Roles.Find(Builders<MgtAppRole>.Filter.Eq(x => x._id, set.roleid.link)).FirstOrDefaultAsync();
                if (role == null) throw new GraphQLException("Invalid roleid.link: role not found.");
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.roleid, role._id));
            }

            if (set.groupid != null)
            {
                if (string.IsNullOrWhiteSpace(set.groupid.link))
                {
                    throw new GraphQLException("groupid.link is required when provided.");
                }
                var group = await ctx.Groups.Find(Builders<MgtAppGroup>.Filter.Eq(x => x._id, set.groupid.link)).FirstOrDefaultAsync();
                if (group == null) throw new GraphQLException("Invalid groupid.link: group not found.");
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.groupid, group._id));
            }

            var update = updates.Count > 0 ? Builders<MgtAppUser>.Update.Combine(updates) : null;
            if (update == null)
            {
                return await ctx.Users.Find(filter).FirstOrDefaultAsync();
            }
            return await ctx.Users.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<MgtAppUser> { ReturnDocument = ReturnDocument.After });
        }

        [GraphQLName("deleteOneMgtappUser")]
        public async Task<MgtAppUser?> DeleteOneMgtAppUser([GraphQLName("query")] MgtappUserQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id)
                && query.clientid == null
                && query.roleid == null
                && string.IsNullOrWhiteSpace(query.username)
                && string.IsNullOrWhiteSpace(query.useremail)
                && query.usernameQuery == null
                && query.useremailQuery == null
                && (query.and == null || !query.and.Any())
                && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("Delete requires a filter (provide _id, clientid, roleid, username/useremail, string queries, and/or logical groups).");
            }
            var filter = BuildFilter(query, ctx);
            var deleted = await ctx.Users.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappUsers")]
        public async Task<DeleteManyMgtAppUsersPayload> DeleteManyMgtAppUsers([GraphQLName("query")] MgtappUserQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id)
                && query.clientid == null
                && query.roleid == null
                && string.IsNullOrWhiteSpace(query.username)
                && string.IsNullOrWhiteSpace(query.useremail)
                && query.usernameQuery == null
                && query.useremailQuery == null
                && (query.and == null || !query.and.Any())
                && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, clientid, roleid, username/useremail, string queries, and/or logical groups).");
            }
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Users.DeleteManyAsync(filter);
            return new DeleteManyMgtAppUsersPayload { deletedCount = (int)result.DeletedCount };
        }

        [GraphQLName("updateManyMgtappUsers")]
        public async Task<UpdateManyMgtAppUsersPayload> UpdateManyMgtAppUsers([GraphQLName("query")] MgtappUserQueryInput query, MgtappUserUpdateInput set, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var updates = new List<UpdateDefinition<MgtAppUser>>();

            if (set.username != null)
            {
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.username, set.username));
            }
            if (set.useremail != null)
            {
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.useremail, set.useremail));
            }

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.clientid, client._id));
            }

            if (set.roleid != null)
            {
                if (string.IsNullOrWhiteSpace(set.roleid.link))
                {
                    throw new GraphQLException("roleid.link is required when provided.");
                }
                var role = await ctx.Roles.Find(Builders<MgtAppRole>.Filter.Eq(x => x._id, set.roleid.link)).FirstOrDefaultAsync();
                if (role == null) throw new GraphQLException("Invalid roleid.link: role not found.");
                updates.Add(Builders<MgtAppUser>.Update.Set(x => x.roleid, role._id));
            }

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppUser>.Update.Combine(updates);
            var result = await ctx.Users.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppUsersPayload { modifiedCount = (int)result.ModifiedCount };
        }

        private static FilterDefinition<MgtAppUser> BuildFilter(MgtappUserQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppUser>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppUser>>();

            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppUser>.Filter.Eq(x => x._id, query._id));
            }

            // Direct equality filters
            if (!string.IsNullOrWhiteSpace(query.username))
            {
                filters.Add(Builders<MgtAppUser>.Filter.Eq(x => x.username, query.username));
            }
            if (!string.IsNullOrWhiteSpace(query.useremail))
            {
                filters.Add(Builders<MgtAppUser>.Filter.Eq(x => x.useremail, query.useremail));
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

            // Nested client filter using MgtAppClientQuery
            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                if (clientIds.Count > 0)
                {
                    filters.Add(Builders<MgtAppUser>.Filter.In(u => u.clientid, clientIds));
                }
            }

            // Nested role filter
            if (query.roleid != null)
            {
                var roleFilters = new List<FilterDefinition<MgtAppRole>>();
                if (!string.IsNullOrWhiteSpace(query.roleid._id))
                {
                    roleFilters.Add(Builders<MgtAppRole>.Filter.Eq(r => r._id, query.roleid._id));
                }
                if (!string.IsNullOrWhiteSpace(query.roleid.rolename))
                {
                    roleFilters.Add(Builders<MgtAppRole>.Filter.Eq(r => r.rolename, query.roleid.rolename));
                }
                if (query.roleid.rolenameQuery != null)
                {
                    var rq = query.roleid.rolenameQuery;
                    var rfq = new List<FilterDefinition<MgtAppRole>>();
                    if (rq.eq != null) rfq.Add(Builders<MgtAppRole>.Filter.Eq(r => r.rolename, rq.eq));
                    if (rq.ne != null) rfq.Add(Builders<MgtAppRole>.Filter.Ne(r => r.rolename, rq.ne));
                    if (rq.@in != null && rq.@in.Count > 0) rfq.Add(Builders<MgtAppRole>.Filter.In(r => r.rolename, rq.@in));
                    if (rq.nin != null && rq.nin.Count > 0) rfq.Add(Builders<MgtAppRole>.Filter.Nin(r => r.rolename, rq.nin));
                    if (rq.regex != null)
                    {
                        var regex = new Regex(rq.regex, RegexOptions.IgnoreCase);
                        rfq.Add(Builders<MgtAppRole>.Filter.Regex(r => r.rolename, new BsonRegularExpression(regex)));
                    }
                    if (rfq.Count > 0) roleFilters.Add(Builders<MgtAppRole>.Filter.And(rfq));
                }

                FilterDefinition<MgtAppRole> roleFilterDef;
                if (!roleFilters.Any())
                {
                    roleFilterDef = Builders<MgtAppRole>.Filter.Empty;
                }
                else if (roleFilters.Count == 1)
                {
                    roleFilterDef = roleFilters[0];
                }
                else
                {
                    roleFilterDef = Builders<MgtAppRole>.Filter.And(roleFilters);
                }

                var roleIds = ctx.Roles.Find(roleFilterDef).Project(r => r._id).ToList();
                if (roleIds.Count > 0)
                {
                    filters.Add(Builders<MgtAppUser>.Filter.In(u => u.roleid, roleIds));
                }
            }

            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppUser>.Filter.And(andFilters));
            }
            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
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