using MongoDB.Driver;
using MongoDB.Bson;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types.MgtAppGroupTypes;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class MgtAppGroupQuery
    {
        [GraphQLName("mgtappGroups")]
        public async Task<List<MgtAppGroup>> GetMgtAppGroups([GraphQLName("query")] MgtAppGroupQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Groups.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappGroup")]
        public async Task<MgtAppGroup?> GetMgtAppGroup([GraphQLName("query")] MgtAppGroupQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Groups.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        private static FilterDefinition<MgtAppGroup> BuildFilter(MgtAppGroupQueryInput? query, MongoDbContext ctx)
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
            // Remove primitive clientid filter per new input shape
            // if (!string.IsNullOrWhiteSpace(query.clientid))
            // {
            //     filters.Add(Builders<MgtAppGroup>.Filter.Eq(x => x.clientid, query.clientid));
            // }

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

            // Nested client filters using MgtAppClientQueryInput (renamed to clientid)
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