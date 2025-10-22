using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.GraphQL.Types.MgtAppRoleTypes;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class MgtAppRoleMutation
    {
        [GraphQLName("insertOneMgtappRole")]
        public async Task<MgtAppRole> InsertOneMgtAppRole(MgtAppRoleInsertInput input, [Service] MongoDbContext ctx)
        {
            var id = ObjectId.GenerateNewId().ToString();
            var doc = new MgtAppRole { _id = id, rolename = input.rolename };
            await ctx.Roles.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappRole")]
        public async Task<MgtAppRole?> UpdateOneMgtAppRole([GraphQLName("query")] MgtAppRoleQueryInput query, MgtAppRoleSetInput set, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query);
            var updates = new List<UpdateDefinition<MgtAppRole>>();
            if (set.rolename != null)
            {
                updates.Add(Builders<MgtAppRole>.Update.Set(x => x.rolename, set.rolename));
            }
            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }
            var combinedUpdate = Builders<MgtAppRole>.Update.Combine(updates);
            var options = new FindOneAndUpdateOptions<MgtAppRole> { ReturnDocument = ReturnDocument.After };
            var result = await ctx.Roles.FindOneAndUpdateAsync(filter, combinedUpdate, options);
            return result;
        }

        [GraphQLName("deleteOneMgtappRole")]
        public async Task<MgtAppRole?> DeleteOneMgtAppRole([GraphQLName("query")] MgtAppRoleQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id) && string.IsNullOrWhiteSpace(query.rolename) && query.rolenameQuery == null && (query.and == null || !query.and.Any()) && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("Delete requires a filter (provide _id, rolename, rolenameQuery, and/or logical groups).");
            }
            var filter = BuildFilter(query);
            var deleted = await ctx.Roles.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappRoles")]
        public async Task<DeleteManyMgtAppRolesPayload> DeleteManyMgtAppRoles([GraphQLName("query")] MgtAppRoleQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id) && string.IsNullOrWhiteSpace(query.rolename) && query.rolenameQuery == null && (query.and == null || !query.and.Any()) && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, rolename, rolenameQuery, and/or logical groups).");
            }
            var filter = BuildFilter(query);
            var result = await ctx.Roles.DeleteManyAsync(filter);
            return new DeleteManyMgtAppRolesPayload { deletedCount = (int)result.DeletedCount };
        }

        [GraphQLName("DeleteManyMgtAppRolesPayload")]
        public class DeleteManyMgtAppRolesPayload
        {
            [GraphQLName("deletedCount")]
            [GraphQLDescription("Number of MgtAppRole documents deleted.")]
            public int deletedCount { get; set; }
        }

        [GraphQLName("UpdateManyMgtAppRolesPayload")]
        public class UpdateManyMgtAppRolesPayload
        {
            [GraphQLName("modifiedCount")]
            [GraphQLDescription("Number of MgtAppRole documents modified.")]
            public int modifiedCount { get; set; }
        }

        [GraphQLName("updateManyMgtappRoles")]
        public async Task<UpdateManyMgtAppRolesPayload> UpdateManyMgtAppRoles([GraphQLName("query")] MgtAppRoleQueryInput query, MgtAppRoleSetInput set, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query);

            var updates = new List<UpdateDefinition<MgtAppRole>>();
            if (set.rolename != null)
            {
                updates.Add(Builders<MgtAppRole>.Update.Set(x => x.rolename, set.rolename));
            }

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppRole>.Update.Combine(updates);
            var result = await ctx.Roles.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppRolesPayload { modifiedCount = (int)result.ModifiedCount };
        }

        private static FilterDefinition<MgtAppRole> BuildFilter(MgtAppRoleQueryInput query)
        {
            var filters = new List<FilterDefinition<MgtAppRole>>();
            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppRole>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.rolename))
            {
                filters.Add(Builders<MgtAppRole>.Filter.Eq(x => x.rolename, query.rolename));
            }
            if (query.rolenameQuery != null)
            {
                var q = query.rolenameQuery;
                var fq = new List<FilterDefinition<MgtAppRole>>();
                if (q.eq != null) fq.Add(Builders<MgtAppRole>.Filter.Eq(x => x.rolename, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppRole>.Filter.Ne(x => x.rolename, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppRole>.Filter.In(x => x.rolename, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppRole>.Filter.Nin(x => x.rolename, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppRole>.Filter.Regex(x => x.rolename, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppRole>.Filter.And(fq));
            }
            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(BuildFilter).ToArray();
                filters.Add(Builders<MgtAppRole>.Filter.And(andFilters));
            }
            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(BuildFilter).ToArray();
                filters.Add(Builders<MgtAppRole>.Filter.Or(orFilters));
            }
            if (!filters.Any())
            {
                return Builders<MgtAppRole>.Filter.Empty;
            }
            if (filters.Count == 1)
            {
                return filters[0];
            }
            return Builders<MgtAppRole>.Filter.And(filters);
        }
    }
}