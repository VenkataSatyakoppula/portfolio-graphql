using MongoDB.Driver;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using MongoDB.Bson;
using portfolio_graphql.GraphQL.Types.MgtAppRoleTypes;
using portfolio_graphql.GraphQL.Types;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class MgtAppRoleQuery
    {
        [GraphQLName("mgtappRoles")]
        public async Task<List<MgtAppRole>> GetMgtAppRoles([GraphQLName("query")] MgtappRoleQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query);
            var result = await ctx.Roles.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappRole")]
        public async Task<MgtAppRole?> GetMgtAppRole([GraphQLName("query")] MgtappRoleQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query);
            var result = await ctx.Roles.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        private static FilterDefinition<MgtAppRole> BuildFilter(MgtappRoleQueryInput? query)
        {
            if (query == null)
            {
                return Builders<MgtAppRole>.Filter.Empty;
            }
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