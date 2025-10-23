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
        public async Task<MgtAppTimesheets?> InsertOneMgtAppTimesheets(
            [Service] MongoDbContext dbContext,
            MgtAppTimesheetsInsertInput input)
        {
            var collection = dbContext.Timesheets;
            var doc = new MgtAppTimesheets
            {
                _id = ObjectId.GenerateNewId().ToString(),
                timesheetmonth = input.timesheetmonth,
                clientid = input.clientid?.link,
                employeeid = input.employeeid?.link,
                timesheetinfo = input.timesheetinfo?.Select(i => new TimesheetInfo
                {
                    timesheetdate = i.timesheetdate,
                    timesheethours = i.timesheethours
                }).ToList()
            };
            await collection.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappTimesheets")]
        public async Task<MgtAppTimesheets?> UpdateOneMgtAppTimesheets(
            [Service] MongoDbContext dbContext,
            MgtappTimesheetsQueryInput query,
            MgtAppTimesheetsSetInput set)
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
            MgtAppTimesheetsSetInput set)
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

        public async Task<MgtAppTimesheets?> DeleteOneMgtAppTimesheets(
            [Service] MongoDbContext dbContext,
            MgtappTimesheetsQueryInput query)
        {
            var collection = dbContext.Timesheets;
            var filter = portfolio_graphql.GraphQL.Queries.MgtAppTimesheetsQuery.BuildFilter(query, dbContext);
            return await collection.FindOneAndDeleteAsync(filter);
        }

        public async Task<long> DeleteManyMgtAppTimesheets(
            [Service] MongoDbContext dbContext,
            MgtappTimesheetsQueryInput query)
        {
            var collection = dbContext.Timesheets;
            var filter = portfolio_graphql.GraphQL.Queries.MgtAppTimesheetsQuery.BuildFilter(query, dbContext);
            var result = await collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }
    }
}