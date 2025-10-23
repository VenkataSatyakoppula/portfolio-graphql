using MongoDB.Driver;
using MongoDB.Bson;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Queries; // for MgtAppClientQuery
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class MgtAppUserQuery
    {
        [GraphQLName("mgtappUsers")]
        public async Task<List<MgtAppUser>> GetMgtAppUsers([GraphQLName("query")] MgtappUserQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Users.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappUser")]
        public async Task<MgtAppUser?> GetMgtAppUser([GraphQLName("query")] MgtappUserQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Users.Find(filter).FirstOrDefaultAsync();
            return result;
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
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
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
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppUser>.Filter.Regex(x => x.useremail, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppUser>.Filter.And(fq));
            }


            // Support nested client object-based filtering using MgtAppClientQueryInput by reusing MgtAppClientQuery.BuildFilter
            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppUser>.Filter.In(u => u.clientid, clientIds));
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