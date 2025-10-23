using MongoDB.Driver;
using MongoDB.Bson;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types.MgtAppTimesheetsTypes;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class MgtAppTimesheetsQuery
    {
        [GraphQLName("mgtappTimesheets")]
        public async Task<List<MgtAppTimesheets>> GetMgtAppTimesheets([GraphQLName("query")] MgtAppTimesheetsQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Timesheets.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappTimesheet")]
        public async Task<MgtAppTimesheets?> GetMgtAppTimesheet([GraphQLName("query")] MgtAppTimesheetsQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Timesheets.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public static FilterDefinition<MgtAppTimesheets> BuildFilter(MgtAppTimesheetsQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppTimesheets>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppTimesheets>>();

            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppTimesheets>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.timesheetmonth))
            {
                filters.Add(Builders<MgtAppTimesheets>.Filter.Eq(x => x.timesheetmonth, query.timesheetmonth));
            }

            void ApplyStringQuery(StringQueryInput? q, System.Linq.Expressions.Expression<Func<MgtAppTimesheets, string?>> fieldExpr)
            {
                if (q == null) return;
                var field = new ExpressionFieldDefinition<MgtAppTimesheets, string?>(fieldExpr);
                var fq = new List<FilterDefinition<MgtAppTimesheets>>();
                if (q.eq != null) fq.Add(Builders<MgtAppTimesheets>.Filter.Eq(field, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppTimesheets>.Filter.Ne(field, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppTimesheets>.Filter.In(field, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppTimesheets>.Filter.Nin(field, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppTimesheets>.Filter.Regex(field, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppTimesheets>.Filter.And(fq));
            }

            ApplyStringQuery(query.timesheetmonthQuery, x => x.timesheetmonth);

            // Handle nested timesheetinfo filtering
            if (query.timesheetinfo != null)
            {
                var timesheetInfoFilters = new List<FilterDefinition<MgtAppTimesheets>>();

                if (!string.IsNullOrWhiteSpace(query.timesheetinfo.timesheethours))
                {
                    timesheetInfoFilters.Add(Builders<MgtAppTimesheets>.Filter.ElemMatch(
                        x => x.timesheetinfo, 
                        Builders<TimesheetInfo>.Filter.Eq(t => t.timesheethours, query.timesheetinfo.timesheethours)));
                }

                if (!string.IsNullOrWhiteSpace(query.timesheetinfo.timesheetdate))
                {
                    timesheetInfoFilters.Add(Builders<MgtAppTimesheets>.Filter.ElemMatch(
                        x => x.timesheetinfo, 
                        Builders<TimesheetInfo>.Filter.Eq(t => t.timesheetdate, query.timesheetinfo.timesheetdate)));
                }

                if (query.timesheetinfo.timesheethoursQuery != null)
                {
                    var hoursQuery = query.timesheetinfo.timesheethoursQuery;
                    var hoursFilters = new List<FilterDefinition<TimesheetInfo>>();
                    
                    if (hoursQuery.eq != null) hoursFilters.Add(Builders<TimesheetInfo>.Filter.Eq(t => t.timesheethours, hoursQuery.eq));
                    if (hoursQuery.ne != null) hoursFilters.Add(Builders<TimesheetInfo>.Filter.Ne(t => t.timesheethours, hoursQuery.ne));
                    if (hoursQuery.@in != null && hoursQuery.@in.Count > 0) hoursFilters.Add(Builders<TimesheetInfo>.Filter.In(t => t.timesheethours, hoursQuery.@in));
                    if (hoursQuery.nin != null && hoursQuery.nin.Count > 0) hoursFilters.Add(Builders<TimesheetInfo>.Filter.Nin(t => t.timesheethours, hoursQuery.nin));
                    if (hoursQuery.regex != null)
                    {
                        var regex = new Regex(hoursQuery.regex, RegexOptions.IgnoreCase);
                        hoursFilters.Add(Builders<TimesheetInfo>.Filter.Regex(t => t.timesheethours, new BsonRegularExpression(regex)));
                    }

                    if (hoursFilters.Count > 0)
                    {
                        timesheetInfoFilters.Add(Builders<MgtAppTimesheets>.Filter.ElemMatch(
                            x => x.timesheetinfo, 
                            Builders<TimesheetInfo>.Filter.And(hoursFilters)));
                    }
                }

                if (query.timesheetinfo.timesheetdateQuery != null)
                {
                    var dateQuery = query.timesheetinfo.timesheetdateQuery;
                    var dateFilters = new List<FilterDefinition<TimesheetInfo>>();
                    
                    if (dateQuery.eq != null) dateFilters.Add(Builders<TimesheetInfo>.Filter.Eq(t => t.timesheetdate, dateQuery.eq));
                    if (dateQuery.ne != null) dateFilters.Add(Builders<TimesheetInfo>.Filter.Ne(t => t.timesheetdate, dateQuery.ne));
                    if (dateQuery.@in != null && dateQuery.@in.Count > 0) dateFilters.Add(Builders<TimesheetInfo>.Filter.In(t => t.timesheetdate, dateQuery.@in));
                    if (dateQuery.nin != null && dateQuery.nin.Count > 0) dateFilters.Add(Builders<TimesheetInfo>.Filter.Nin(t => t.timesheetdate, dateQuery.nin));
                    if (dateQuery.regex != null)
                    {
                        var regex = new Regex(dateQuery.regex, RegexOptions.IgnoreCase);
                        dateFilters.Add(Builders<TimesheetInfo>.Filter.Regex(t => t.timesheetdate, new BsonRegularExpression(regex)));
                    }

                    if (dateFilters.Count > 0)
                    {
                        timesheetInfoFilters.Add(Builders<MgtAppTimesheets>.Filter.ElemMatch(
                            x => x.timesheetinfo, 
                            Builders<TimesheetInfo>.Filter.And(dateFilters)));
                    }
                }

                if (timesheetInfoFilters.Count > 0)
                {
                    filters.Add(Builders<MgtAppTimesheets>.Filter.And(timesheetInfoFilters));
                }
            }

            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppTimesheets>.Filter.In(t => t.clientid, clientIds));
            }

            if (query.employeeid != null)
            {
                var empFilter = MgtAppEmployeeQuery.BuildFilter(query.employeeid, ctx);
                var empIds = ctx.Employees.Find(empFilter).Project(e => e._id).ToList();
                filters.Add(Builders<MgtAppTimesheets>.Filter.In(t => t.employeeid, empIds));
            }

            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppTimesheets>.Filter.And(andFilters));
            }

            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppTimesheets>.Filter.Or(orFilters));
            }

            if (!filters.Any())
            {
                return Builders<MgtAppTimesheets>.Filter.Empty;
            }

            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppTimesheets>.Filter.And(filters);
        }
    }
}