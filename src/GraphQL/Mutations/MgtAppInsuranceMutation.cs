using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.GraphQL.Types.MgtAppInsuranceTypes;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Queries;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppInsurancesPayload")]
    public class DeleteManyMgtAppInsurancesPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppInsurance documents deleted.")]
        public int deletedCount { get; set; }
    }

    [GraphQLName("UpdateManyMgtAppInsurancesPayload")]
    public class UpdateManyMgtAppInsurancesPayload
    {
        [GraphQLName("modifiedCount")]
        [GraphQLDescription("Number of MgtAppInsurance documents modified.")]
        public int modifiedCount { get; set; }
    }

    [ExtendObjectType("Mutation")]
    public class MgtAppInsuranceMutation
    {
        [GraphQLName("insertOneMgtappInsurance")]
        public async Task<MgtAppInsurance> InsertOneMgtAppInsurance(MgtAppInsuranceInsertInput input, [Service] MongoDbContext ctx)
        {
            if (input.clientid == null || string.IsNullOrWhiteSpace(input.clientid.link))
            {
                throw new GraphQLException("clientid.link is required.");
            }
            var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, input.clientid.link)).FirstOrDefaultAsync();
            if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");

            if (input.employeeid == null || string.IsNullOrWhiteSpace(input.employeeid.link))
            {
                throw new GraphQLException("employeeid.link is required.");
            }
            var employee = await ctx.Employees.Find(Builders<MgtAppEmployee>.Filter.Eq(x => x._id, input.employeeid.link)).FirstOrDefaultAsync();
            if (employee == null) throw new GraphQLException("Invalid employeeid.link: employee not found.");

            var doc = new MgtAppInsurance
            {
                _id = ObjectId.GenerateNewId().ToString(),
                clientid = client._id,
                employeeid = employee._id,
                insurancestatus = input.insurancestatus ?? string.Empty,
                insurancesubstatus = input.insurancesubstatus ?? string.Empty
            };

            await ctx.Insurances.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappInsurance")]
        public async Task<MgtAppInsurance?> UpdateOneMgtAppInsurance([GraphQLName("query")] MgtAppInsuranceQueryInput query, MgtAppInsuranceSetInput set, [Service] MongoDbContext ctx)
        {
            var filter = MgtAppInsuranceQuery.BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppInsurance>>();

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppInsurance>.Update.Set(x => x.clientid, client._id));
            }

            if (set.employeeid != null)
            {
                if (string.IsNullOrWhiteSpace(set.employeeid.link))
                {
                    throw new GraphQLException("employeeid.link is required when provided.");
                }
                var emp = await ctx.Employees.Find(Builders<MgtAppEmployee>.Filter.Eq(x => x._id, set.employeeid.link)).FirstOrDefaultAsync();
                if (emp == null) throw new GraphQLException("Invalid employeeid.link: employee not found.");
                updates.Add(Builders<MgtAppInsurance>.Update.Set(x => x.employeeid, emp._id));
            }

            if (set.insurancestatus != null) updates.Add(Builders<MgtAppInsurance>.Update.Set(x => x.insurancestatus, set.insurancestatus));
            if (set.insurancesubstatus != null) updates.Add(Builders<MgtAppInsurance>.Update.Set(x => x.insurancesubstatus, set.insurancesubstatus));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppInsurance>.Update.Combine(updates);
            var options = new FindOneAndUpdateOptions<MgtAppInsurance> { ReturnDocument = ReturnDocument.After };
            var result = await ctx.Insurances.FindOneAndUpdateAsync(filter, combinedUpdate, options);
            return result;
        }

        [GraphQLName("updateManyMgtappInsurances")]
        public async Task<UpdateManyMgtAppInsurancesPayload> UpdateManyMgtAppInsurances([GraphQLName("query")] MgtAppInsuranceQueryInput query, MgtAppInsuranceSetInput set, [Service] MongoDbContext ctx)
        {
            var filter = MgtAppInsuranceQuery.BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppInsurance>>();

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppInsurance>.Update.Set(x => x.clientid, client._id));
            }

            if (set.employeeid != null)
            {
                if (string.IsNullOrWhiteSpace(set.employeeid.link))
                {
                    throw new GraphQLException("employeeid.link is required when provided.");
                }
                var emp = await ctx.Employees.Find(Builders<MgtAppEmployee>.Filter.Eq(x => x._id, set.employeeid.link)).FirstOrDefaultAsync();
                if (emp == null) throw new GraphQLException("Invalid employeeid.link: employee not found.");
                updates.Add(Builders<MgtAppInsurance>.Update.Set(x => x.employeeid, emp._id));
            }

            if (set.insurancestatus != null) updates.Add(Builders<MgtAppInsurance>.Update.Set(x => x.insurancestatus, set.insurancestatus));
            if (set.insurancesubstatus != null) updates.Add(Builders<MgtAppInsurance>.Update.Set(x => x.insurancesubstatus, set.insurancesubstatus));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppInsurance>.Update.Combine(updates);
            var result = await ctx.Insurances.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppInsurancesPayload { modifiedCount = (int)result.ModifiedCount };
        }

        [GraphQLName("deleteOneMgtappInsurance")]
        public async Task<MgtAppInsurance?> DeleteOneMgtAppInsurance([GraphQLName("query")] MgtAppInsuranceQueryInput query, [Service] MongoDbContext ctx)
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(query._id)
                || query.clientid != null
                || query.employeeid != null
                || !string.IsNullOrWhiteSpace(query.insurancestatus)
                || !string.IsNullOrWhiteSpace(query.insurancesubstatus)
                || query.insurancestatusQuery != null
                || query.insurancesubstatusQuery != null
                || (query.and != null && query.and.Any())
                || (query.or != null && query.or.Any());

            if (!hasFilter)
            {
                throw new GraphQLException("Delete requires a filter (provide _id, clientid, employeeid, string queries, and/or logical groups).");
            }

            var filter = MgtAppInsuranceQuery.BuildFilter(query, ctx);
            var deleted = await ctx.Insurances.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappInsurances")]
        public async Task<DeleteManyMgtAppInsurancesPayload> DeleteManyMgtAppInsurances([GraphQLName("query")] MgtAppInsuranceQueryInput query, [Service] MongoDbContext ctx)
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(query._id)
                || query.clientid != null
                || query.employeeid != null
                || !string.IsNullOrWhiteSpace(query.insurancestatus)
                || !string.IsNullOrWhiteSpace(query.insurancesubstatus)
                || query.insurancestatusQuery != null
                || query.insurancesubstatusQuery != null
                || (query.and != null && query.and.Any())
                || (query.or != null && query.or.Any());

            if (!hasFilter)
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, clientid, employeeid, string queries, and/or logical groups).");
            }

            var filter = MgtAppInsuranceQuery.BuildFilter(query, ctx);
            var result = await ctx.Insurances.DeleteManyAsync(filter);
            return new DeleteManyMgtAppInsurancesPayload { deletedCount = (int)result.DeletedCount };
        }
    }
}