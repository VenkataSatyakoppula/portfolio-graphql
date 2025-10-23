using MongoDB.Driver;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppProfileTypes;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class MgtAppTicketQuery
    {
        [GraphQLName("mgtappTickets")]
        public async Task<List<MgtAppTicket>> GetMgtAppTickets([GraphQLName("query")] MgtAppTicketQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Tickets.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappTicket")]
        public async Task<MgtAppTicket?> GetMgtAppTicket([GraphQLName("query")] MgtAppTicketQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Tickets.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public static FilterDefinition<MgtAppTicket> BuildFilter(MgtAppTicketQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppTicket>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppTicket>>();

            // String-based _id, matching other collection implementations
            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppTicket>.Filter.Eq(x => x._id, query._id));
            }

            // Direct string equality filters for ticket fields
            if (!string.IsNullOrWhiteSpace(query.ticketcreateddate))
            {
                filters.Add(Builders<MgtAppTicket>.Filter.Eq(x => x.ticketcreateddate, query.ticketcreateddate));
            }
            if (!string.IsNullOrWhiteSpace(query.timesheetweek))
            {
                filters.Add(Builders<MgtAppTicket>.Filter.Eq(x => x.timesheetweek, query.timesheetweek));
            }
            if (!string.IsNullOrWhiteSpace(query.tickettype))
            {
                filters.Add(Builders<MgtAppTicket>.Filter.Eq(x => x.tickettype, query.tickettype));
            }
            if (!string.IsNullOrWhiteSpace(query.ticketstatus))
            {
                filters.Add(Builders<MgtAppTicket>.Filter.Eq(x => x.ticketstatus, query.ticketstatus));
            }
            if (!string.IsNullOrWhiteSpace(query.ticketcategory))
            {
                filters.Add(Builders<MgtAppTicket>.Filter.Eq(x => x.ticketcategory, query.ticketcategory));
            }

            // Nested profile filter (limited to _id support to avoid duplicating full profile filters)
            if (query.profileid != null)
            {
                var profileIds = new List<string>();
                if (!string.IsNullOrWhiteSpace(query.profileid._id))
                {
                    profileIds.Add(query.profileid._id);
                }
                if (profileIds.Count > 0)
                {
                    filters.Add(Builders<MgtAppTicket>.Filter.In(t => t.profileid, profileIds));
                }
            }

            // Nested user filters for createdby and assignedto
            if (query.ticketcreatedby != null)
            {
                var userFilter = BuildUserFilter(query.ticketcreatedby, ctx);
                var userIds = ctx.Users.Find(userFilter).Project(u => u._id).ToList();
                if (userIds.Count > 0)
                {
                    filters.Add(Builders<MgtAppTicket>.Filter.In(t => t.ticketcreatedby, userIds));
                }
            }

            if (query.ticketassignedto != null)
            {
                var userFilter = BuildUserFilter(query.ticketassignedto, ctx);
                var userIds = ctx.Users.Find(userFilter).Project(u => u._id).ToList();
                if (userIds.Count > 0)
                {
                    filters.Add(Builders<MgtAppTicket>.Filter.In(t => t.ticketassignedto, userIds));
                }
            }

            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppTicket>.Filter.And(andFilters));
            }

            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppTicket>.Filter.Or(orFilters));
            }

            if (!filters.Any())
            {
                return Builders<MgtAppTicket>.Filter.Empty;
            }

            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppTicket>.Filter.And(filters);
        }

        private static FilterDefinition<MgtAppUser> BuildUserFilter(MgtAppUserQueryInput? query, MongoDbContext ctx)
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
                    fq.Add(Builders<MgtAppUser>.Filter.Regex(x => x.username, new MongoDB.Bson.BsonRegularExpression(regex)));
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
                    fq.Add(Builders<MgtAppUser>.Filter.Regex(x => x.useremail, new MongoDB.Bson.BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppUser>.Filter.And(fq));
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