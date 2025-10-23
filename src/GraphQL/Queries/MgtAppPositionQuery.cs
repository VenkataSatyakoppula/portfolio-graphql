using MongoDB.Driver;
using MongoDB.Bson;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types.MgtAppPositionTypes;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class MgtAppPositionQuery
    {
        [GraphQLName("mgtappPositions")]
        public async Task<List<MgtAppPosition>> GetMgtAppPositions([GraphQLName("query")] MgtappPositionQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Positions.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappPosition")]
        public async Task<MgtAppPosition?> GetMgtAppPosition([GraphQLName("query")] MgtappPositionQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Positions.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public static FilterDefinition<MgtAppPosition> BuildFilter(MgtappPositionQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppPosition>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppPosition>>();

            // Primitive field filters
            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.groupid))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.groupid, query.groupid));
            }
            if (!string.IsNullOrWhiteSpace(query.jobtitle))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.jobtitle, query.jobtitle));
            }
            if (!string.IsNullOrWhiteSpace(query.experience))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.experience, query.experience));
            }
            if (!string.IsNullOrWhiteSpace(query.skillset))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.skillset, query.skillset));
            }
            if (!string.IsNullOrWhiteSpace(query.billingrate))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.billingrate, query.billingrate));
            }
            if (!string.IsNullOrWhiteSpace(query.status))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.status, query.status));
            }

            // StringQueryInput for various fields
            if (query.jobtitleQuery != null)
            {
                var q = query.jobtitleQuery;
                var fq = new List<FilterDefinition<MgtAppPosition>>();
                if (q.eq != null) fq.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.jobtitle, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppPosition>.Filter.Ne(x => x.jobtitle, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.In(x => x.jobtitle, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.Nin(x => x.jobtitle, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppPosition>.Filter.Regex(x => x.jobtitle, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppPosition>.Filter.And(fq));
            }

            if (query.experienceQuery != null)
            {
                var q = query.experienceQuery;
                var fq = new List<FilterDefinition<MgtAppPosition>>();
                if (q.eq != null) fq.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.experience, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppPosition>.Filter.Ne(x => x.experience, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.In(x => x.experience, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.Nin(x => x.experience, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppPosition>.Filter.Regex(x => x.experience, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppPosition>.Filter.And(fq));
            }

            if (query.skillsetQuery != null)
            {
                var q = query.skillsetQuery;
                var fq = new List<FilterDefinition<MgtAppPosition>>();
                if (q.eq != null) fq.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.skillset, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppPosition>.Filter.Ne(x => x.skillset, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.In(x => x.skillset, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.Nin(x => x.skillset, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppPosition>.Filter.Regex(x => x.skillset, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppPosition>.Filter.And(fq));
            }

            if (query.billingrateQuery != null)
            {
                var q = query.billingrateQuery;
                var fq = new List<FilterDefinition<MgtAppPosition>>();
                if (q.eq != null) fq.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.billingrate, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppPosition>.Filter.Ne(x => x.billingrate, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.In(x => x.billingrate, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.Nin(x => x.billingrate, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppPosition>.Filter.Regex(x => x.billingrate, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppPosition>.Filter.And(fq));
            }

            if (query.statusQuery != null)
            {
                var q = query.statusQuery;
                var fq = new List<FilterDefinition<MgtAppPosition>>();
                if (q.eq != null) fq.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.status, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppPosition>.Filter.Ne(x => x.status, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.In(x => x.status, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.Nin(x => x.status, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppPosition>.Filter.Regex(x => x.status, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppPosition>.Filter.And(fq));
            }

            // Nested client filters using MgtAppClientQueryInput (renamed to clientid)
            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppPosition>.Filter.In(p => p.clientid, clientIds));
            }

            // Logical groups
            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppPosition>.Filter.And(andFilters));
            }

            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppPosition>.Filter.Or(orFilters));
            }

            // No filters means match all
            if (!filters.Any())
            {
                return Builders<MgtAppPosition>.Filter.Empty;
            }

            // Combine accumulated filters
            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppPosition>.Filter.And(filters);
        }
    }
}