using MongoDB.Driver;
using MongoDB.Bson;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types.MgtAppBankDetailsTypes;
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
    public class MgtAppBankDetailsQuery
    {
        [GraphQLName("mgtappBankDetails")]
        public async Task<List<MgtAppBankDetails>> GetMgtAppBankDetails([GraphQLName("query")] MgtappBankDetailsQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.BankDetails.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappBankDetail")]
        public async Task<MgtAppBankDetails?> GetMgtAppBankDetail([GraphQLName("query")] MgtappBankDetailsQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.BankDetails.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public static FilterDefinition<MgtAppBankDetails> BuildFilter(MgtappBankDetailsQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppBankDetails>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppBankDetails>>();

            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppBankDetails>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.bankstatus))
            {
                filters.Add(Builders<MgtAppBankDetails>.Filter.Eq(x => x.bankstatus, query.bankstatus));
            }
            if (!string.IsNullOrWhiteSpace(query.bankroutingno))
            {
                filters.Add(Builders<MgtAppBankDetails>.Filter.Eq(x => x.bankroutingno, query.bankroutingno));
            }
            if (!string.IsNullOrWhiteSpace(query.bankaccountno))
            {
                filters.Add(Builders<MgtAppBankDetails>.Filter.Eq(x => x.bankaccountno, query.bankaccountno));
            }

            void ApplyStringQuery(StringQueryInput? q, System.Linq.Expressions.Expression<Func<MgtAppBankDetails, string?>> fieldExpr)
            {
                if (q == null) return;
                var field = new ExpressionFieldDefinition<MgtAppBankDetails, string?>(fieldExpr);
                var fq = new List<FilterDefinition<MgtAppBankDetails>>();
                if (q.eq != null) fq.Add(Builders<MgtAppBankDetails>.Filter.Eq(field, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppBankDetails>.Filter.Ne(field, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppBankDetails>.Filter.In(field, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppBankDetails>.Filter.Nin(field, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppBankDetails>.Filter.Regex(field, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppBankDetails>.Filter.And(fq));
            }

            ApplyStringQuery(query.bankstatusQuery, x => x.bankstatus);
            ApplyStringQuery(query.bankroutingnoQuery, x => x.bankroutingno);
            ApplyStringQuery(query.bankaccountnoQuery, x => x.bankaccountno);

            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppBankDetails>.Filter.In(b => b.clientid, clientIds));
            }

            if (query.employeeid != null)
            {
                var empFilter = MgtAppEmployeeQuery.BuildFilter(query.employeeid, ctx);
                var empIds = ctx.Employees.Find(empFilter).Project(e => e._id).ToList();
                filters.Add(Builders<MgtAppBankDetails>.Filter.In(b => b.employeeid, empIds));
            }

            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppBankDetails>.Filter.And(andFilters));
            }

            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppBankDetails>.Filter.Or(orFilters));
            }

            if (!filters.Any())
            {
                return Builders<MgtAppBankDetails>.Filter.Empty;
            }

            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppBankDetails>.Filter.And(filters);
        }
    }
}