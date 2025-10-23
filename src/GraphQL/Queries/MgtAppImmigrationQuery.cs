using MongoDB.Driver;
using MongoDB.Bson;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types.MgtAppImmigrationTypes;
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
    public class MgtAppImmigrationQuery
    {
        [GraphQLName("mgtappImmigrations")]
        public async Task<List<MgtAppImmigration>> GetMgtAppImmigrations([GraphQLName("query")] MgtappImmigrationQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Immigrations.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappImmigration")]
        public async Task<MgtAppImmigration?> GetMgtAppImmigration([GraphQLName("query")] MgtappImmigrationQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Immigrations.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public static FilterDefinition<MgtAppImmigration> BuildFilter(MgtappImmigrationQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppImmigration>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppImmigration>>();

            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppImmigration>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.immigrationstatus))
            {
                filters.Add(Builders<MgtAppImmigration>.Filter.Eq(x => x.immigrationstatus, query.immigrationstatus));
            }
            if (!string.IsNullOrWhiteSpace(query.immigrationsubstatus))
            {
                filters.Add(Builders<MgtAppImmigration>.Filter.Eq(x => x.immigrationsubstatus, query.immigrationsubstatus));
            }

            void ApplyStringQuery(StringQueryInput? q, System.Linq.Expressions.Expression<Func<MgtAppImmigration, string?>> fieldExpr)
            {
                if (q == null) return;
                var field = new ExpressionFieldDefinition<MgtAppImmigration, string?>(fieldExpr);
                var fq = new List<FilterDefinition<MgtAppImmigration>>();
                if (q.eq != null) fq.Add(Builders<MgtAppImmigration>.Filter.Eq(field, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppImmigration>.Filter.Ne(field, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppImmigration>.Filter.In(field, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppImmigration>.Filter.Nin(field, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppImmigration>.Filter.Regex(field, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppImmigration>.Filter.And(fq));
            }

            ApplyStringQuery(query.immigrationstatusQuery, x => x.immigrationstatus);
            ApplyStringQuery(query.immigrationsubstatusQuery, x => x.immigrationsubstatus);

            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppImmigration>.Filter.In(i => i.clientid, clientIds));
            }

            if (query.employeeid != null)
            {
                var empFilter = MgtAppEmployeeQuery.BuildFilter(query.employeeid, ctx);
                var empIds = ctx.Employees.Find(empFilter).Project(e => e._id).ToList();
                filters.Add(Builders<MgtAppImmigration>.Filter.In(i => i.employeeid, empIds));
            }

            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppImmigration>.Filter.And(andFilters));
            }

            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppImmigration>.Filter.Or(orFilters));
            }

            if (!filters.Any())
            {
                return Builders<MgtAppImmigration>.Filter.Empty;
            }

            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppImmigration>.Filter.And(filters);
        }
    }
}