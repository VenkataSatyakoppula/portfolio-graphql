using MongoDB.Driver;
using MongoDB.Bson;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types.MgtAppInsuranceTypes;
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
    public class MgtAppInsuranceQuery
    {
        [GraphQLName("mgtappInsurances")]
        public async Task<List<MgtAppInsurance>> GetMgtAppInsurances([GraphQLName("query")] MgtappInsuranceQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Insurances.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappInsurance")]
        public async Task<MgtAppInsurance?> GetMgtAppInsurance([GraphQLName("query")] MgtappInsuranceQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Insurances.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public static FilterDefinition<MgtAppInsurance> BuildFilter(MgtappInsuranceQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppInsurance>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppInsurance>>();

            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppInsurance>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.insurancestatus))
            {
                filters.Add(Builders<MgtAppInsurance>.Filter.Eq(x => x.insurancestatus, query.insurancestatus));
            }
            if (!string.IsNullOrWhiteSpace(query.insurancesubstatus))
            {
                filters.Add(Builders<MgtAppInsurance>.Filter.Eq(x => x.insurancesubstatus, query.insurancesubstatus));
            }

            void ApplyStringQuery(StringQueryInput? q, System.Linq.Expressions.Expression<Func<MgtAppInsurance, string?>> fieldExpr)
            {
                if (q == null) return;
                var field = new ExpressionFieldDefinition<MgtAppInsurance, string?>(fieldExpr);
                var fq = new List<FilterDefinition<MgtAppInsurance>>();
                if (q.eq != null) fq.Add(Builders<MgtAppInsurance>.Filter.Eq(field, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppInsurance>.Filter.Ne(field, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppInsurance>.Filter.In(field, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppInsurance>.Filter.Nin(field, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppInsurance>.Filter.Regex(field, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppInsurance>.Filter.And(fq));
            }

            ApplyStringQuery(query.insurancestatusQuery, x => x.insurancestatus);
            ApplyStringQuery(query.insurancesubstatusQuery, x => x.insurancesubstatus);

            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppInsurance>.Filter.In(i => i.clientid, clientIds));
            }

            if (query.employeeid != null)
            {
                var empFilter = MgtAppEmployeeQuery.BuildFilter(query.employeeid, ctx);
                var empIds = ctx.Employees.Find(empFilter).Project(e => e._id).ToList();
                filters.Add(Builders<MgtAppInsurance>.Filter.In(i => i.employeeid, empIds));
            }

            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppInsurance>.Filter.And(andFilters));
            }

            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppInsurance>.Filter.Or(orFilters));
            }

            if (!filters.Any())
            {
                return Builders<MgtAppInsurance>.Filter.Empty;
            }

            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppInsurance>.Filter.And(filters);
        }
    }
}