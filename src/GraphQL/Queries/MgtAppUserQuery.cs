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

            // NEW: nested role filter support
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