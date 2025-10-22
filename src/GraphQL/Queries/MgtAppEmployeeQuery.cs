using MongoDB.Driver;
using MongoDB.Bson;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class MgtAppEmployeeQuery
    {
        [GraphQLName("mgtappEmployees")]
        public async Task<List<MgtAppEmployee>> GetMgtAppEmployees([GraphQLName("query")] MgtAppEmployeeQueryInput? query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Employees.Find(filter).ToListAsync();
            return result;
        }

        [GraphQLName("mgtappEmployee")]
        public async Task<MgtAppEmployee?> GetMgtAppEmployee([GraphQLName("query")] MgtAppEmployeeQueryInput query, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Employees.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public static FilterDefinition<MgtAppEmployee> BuildFilter(MgtAppEmployeeQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppEmployee>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppEmployee>>();

            // Primitive equality filters
            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.employeephone))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeephone, query.employeephone));
            }
            if (!string.IsNullOrWhiteSpace(query.employeefirstname))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeefirstname, query.employeefirstname));
            }
            if (!string.IsNullOrWhiteSpace(query.employeesalaryrate))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeesalaryrate, query.employeesalaryrate));
            }
            if (!string.IsNullOrWhiteSpace(query.employeeworkemail))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeeworkemail, query.employeeworkemail));
            }
            if (!string.IsNullOrWhiteSpace(query.employeeexpirydate))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeeexpirydate, query.employeeexpirydate));
            }
            if (!string.IsNullOrWhiteSpace(query.employeelastname))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeelastname, query.employeelastname));
            }
            if (!string.IsNullOrWhiteSpace(query.employeedob))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeedob, query.employeedob));
            }
            if (!string.IsNullOrWhiteSpace(query.employeevisastatus))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeevisastatus, query.employeevisastatus));
            }
            if (!string.IsNullOrWhiteSpace(query.employeeemail))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeeemail, query.employeeemail));
            }
            if (!string.IsNullOrWhiteSpace(query.employeevendor))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeevendor, query.employeevendor));
            }
            if (!string.IsNullOrWhiteSpace(query.employeetype))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeetype, query.employeetype));
            }
            if (!string.IsNullOrWhiteSpace(query.employeebillrate))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeebillrate, query.employeebillrate));
            }
            if (!string.IsNullOrWhiteSpace(query.employeesubstatus))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeesubstatus, query.employeesubstatus));
            }
            if (!string.IsNullOrWhiteSpace(query.employeestatus))
            {
                filters.Add(Builders<MgtAppEmployee>.Filter.Eq(x => x.employeestatus, query.employeestatus));
            }

            // StringQueryInput filters
            void ApplyStringQuery(StringQueryInput? q, System.Linq.Expressions.Expression<System.Func<MgtAppEmployee, string?>> fieldExpr)
            {
                if (q == null) return;
                var field = new ExpressionFieldDefinition<MgtAppEmployee, string?>(fieldExpr);
                var fq = new List<FilterDefinition<MgtAppEmployee>>();
                if (q.eq != null) fq.Add(Builders<MgtAppEmployee>.Filter.Eq(field, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppEmployee>.Filter.Ne(field, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppEmployee>.Filter.In(field, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppEmployee>.Filter.Nin(field, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppEmployee>.Filter.Regex(field, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppEmployee>.Filter.And(fq));
            }

            ApplyStringQuery(query.employeephoneQuery, x => x.employeephone);
            ApplyStringQuery(query.employeefirstnameQuery, x => x.employeefirstname);
            ApplyStringQuery(query.employeesalaryrateQuery, x => x.employeesalaryrate);
            ApplyStringQuery(query.employeeworkemailQuery, x => x.employeeworkemail);
            ApplyStringQuery(query.employeeexpirydateQuery, x => x.employeeexpirydate);
            ApplyStringQuery(query.employeelastnameQuery, x => x.employeelastname);
            ApplyStringQuery(query.employeedobQuery, x => x.employeedob);
            ApplyStringQuery(query.employeevisastatusQuery, x => x.employeevisastatus);
            ApplyStringQuery(query.employeeemailQuery, x => x.employeeemail);
            ApplyStringQuery(query.employeevendorQuery, x => x.employeevendor);
            ApplyStringQuery(query.employeetypeQuery, x => x.employeetype);
            ApplyStringQuery(query.employeebillrateQuery, x => x.employeebillrate);
            ApplyStringQuery(query.employeesubstatusQuery, x => x.employeesubstatus);
            ApplyStringQuery(query.employeestatusQuery, x => x.employeestatus);

            // Nested client filter
            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppEmployee>.Filter.In(e => e.clientid, clientIds));
            }

            // Nested user filters for employeeuserid and employeemanagerid
            if (query.employeeuserid != null)
            {
                var userFilter = BuildUserFilter(query.employeeuserid, ctx);
                var userIds = ctx.Users.Find(userFilter).Project(u => u._id).ToList();
                filters.Add(Builders<MgtAppEmployee>.Filter.In(e => e.employeeuserid, userIds));
            }

            if (query.employeemanagerid != null)
            {
                var mgrFilter = BuildUserFilter(query.employeemanagerid, ctx);
                var mgrIds = ctx.Users.Find(mgrFilter).Project(u => u._id).ToList();
                filters.Add(Builders<MgtAppEmployee>.Filter.In(e => e.employeemanagerid, mgrIds));
            }

            // Logical groups
            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppEmployee>.Filter.And(andFilters));
            }

            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppEmployee>.Filter.Or(orFilters));
            }

            if (!filters.Any())
            {
                return Builders<MgtAppEmployee>.Filter.Empty;
            }

            if (filters.Count == 1)
            {
                return filters[0];
            }

            return Builders<MgtAppEmployee>.Filter.And(filters);
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

            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppUser>.Filter.In(u => u.clientid, clientIds));
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