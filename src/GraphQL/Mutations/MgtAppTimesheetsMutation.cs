using System.Text.RegularExpressions;
using HotChocolate;
using HotChocolate.Types;
using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types.MgtAppTimesheetsTypes;
using portfolio_graphql.Models;
using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class MgtAppTimesheetsMutation
    {
        [GraphQLName("insertOneMgtappTimesheet")]
        public async Task<MgtAppTimesheets?> InsertOneMgtAppTimesheets(
            [Service] MongoDbContext dbContext,
            MgtappTimesheetInsertInput data)
        {
            var collection = dbContext.Timesheets;
            var doc = new MgtAppTimesheets
            {
                _id = ObjectId.GenerateNewId().ToString(),
                timesheetmonth = data.timesheetmonth,
                clientid = data.clientid?.link,
                employeeid = data.employeeid?.link,
                timesheetinfo = data.timesheetinfo?.Select(i => new TimesheetInfo
                {
                    timesheetdate = i.timesheetdate,
                    timesheethours = i.timesheethours
                }).ToList()
            };
            await collection.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappTimesheet")]
        public async Task<MgtAppTimesheets?> UpdateOneMgtAppTimesheets(
            [Service] MongoDbContext dbContext,
            MgtappTimesheetsQueryInput query,
            MgtappTimesheetUpdateInput set)
         {
            var collection = dbContext.Timesheets;
            var filter = portfolio_graphql.GraphQL.Queries.MgtAppTimesheetsQuery.BuildFilter(query, dbContext);
            var updates = new List<UpdateDefinition<MgtAppTimesheets>>();

            if (set.timesheetmonth != null) updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.timesheetmonth, set.timesheetmonth));
            if (set.clientid != null) updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.clientid, set.clientid.link));
            if (set.employeeid != null) updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.employeeid, set.employeeid.link));
            if (set.timesheetinfo != null)
            {
                var list = set.timesheetinfo.Select(i => new TimesheetInfo
                {
                    timesheetdate = i.timesheetdate,
                    timesheethours = i.timesheethours
                }).ToList();
                updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.timesheetinfo, list));
            }

            if (updates.Count == 0) return await collection.Find(filter).FirstOrDefaultAsync();

            var update = Builders<MgtAppTimesheets>.Update.Combine(updates);
            return await collection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<MgtAppTimesheets> { ReturnDocument = ReturnDocument.After });
        }

        [GraphQLName("updateManyMgtappTimesheets")]
        public async Task<long> UpdateManyMgtAppTimesheets(
            [Service] MongoDbContext dbContext,
            MgtappTimesheetsQueryInput query,
            MgtappTimesheetUpdateInput set)
         {
            var collection = dbContext.Timesheets;
            var filter = portfolio_graphql.GraphQL.Queries.MgtAppTimesheetsQuery.BuildFilter(query, dbContext);
             var updates = new List<UpdateDefinition<MgtAppTimesheets>>();

            if (set.timesheetmonth != null) updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.timesheetmonth, set.timesheetmonth));
            if (set.clientid != null) updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.clientid, set.clientid.link));
            if (set.employeeid != null) updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.employeeid, set.employeeid.link));
            if (set.timesheetinfo != null)
            {
                var list = set.timesheetinfo.Select(i => new TimesheetInfo
                {
                    timesheetdate = i.timesheetdate,
                    timesheethours = i.timesheethours
                }).ToList();
                updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.timesheetinfo, list));
            }

            if (updates.Count == 0) return 0;

            var update = Builders<MgtAppTimesheets>.Update.Combine(updates);
            var result = await collection.UpdateManyAsync(filter, update);
            return result.ModifiedCount;
         }

        [GraphQLName("deleteManyMgtappTimesheets")]
        public async Task<DeleteManyMgtAppTimesheetsPayload> DeleteManyMgtAppTimesheets(
            [Service] MongoDbContext dbContext,
            MgtappTimesheetsQueryInput query)
        {
            var collection = dbContext.Timesheets;
            var filter = portfolio_graphql.GraphQL.Queries.MgtAppTimesheetsQuery.BuildFilter(query, dbContext);
            var result = await collection.DeleteManyAsync(filter);
            return new DeleteManyMgtAppTimesheetsPayload { deletedCount = (int)result.DeletedCount };
        }

        [GraphQLName("deleteOneMgtappTimesheet")]
        public async Task<MgtAppTimesheets?> DeleteOneMgtAppTimesheet(
            [Service] MongoDbContext dbContext,
            MgtappTimesheetsQueryInput query)
        {
            var collection = dbContext.Timesheets;
            var filter = portfolio_graphql.GraphQL.Queries.MgtAppTimesheetsQuery.BuildFilter(query, dbContext);
            return await collection.FindOneAndDeleteAsync(filter);
        }

        [GraphQLName("upsertOneMgtappTimesheet")]
        public async Task<MgtAppTimesheets?> UpsertOneMgtAppTimesheet(
            [Service] MongoDbContext dbContext,
            MgtappTimesheetsQueryInput query,
            MgtappTimesheetInsertInput data)
        {
            var collection = dbContext.Timesheets;
            var filter = portfolio_graphql.GraphQL.Queries.MgtAppTimesheetsQuery.BuildFilter(query, dbContext);
            var existing = await collection.Find(filter).FirstOrDefaultAsync();

            if (existing != null)
            {
                var updates = new List<UpdateDefinition<MgtAppTimesheets>>();
                if (data.timesheetmonth != null) updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.timesheetmonth, data.timesheetmonth));
                if (data.clientid != null) updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.clientid, data.clientid.link));
                if (data.employeeid != null) updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.employeeid, data.employeeid.link));
                if (data.timesheetinfo != null)
                {
                    var list = data.timesheetinfo.Select(i => new TimesheetInfo
                    {
                        timesheetdate = i.timesheetdate,
                        timesheethours = i.timesheethours
                    }).ToList();
                    updates.Add(Builders<MgtAppTimesheets>.Update.Set(x => x.timesheetinfo, list));
                }
                if (updates.Count == 0) return existing;
                var update = Builders<MgtAppTimesheets>.Update.Combine(updates);
                return await collection.FindOneAndUpdateAsync(
                    Builders<MgtAppTimesheets>.Filter.Eq(x => x._id, existing._id),
                    update,
                    new FindOneAndUpdateOptions<MgtAppTimesheets> { ReturnDocument = ReturnDocument.After });
            }
            else
            {
                var doc = new MgtAppTimesheets
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    timesheetmonth = data.timesheetmonth,
                    clientid = data.clientid?.link,
                    employeeid = data.employeeid?.link,
                    timesheetinfo = data.timesheetinfo?.Select(i => new TimesheetInfo
                    {
                        timesheetdate = i.timesheetdate,
                        timesheethours = i.timesheethours
                    }).ToList()
                };
                await collection.InsertOneAsync(doc);
                return doc;
            }
        }
    }
}