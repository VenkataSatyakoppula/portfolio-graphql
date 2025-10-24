using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.GraphQL.Types.MgtAppBankDetailsTypes;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Queries;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppBankDetailsPayload")]
    public class DeleteManyMgtAppBankDetailsPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppBankDetails documents deleted.")]
        public int deletedCount { get; set; }
    }

    [GraphQLName("UpdateManyMgtAppBankDetailsPayload")]
    public class UpdateManyMgtAppBankDetailsPayload
    {
        [GraphQLName("modifiedCount")]
        [GraphQLDescription("Number of MgtAppBankDetails documents modified.")]
        public int modifiedCount { get; set; }
    }

    [ExtendObjectType("Mutation")]
    public class MgtAppBankDetailsMutation
    {
        [GraphQLName("insertOneMgtappBankdetail")]
        public async Task<MgtAppBankDetails> InsertOneMgtAppBankDetail(MgtappBankdetailInsertInput data, [Service] MongoDbContext ctx)
        {
            if (data.clientid == null || string.IsNullOrWhiteSpace(data.clientid.link))
            {
                throw new GraphQLException("clientid.link is required.");
            }

            var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, data.clientid.link)).FirstOrDefaultAsync();
            if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");

            if (data.employeeid == null || string.IsNullOrWhiteSpace(data.employeeid.link))
            {
                throw new GraphQLException("employeeid.link is required.");
            }
            var employee = await ctx.Employees.Find(Builders<MgtAppEmployee>.Filter.Eq(x => x._id, data.employeeid.link)).FirstOrDefaultAsync();
            if (employee == null) throw new GraphQLException("Invalid employeeid.link: employee not found.");

            var doc = new MgtAppBankDetails
            {
                _id = ObjectId.GenerateNewId().ToString(),
                clientid = client._id,
                employeeid = employee._id,
                bankstatus = data.bankstatus,
                bankroutingno = data.bankroutingno,
                bankaccountno = data.bankaccountno
            };

            await ctx.BankDetails.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappBankdetail")]
        public async Task<MgtAppBankDetails?> UpdateOneMgtAppBankDetail([GraphQLName("query")] MgtappBankDetailsQueryInput query, MgtappBankdetailUpdateInput set, [Service] MongoDbContext ctx)
        {
            var filter = MgtAppBankDetailsQuery.BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppBankDetails>>();

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppBankDetails>.Update.Set(x => x.clientid, client._id));
            }

            if (set.employeeid != null)
            {
                if (string.IsNullOrWhiteSpace(set.employeeid.link))
                {
                    throw new GraphQLException("employeeid.link is required when provided.");
                }
                var emp = await ctx.Employees.Find(Builders<MgtAppEmployee>.Filter.Eq(x => x._id, set.employeeid.link)).FirstOrDefaultAsync();
                if (emp == null) throw new GraphQLException("Invalid employeeid.link: employee not found.");
                updates.Add(Builders<MgtAppBankDetails>.Update.Set(x => x.employeeid, emp._id));
            }

            if (set.bankstatus != null) updates.Add(Builders<MgtAppBankDetails>.Update.Set(x => x.bankstatus, set.bankstatus));
            if (set.bankroutingno != null) updates.Add(Builders<MgtAppBankDetails>.Update.Set(x => x.bankroutingno, set.bankroutingno));
            if (set.bankaccountno != null) updates.Add(Builders<MgtAppBankDetails>.Update.Set(x => x.bankaccountno, set.bankaccountno));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppBankDetails>.Update.Combine(updates);
            var options = new FindOneAndUpdateOptions<MgtAppBankDetails> { ReturnDocument = ReturnDocument.After };
            var result = await ctx.BankDetails.FindOneAndUpdateAsync(filter, combinedUpdate, options);
            return result;
        }

        [GraphQLName("updateManyMgtappBankDetails")]
        public async Task<UpdateManyMgtAppBankDetailsPayload> UpdateManyMgtAppBankDetails([GraphQLName("query")] MgtappBankDetailsQueryInput query, MgtappBankdetailUpdateInput set, [Service] MongoDbContext ctx)
        {
            var filter = MgtAppBankDetailsQuery.BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppBankDetails>>();

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppBankDetails>.Update.Set(x => x.clientid, client._id));
            }

            if (set.employeeid != null)
            {
                if (string.IsNullOrWhiteSpace(set.employeeid.link))
                {
                    throw new GraphQLException("employeeid.link is required when provided.");
                }
                var emp = await ctx.Employees.Find(Builders<MgtAppEmployee>.Filter.Eq(x => x._id, set.employeeid.link)).FirstOrDefaultAsync();
                if (emp == null) throw new GraphQLException("Invalid employeeid.link: employee not found.");
                updates.Add(Builders<MgtAppBankDetails>.Update.Set(x => x.employeeid, emp._id));
            }

            if (set.bankstatus != null) updates.Add(Builders<MgtAppBankDetails>.Update.Set(x => x.bankstatus, set.bankstatus));
            if (set.bankroutingno != null) updates.Add(Builders<MgtAppBankDetails>.Update.Set(x => x.bankroutingno, set.bankroutingno));
            if (set.bankaccountno != null) updates.Add(Builders<MgtAppBankDetails>.Update.Set(x => x.bankaccountno, set.bankaccountno));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppBankDetails>.Update.Combine(updates);
            var result = await ctx.BankDetails.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppBankDetailsPayload { modifiedCount = (int)result.ModifiedCount };
        }
        [GraphQLName("deleteOneMgtappBankdetail")]
        public async Task<MgtAppBankDetails?> DeleteOneMgtAppBankDetail([GraphQLName("query")] MgtappBankDetailsQueryInput query, [Service] MongoDbContext ctx)
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(query._id)
                || query.clientid != null
                || query.employeeid != null
                || !string.IsNullOrWhiteSpace(query.bankstatus)
                || !string.IsNullOrWhiteSpace(query.bankroutingno)
                || !string.IsNullOrWhiteSpace(query.bankaccountno)
                || query.bankstatusQuery != null
                || query.bankroutingnoQuery != null
                || query.bankaccountnoQuery != null
                || (query.and != null && query.and.Any())
                || (query.or != null && query.or.Any());

            if (!hasFilter)
            {
                throw new GraphQLException("Delete requires a filter (provide _id, clientid, employeeid, string queries, and/or logical groups).");
            }

            var filter = MgtAppBankDetailsQuery.BuildFilter(query, ctx);
            var deleted = await ctx.BankDetails.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappBankdetails")]
        public async Task<DeleteManyMgtAppBankDetailsPayload> DeleteManyMgtAppBankDetails([GraphQLName("query")] MgtappBankDetailsQueryInput query, [Service] MongoDbContext ctx)
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(query._id)
                || query.clientid != null
                || query.employeeid != null
                || !string.IsNullOrWhiteSpace(query.bankstatus)
                || !string.IsNullOrWhiteSpace(query.bankroutingno)
                || !string.IsNullOrWhiteSpace(query.bankaccountno)
                || query.bankstatusQuery != null
                || query.bankroutingnoQuery != null
                || query.bankaccountnoQuery != null
                || (query.and != null && query.and.Any())
                || (query.or != null && query.or.Any());

            if (!hasFilter)
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, clientid, employeeid, string queries, and/or logical groups).");
            }

            var filter = MgtAppBankDetailsQuery.BuildFilter(query, ctx);
            var result = await ctx.BankDetails.DeleteManyAsync(filter);
            return new DeleteManyMgtAppBankDetailsPayload { deletedCount = (int)result.DeletedCount };
        }
    }
}