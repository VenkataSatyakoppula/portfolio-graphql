using MongoDB.Driver;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using MongoDB.Bson;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class MgtAppClientQuery
    {
        // Query multiple clients by input filter
        [GraphQLName("mgtappClients")]
        public async Task<List<MgtAppClient>> GetMgtAppClients([GraphQLName("query")] MgtappClientQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query);
            var result = await ctx.Clients.Find(filter).ToListAsync();
            return result;
        }

        // Query single client by input filter
        [GraphQLName("mgtappClient")]
        public async Task<MgtAppClient?> GetMgtAppClient([GraphQLName("query")] MgtappClientQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query);
            var result = await ctx.Clients.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public static FilterDefinition<MgtAppClient> BuildFilter(MgtappClientQueryInput? query)
        {
            if (query == null)
            {
                return Builders<MgtAppClient>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppClient>>();

            // Primitive field filters
            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppClient>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.clientname))
            {
                filters.Add(Builders<MgtAppClient>.Filter.Eq(x => x.clientname, query.clientname));
            }

            // StringQueryInput for clientname
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

            // Logical groups
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

            // No filters means match all
            if (!filters.Any())
            {
                return Builders<MgtAppClient>.Filter.Empty;
            }

            // Combine accumulated filters
            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppClient>.Filter.And(filters);
        }
    }
}
