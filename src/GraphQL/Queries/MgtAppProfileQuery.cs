using MongoDB.Driver;
using MongoDB.Bson;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types.MgtAppProfileTypes;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppPositionTypes;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class MgtAppProfileQuery
    {
        [GraphQLName("mgtappProfiles")]
        public async Task<List<MgtAppProfile>> GetMgtAppProfiles([GraphQLName("query")] MgtAppProfileQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Profiles.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappProfile")]
        public async Task<MgtAppProfile?> GetMgtAppProfile([GraphQLName("query")] MgtAppProfileQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Profiles.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        private static FilterDefinition<MgtAppProfile> BuildFilter(MgtAppProfileQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppProfile>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppProfile>>();

            // Primitive equality filters
            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.resume))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.resume, query.resume));
            }
            if (!string.IsNullOrWhiteSpace(query.profilevisastatus))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilevisastatus, query.profilevisastatus));
            }
            if (!string.IsNullOrWhiteSpace(query.profilerate))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilerate, query.profilerate));
            }
            if (!string.IsNullOrWhiteSpace(query.profilelastname))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilelastname, query.profilelastname));
            }
            if (!string.IsNullOrWhiteSpace(query.profilefirstname))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilefirstname, query.profilefirstname));
            }
            if (!string.IsNullOrWhiteSpace(query.profileemail))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profileemail, query.profileemail));
            }
            if (!string.IsNullOrWhiteSpace(query.profiletype))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profiletype, query.profiletype));
            }
            if (!string.IsNullOrWhiteSpace(query.profileexpirydate))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profileexpirydate, query.profileexpirydate));
            }
            if (!string.IsNullOrWhiteSpace(query.profiledob))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profiledob, query.profiledob));
            }
            if (!string.IsNullOrWhiteSpace(query.profilestatus))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilestatus, query.profilestatus));
            }
            if (!string.IsNullOrWhiteSpace(query.profilephone))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilephone, query.profilephone));
            }
            if (!string.IsNullOrWhiteSpace(query.profilevendor))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilevendor, query.profilevendor));
            }
            if (!string.IsNullOrWhiteSpace(query.profilecomments))
            {
                filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilecomments, query.profilecomments));
            }

            // StringQueryInput filters
            void ApplyStringQuery(StringQueryInput? q, System.Linq.Expressions.Expression<Func<MgtAppProfile, string?>> fieldExpr)
            {
                if (q == null) return;
                var field = new ExpressionFieldDefinition<MgtAppProfile, string?>(fieldExpr);
                var fq = new List<FilterDefinition<MgtAppProfile>>();
                if (q.eq != null) fq.Add(Builders<MgtAppProfile>.Filter.Eq(field, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppProfile>.Filter.Ne(field, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppProfile>.Filter.In(field, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppProfile>.Filter.Nin(field, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppProfile>.Filter.Regex(field, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppProfile>.Filter.And(fq));
            }

            ApplyStringQuery(query.resumeQuery, x => x.resume);
            ApplyStringQuery(query.profilevisastatusQuery, x => x.profilevisastatus);
            ApplyStringQuery(query.profilerateQuery, x => x.profilerate);
            ApplyStringQuery(query.profilelastnameQuery, x => x.profilelastname);
            ApplyStringQuery(query.profilefirstnameQuery, x => x.profilefirstname);
            ApplyStringQuery(query.profileemailQuery, x => x.profileemail);
            ApplyStringQuery(query.profiletypeQuery, x => x.profiletype);
            ApplyStringQuery(query.profileexpirydateQuery, x => x.profileexpirydate);
            ApplyStringQuery(query.profiledobQuery, x => x.profiledob);
            ApplyStringQuery(query.profilestatusQuery, x => x.profilestatus);
            ApplyStringQuery(query.profilephoneQuery, x => x.profilephone);
            ApplyStringQuery(query.profilevendorQuery, x => x.profilevendor);
            ApplyStringQuery(query.profilecommentsQuery, x => x.profilecomments);

            // Nested client filter (renamed to clientid)
            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppProfile>.Filter.In(p => p.clientid, clientIds));
            }

            // Nested position filter (renamed to positionid)
            if (query.positionid != null)
            {
                var posFilter = MgtAppPositionQuery.BuildFilter(query.positionid, ctx);
                var posIds = ctx.Positions.Find(posFilter).Project(p => p._id).ToList();
                filters.Add(Builders<MgtAppProfile>.Filter.In(p => p.positionid, posIds));
            }

            // Logical groups
            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppProfile>.Filter.And(andFilters));
            }

            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppProfile>.Filter.Or(orFilters));
            }

            if (!filters.Any())
            {
                return Builders<MgtAppProfile>.Filter.Empty;
            }

            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppProfile>.Filter.And(filters);
        }
    }
}