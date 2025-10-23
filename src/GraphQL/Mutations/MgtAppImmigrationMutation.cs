using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.GraphQL.Types.MgtAppImmigrationTypes;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Queries;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppImmigrationsPayload")]
    public class DeleteManyMgtAppImmigrationsPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppImmigration documents deleted.")]
        public int deletedCount { get; set; }
    }

    [GraphQLName("UpdateManyMgtAppImmigrationsPayload")]
    public class UpdateManyMgtAppImmigrationsPayload
    {
        [GraphQLName("modifiedCount")]
        [GraphQLDescription("Number of MgtAppImmigration documents modified.")]
        public int modifiedCount { get; set; }
    }

    [ExtendObjectType("Mutation")]
    public class MgtAppImmigrationMutation
    {
        [GraphQLName("insertOneMgtappImmigration")]
        public async Task<MgtAppImmigration> InsertOneMgtAppImmigration(MgtAppImmigrationInsertInput input, [Service] MongoDbContext ctx)
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

            var doc = new MgtAppImmigration
            {
                _id = ObjectId.GenerateNewId().ToString(),
                clientid = client._id,
                employeeid = employee._id,
                immigrationstatus = input.immigrationstatus ?? string.Empty,
                immigrationsubstatus = input.immigrationsubstatus ?? string.Empty
            };

            await ctx.Immigrations.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappImmigration")]
        public async Task<MgtAppImmigration?> UpdateOneMgtAppImmigration([GraphQLName("query")] MgtAppImmigrationQueryInput query, MgtAppImmigrationSetInput set, [Service] MongoDbContext ctx)
        {
            var filter = MgtAppImmigrationQuery.BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppImmigration>>();

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppImmigration>.Update.Set(x => x.clientid, client._id));
            }

            if (set.employeeid != null)
            {
                if (string.IsNullOrWhiteSpace(set.employeeid.link))
                {
                    throw new GraphQLException("employeeid.link is required when provided.");
                }
                var emp = await ctx.Employees.Find(Builders<MgtAppEmployee>.Filter.Eq(x => x._id, set.employeeid.link)).FirstOrDefaultAsync();
                if (emp == null) throw new GraphQLException("Invalid employeeid.link: employee not found.");
                updates.Add(Builders<MgtAppImmigration>.Update.Set(x => x.employeeid, emp._id));
            }

            if (set.immigrationstatus != null) updates.Add(Builders<MgtAppImmigration>.Update.Set(x => x.immigrationstatus, set.immigrationstatus));
            if (set.immigrationsubstatus != null) updates.Add(Builders<MgtAppImmigration>.Update.Set(x => x.immigrationsubstatus, set.immigrationsubstatus));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppImmigration>.Update.Combine(updates);
            var options = new FindOneAndUpdateOptions<MgtAppImmigration> { ReturnDocument = ReturnDocument.After };
            var result = await ctx.Immigrations.FindOneAndUpdateAsync(filter, combinedUpdate, options);
            return result;
        }

        [GraphQLName("updateManyMgtappImmigrations")]
        public async Task<UpdateManyMgtAppImmigrationsPayload> UpdateManyMgtAppImmigrations([GraphQLName("query")] MgtAppImmigrationQueryInput query, MgtAppImmigrationSetInput set, [Service] MongoDbContext ctx)
        {
            var filter = MgtAppImmigrationQuery.BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppImmigration>>();

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppImmigration>.Update.Set(x => x.clientid, client._id));
            }

            if (set.employeeid != null)
            {
                if (string.IsNullOrWhiteSpace(set.employeeid.link))
                {
                    throw new GraphQLException("employeeid.link is required when provided.");
                }
                var emp = await ctx.Employees.Find(Builders<MgtAppEmployee>.Filter.Eq(x => x._id, set.employeeid.link)).FirstOrDefaultAsync();
                if (emp == null) throw new GraphQLException("Invalid employeeid.link: employee not found.");
                updates.Add(Builders<MgtAppImmigration>.Update.Set(x => x.employeeid, emp._id));
            }

            if (set.immigrationstatus != null) updates.Add(Builders<MgtAppImmigration>.Update.Set(x => x.immigrationstatus, set.immigrationstatus));
            if (set.immigrationsubstatus != null) updates.Add(Builders<MgtAppImmigration>.Update.Set(x => x.immigrationsubstatus, set.immigrationsubstatus));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppImmigration>.Update.Combine(updates);
            var result = await ctx.Immigrations.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppImmigrationsPayload { modifiedCount = (int)result.ModifiedCount };
        }

        [GraphQLName("deleteOneMgtappImmigration")]
        public async Task<MgtAppImmigration?> DeleteOneMgtAppImmigration([GraphQLName("query")] MgtAppImmigrationQueryInput query, [Service] MongoDbContext ctx)
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(query._id)
                || query.clientid != null
                || query.employeeid != null
                || !string.IsNullOrWhiteSpace(query.immigrationstatus)
                || !string.IsNullOrWhiteSpace(query.immigrationsubstatus)
                || query.immigrationstatusQuery != null
                || query.immigrationsubstatusQuery != null
                || (query.and != null && query.and.Any())
                || (query.or != null && query.or.Any());

            if (!hasFilter)
            {
                throw new GraphQLException("Delete requires a filter (provide _id, clientid, employeeid, string queries, and/or logical groups).");
            }

            var filter = MgtAppImmigrationQuery.BuildFilter(query, ctx);
            var deleted = await ctx.Immigrations.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappImmigrations")]
        public async Task<DeleteManyMgtAppImmigrationsPayload> DeleteManyMgtAppImmigrations([GraphQLName("query")] MgtAppImmigrationQueryInput query, [Service] MongoDbContext ctx)
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(query._id)
                || query.clientid != null
                || query.employeeid != null
                || !string.IsNullOrWhiteSpace(query.immigrationstatus)
                || !string.IsNullOrWhiteSpace(query.immigrationsubstatus)
                || query.immigrationstatusQuery != null
                || query.immigrationsubstatusQuery != null
                || (query.and != null && query.and.Any())
                || (query.or != null && query.or.Any());

            if (!hasFilter)
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, clientid, employeeid, string queries, and/or logical groups).");
            }

            var filter = MgtAppImmigrationQuery.BuildFilter(query, ctx);
            var result = await ctx.Immigrations.DeleteManyAsync(filter);
            return new DeleteManyMgtAppImmigrationsPayload { deletedCount = (int)result.DeletedCount };
        }
    }
}