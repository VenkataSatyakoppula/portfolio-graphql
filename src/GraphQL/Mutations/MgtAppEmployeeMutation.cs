using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Queries; // for MgtAppEmployeeQuery.BuildFilter
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppEmployeesPayload")]
    public class DeleteManyMgtAppEmployeesPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppEmployee documents deleted.")]
        public int deletedCount { get; set; }
    }

    [GraphQLName("UpdateManyMgtAppEmployeesPayload")]
    public class UpdateManyMgtAppEmployeesPayload
    {
        [GraphQLName("modifiedCount")]
        [GraphQLDescription("Number of MgtAppEmployee documents modified.")]
        public int modifiedCount { get; set; }
    }

    [ExtendObjectType("Mutation")]
    public class MgtAppEmployeeMutation
    {
        [GraphQLName("insertOneMgtappEmployee")]
        public async Task<MgtAppEmployee> InsertOneMgtAppEmployee(MgtAppEmployeeInsertInput input, [Service] MongoDbContext ctx)
        {
            var id = ObjectId.GenerateNewId().ToString();

            if (input.clientid == null || string.IsNullOrWhiteSpace(input.clientid.link))
            {
                throw new GraphQLException("clientid.link is required.");
            }

            var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, input.clientid.link)).FirstOrDefaultAsync();
            if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");

            string? employeeUserId = null;
            if (input.employeeuserid != null)
            {
                if (string.IsNullOrWhiteSpace(input.employeeuserid.link))
                {
                    throw new GraphQLException("employeeuserid.link is required when provided.");
                }
                var user = await ctx.Users.Find(Builders<MgtAppUser>.Filter.Eq(x => x._id, input.employeeuserid.link)).FirstOrDefaultAsync();
                if (user == null) throw new GraphQLException("Invalid employeeuserid.link: user not found.");
                employeeUserId = user._id;
            }

            string? managerUserId = null;
            if (input.employeemanagerid != null)
            {
                if (string.IsNullOrWhiteSpace(input.employeemanagerid.link))
                {
                    throw new GraphQLException("employeemanagerid.link is required when provided.");
                }
                var mgr = await ctx.Users.Find(Builders<MgtAppUser>.Filter.Eq(x => x._id, input.employeemanagerid.link)).FirstOrDefaultAsync();
                if (mgr == null) throw new GraphQLException("Invalid employeemanagerid.link: user not found.");
                managerUserId = mgr._id;
            }

            var doc = new MgtAppEmployee
            {
                _id = id,
                clientid = client._id,
                employeeuserid = employeeUserId,
                employeemanagerid = managerUserId,
                employeephone = input.employeephone,
                employeefirstname = input.employeefirstname,
                employeesalaryrate = input.employeesalaryrate,
                employeeworkemail = input.employeeworkemail,
                employeeexpirydate = input.employeeexpirydate,
                employeelastname = input.employeelastname,
                employeedob = input.employeedob,
                employeevisastatus = input.employeevisastatus,
                employeeemail = input.employeeemail,
                employeevendor = input.employeevendor,
                employeetype = input.employeetype,
                employeebillrate = input.employeebillrate,
                employeesubstatus = input.employeesubstatus,
                employeestatus = input.employeestatus
            };

            await ctx.Employees.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappEmployee")]
        public async Task<MgtAppEmployee?> UpdateOneMgtAppEmployee([GraphQLName("query")] MgtappEmployeeQueryInput query, MgtAppEmployeeSetInput set, [Service] MongoDbContext ctx)
        {
            var filter = MgtAppEmployeeQuery.BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppEmployee>>();

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.clientid, client._id));
            }

            if (set.employeeuserid != null)
            {
                if (string.IsNullOrWhiteSpace(set.employeeuserid.link))
                {
                    throw new GraphQLException("employeeuserid.link is required when provided.");
                }
                var user = await ctx.Users.Find(Builders<MgtAppUser>.Filter.Eq(x => x._id, set.employeeuserid.link)).FirstOrDefaultAsync();
                if (user == null) throw new GraphQLException("Invalid employeeuserid.link: user not found.");
                updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeeuserid, user._id));
            }

            if (set.employeemanagerid != null)
            {
                if (string.IsNullOrWhiteSpace(set.employeemanagerid.link))
                {
                    throw new GraphQLException("employeemanagerid.link is required when provided.");
                }
                var mgr = await ctx.Users.Find(Builders<MgtAppUser>.Filter.Eq(x => x._id, set.employeemanagerid.link)).FirstOrDefaultAsync();
                if (mgr == null) throw new GraphQLException("Invalid employeemanagerid.link: user not found.");
                updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeemanagerid, mgr._id));
            }

            if (set.employeephone != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeephone, set.employeephone));
            if (set.employeefirstname != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeefirstname, set.employeefirstname));
            if (set.employeesalaryrate != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeesalaryrate, set.employeesalaryrate));
            if (set.employeeworkemail != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeeworkemail, set.employeeworkemail));
            if (set.employeeexpirydate != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeeexpirydate, set.employeeexpirydate));
            if (set.employeelastname != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeelastname, set.employeelastname));
            if (set.employeedob != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeedob, set.employeedob));
            if (set.employeevisastatus != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeevisastatus, set.employeevisastatus));
            if (set.employeeemail != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeeemail, set.employeeemail));
            if (set.employeevendor != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeevendor, set.employeevendor));
            if (set.employeetype != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeetype, set.employeetype));
            if (set.employeebillrate != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeebillrate, set.employeebillrate));
            if (set.employeesubstatus != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeesubstatus, set.employeesubstatus));
            if (set.employeestatus != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeestatus, set.employeestatus));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppEmployee>.Update.Combine(updates);
            var options = new FindOneAndUpdateOptions<MgtAppEmployee> { ReturnDocument = ReturnDocument.After };
            var result = await ctx.Employees.FindOneAndUpdateAsync(filter, combinedUpdate, options);
            return result;
        }

        [GraphQLName("deleteOneMgtappEmployee")]
        public async Task<MgtAppEmployee?> DeleteOneMgtAppEmployee([GraphQLName("query")] MgtappEmployeeQueryInput query, [Service] MongoDbContext ctx)
        {
            // Require some filter criteria to avoid deleting arbitrary documents
            bool hasFilter = !string.IsNullOrWhiteSpace(query._id)
                || query.clientid != null
                || query.employeeuserid != null
                || query.employeemanagerid != null
                || query.employeephoneQuery != null
                || query.employeefirstnameQuery != null
                || query.employeelastnameQuery != null
                || query.employeeemailQuery != null
                || (query.and != null && query.and.Any())
                || (query.or != null && query.or.Any());

            if (!hasFilter)
            {
                throw new GraphQLException("Delete requires a filter (provide _id, clientid, employeeuserid, employeemanagerid, string queries, and/or logical groups).");
            }

            var filter = MgtAppEmployeeQuery.BuildFilter(query, ctx);
            var deleted = await ctx.Employees.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappEmployees")]
        public async Task<DeleteManyMgtAppEmployeesPayload> DeleteManyMgtAppEmployees([GraphQLName("query")] MgtappEmployeeQueryInput query, [Service] MongoDbContext ctx)
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(query._id)
                || query.clientid != null
                || query.employeeuserid != null
                || query.employeemanagerid != null
                || query.employeephoneQuery != null
                || query.employeefirstnameQuery != null
                || query.employeelastnameQuery != null
                || query.employeeemailQuery != null
                || (query.and != null && query.and.Any())
                || (query.or != null && query.or.Any());

            if (!hasFilter)
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, clientid, employeeuserid, employeemanagerid, string queries, and/or logical groups).");
            }

            var filter = MgtAppEmployeeQuery.BuildFilter(query, ctx);
            var result = await ctx.Employees.DeleteManyAsync(filter);
            return new DeleteManyMgtAppEmployeesPayload { deletedCount = (int)result.DeletedCount };
        }

        [GraphQLName("updateManyMgtappEmployees")]
        public async Task<UpdateManyMgtAppEmployeesPayload> UpdateManyMgtAppEmployees([GraphQLName("query")] MgtappEmployeeQueryInput query, MgtAppEmployeeSetInput set, [Service] MongoDbContext ctx)
        {
            var filter = MgtAppEmployeeQuery.BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppEmployee>>();

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.clientid, client._id));
            }

            if (set.employeeuserid != null)
            {
                if (string.IsNullOrWhiteSpace(set.employeeuserid.link))
                {
                    throw new GraphQLException("employeeuserid.link is required when provided.");
                }
                var user = await ctx.Users.Find(Builders<MgtAppUser>.Filter.Eq(x => x._id, set.employeeuserid.link)).FirstOrDefaultAsync();
                if (user == null) throw new GraphQLException("Invalid employeeuserid.link: user not found.");
                updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeeuserid, user._id));
            }

            if (set.employeemanagerid != null)
            {
                if (string.IsNullOrWhiteSpace(set.employeemanagerid.link))
                {
                    throw new GraphQLException("employeemanagerid.link is required when provided.");
                }
                var mgr = await ctx.Users.Find(Builders<MgtAppUser>.Filter.Eq(x => x._id, set.employeemanagerid.link)).FirstOrDefaultAsync();
                if (mgr == null) throw new GraphQLException("Invalid employeemanagerid.link: user not found.");
                updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeemanagerid, mgr._id));
            }

            if (set.employeephone != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeephone, set.employeephone));
            if (set.employeefirstname != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeefirstname, set.employeefirstname));
            if (set.employeesalaryrate != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeesalaryrate, set.employeesalaryrate));
            if (set.employeeworkemail != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeeworkemail, set.employeeworkemail));
            if (set.employeeexpirydate != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeeexpirydate, set.employeeexpirydate));
            if (set.employeelastname != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeelastname, set.employeelastname));
            if (set.employeedob != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeedob, set.employeedob));
            if (set.employeevisastatus != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeevisastatus, set.employeevisastatus));
            if (set.employeeemail != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeeemail, set.employeeemail));
            if (set.employeevendor != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeevendor, set.employeevendor));
            if (set.employeetype != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeetype, set.employeetype));
            if (set.employeebillrate != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeebillrate, set.employeebillrate));
            if (set.employeesubstatus != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeesubstatus, set.employeesubstatus));
            if (set.employeestatus != null) updates.Add(Builders<MgtAppEmployee>.Update.Set(x => x.employeestatus, set.employeestatus));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppEmployee>.Update.Combine(updates);
            var result = await ctx.Employees.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppEmployeesPayload { modifiedCount = (int)result.ModifiedCount };
        }
    }
}