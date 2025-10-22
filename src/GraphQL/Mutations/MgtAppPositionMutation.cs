using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.GraphQL.Types.MgtAppPositionTypes;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using portfolio_graphql.GraphQL.Queries;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppPositionsPayload")]
    public class DeleteManyMgtAppPositionsPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppPosition documents deleted.")]
        public int deletedCount { get; set; }
    }

    [GraphQLName("UpdateManyMgtAppPositionsPayload")]
    public class UpdateManyMgtAppPositionsPayload
    {
        [GraphQLName("modifiedCount")]
        [GraphQLDescription("Number of MgtAppPosition documents modified.")]
        public int modifiedCount { get; set; }
    }

    [ExtendObjectType("Mutation")]
    public class MgtAppPositionMutation
    {
        [GraphQLName("insertOneMgtappPosition")]
        public async Task<MgtAppPosition> InsertOneMgtAppPosition(MgtAppPositionInsertInput input, [Service] MongoDbContext ctx)
        {
            if (input.clientid == null || string.IsNullOrWhiteSpace(input.clientid.link))
            {
                throw new GraphQLException("clientid.link is required.");
            }
            // groupid is optional — validate only if provided
            if (input.groupid != null && string.IsNullOrWhiteSpace(input.groupid.link))
            {
                throw new GraphQLException("groupid.link is required when provided.");
            }

            var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, input.clientid.link)).FirstOrDefaultAsync();
            if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");

            var doc = new MgtAppPosition
            {
                _id = ObjectId.GenerateNewId().ToString(),
                clientid = client._id,
                groupid = null,
                jobtitle = input.jobtitle,
                experience = input.experience,
                skillset = input.skillset,
                billingrate = input.billingrate,
                status = input.status
            };

            if (input.groupid != null)
            {
                var group = await ctx.Groups.Find(Builders<MgtAppGroup>.Filter.Eq(x => x._id, input.groupid.link)).FirstOrDefaultAsync();
                if (group == null) throw new GraphQLException("Invalid groupid.link: group not found.");
                doc.groupid = group._id;
            }

            await ctx.Positions.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappPosition")]
        public async Task<MgtAppPosition?> UpdateOneMgtAppPosition([GraphQLName("query")] MgtAppPositionQueryInput query, MgtAppPositionSetInput set, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppPosition>>();
            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.clientid, client._id));
            }
            if (set.groupid != null)
            {
                if (string.IsNullOrWhiteSpace(set.groupid.link))
                {
                    throw new GraphQLException("groupid.link is required when provided.");
                }
                var group = await ctx.Groups.Find(Builders<MgtAppGroup>.Filter.Eq(x => x._id, set.groupid.link)).FirstOrDefaultAsync();
                if (group == null) throw new GraphQLException("Invalid groupid.link: group not found.");
                updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.groupid, group._id));
            }
            if (set.jobtitle != null) updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.jobtitle, set.jobtitle));
            if (set.experience != null) updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.experience, set.experience));
            if (set.skillset != null) updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.skillset, set.skillset));
            if (set.billingrate != null) updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.billingrate, set.billingrate));
            if (set.status != null) updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.status, set.status));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppPosition>.Update.Combine(updates);
            var options = new FindOneAndUpdateOptions<MgtAppPosition> { ReturnDocument = ReturnDocument.After };
            var result = await ctx.Positions.FindOneAndUpdateAsync(filter, combinedUpdate, options);
            return result;
        }

        [GraphQLName("deleteOneMgtappPosition")]
        public async Task<MgtAppPosition?> DeleteOneMgtAppPosition([GraphQLName("query")] MgtAppPositionQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id) && (query.groupid == null) && string.IsNullOrWhiteSpace(query.jobtitle) && string.IsNullOrWhiteSpace(query.experience) && string.IsNullOrWhiteSpace(query.skillset) && string.IsNullOrWhiteSpace(query.billingrate) && string.IsNullOrWhiteSpace(query.status) && query.jobtitleQuery == null && query.experienceQuery == null && query.skillsetQuery == null && query.statusQuery == null && (query.clientid == null) && (query.and == null || !query.and.Any()) && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("Delete requires a filter (provide _id, groupid, jobtitle, experience, skillset, billingrate, status, string queries, clientid, and/or logical groups).");
            }
            var filter = BuildFilter(query, ctx);
            var deleted = await ctx.Positions.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappPositions")]
        public async Task<DeleteManyMgtAppPositionsPayload> DeleteManyMgtAppPositions([GraphQLName("query")] MgtAppPositionQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id) && (query.groupid == null) && string.IsNullOrWhiteSpace(query.jobtitle) && string.IsNullOrWhiteSpace(query.experience) && string.IsNullOrWhiteSpace(query.skillset) && string.IsNullOrWhiteSpace(query.billingrate) && string.IsNullOrWhiteSpace(query.status) && query.jobtitleQuery == null && query.experienceQuery == null && query.skillsetQuery == null && query.statusQuery == null && (query.clientid == null) && (query.and == null || !query.and.Any()) && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, groupid, jobtitle, experience, skillset, billingrate, status, string queries, clientid, and/or logical groups).");
            }
            var filter = BuildFilter(query, ctx);
            var result = await ctx.Positions.DeleteManyAsync(filter);
            return new DeleteManyMgtAppPositionsPayload { deletedCount = (int)result.DeletedCount };
        }

        [GraphQLName("updateManyMgtappPositions")]
        public async Task<UpdateManyMgtAppPositionsPayload> UpdateManyMgtAppPositions([GraphQLName("query")] MgtAppPositionQueryInput query, MgtAppPositionSetInput set, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppPosition>>();
            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.clientid, client._id));
            }
            if (set.groupid != null)
            {
                if (string.IsNullOrWhiteSpace(set.groupid.link))
                {
                    throw new GraphQLException("groupid.link is required when provided.");
                }
                var group = await ctx.Groups.Find(Builders<MgtAppGroup>.Filter.Eq(x => x._id, set.groupid.link)).FirstOrDefaultAsync();
                if (group == null) throw new GraphQLException("Invalid groupid.link: group not found.");
                updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.groupid, group._id));
            }
            if (set.jobtitle != null) updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.jobtitle, set.jobtitle));
            if (set.experience != null) updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.experience, set.experience));
            if (set.skillset != null) updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.skillset, set.skillset));
            if (set.billingrate != null) updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.billingrate, set.billingrate));
            if (set.status != null) updates.Add(Builders<MgtAppPosition>.Update.Set(x => x.status, set.status));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppPosition>.Update.Combine(updates);
            var result = await ctx.Positions.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppPositionsPayload { modifiedCount = (int)result.ModifiedCount };
        }

        private static FilterDefinition<MgtAppPosition> BuildFilter(MgtAppPositionQueryInput? query, MongoDbContext ctx)
        {
            if (query == null)
            {
                return Builders<MgtAppPosition>.Filter.Empty;
            }

            var filters = new List<FilterDefinition<MgtAppPosition>>();

            // Primitive field filters
            if (!string.IsNullOrWhiteSpace(query._id))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x._id, query._id));
            }
            if (!string.IsNullOrWhiteSpace(query.groupid))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.groupid, query.groupid));
            }
            if (!string.IsNullOrWhiteSpace(query.jobtitle))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.jobtitle, query.jobtitle));
            }
            if (!string.IsNullOrWhiteSpace(query.experience))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.experience, query.experience));
            }
            if (!string.IsNullOrWhiteSpace(query.skillset))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.skillset, query.skillset));
            }
            if (!string.IsNullOrWhiteSpace(query.billingrate))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.billingrate, query.billingrate));
            }
            if (!string.IsNullOrWhiteSpace(query.status))
            {
                filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.status, query.status));
            }
            // Remove primitive clientid filter per new input shape
            // if (!string.IsNullOrWhiteSpace(query.clientid))
            // {
            //     filters.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.clientid, query.clientid));
            // }

            // Apply StringQueryInputs for strings
            // Removed _idQuery block to align with MgtAppPositionQueryInput which does not define _idQuery
            if (query.jobtitleQuery != null)
            {
                var q = query.jobtitleQuery;
                var fq = new List<FilterDefinition<MgtAppPosition>>();
                if (q.eq != null) fq.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.jobtitle, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppPosition>.Filter.Ne(x => x.jobtitle, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.In(x => x.jobtitle, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.Nin(x => x.jobtitle, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppPosition>.Filter.Regex(x => x.jobtitle, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppPosition>.Filter.And(fq));
            }
            if (query.experienceQuery != null)
            {
                var q = query.experienceQuery;
                var fq = new List<FilterDefinition<MgtAppPosition>>();
                if (q.eq != null) fq.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.experience, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppPosition>.Filter.Ne(x => x.experience, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.In(x => x.experience, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.Nin(x => x.experience, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppPosition>.Filter.Regex(x => x.experience, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppPosition>.Filter.And(fq));
            }
            if (query.skillsetQuery != null)
            {
                var q = query.skillsetQuery;
                var fq = new List<FilterDefinition<MgtAppPosition>>();
                if (q.eq != null) fq.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.skillset, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppPosition>.Filter.Ne(x => x.skillset, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.In(x => x.skillset, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.Nin(x => x.skillset, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppPosition>.Filter.Regex(x => x.skillset, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppPosition>.Filter.And(fq));
            }
            if (query.billingrateQuery != null)
            {
                var q = query.billingrateQuery;
                var fq = new List<FilterDefinition<MgtAppPosition>>();
                if (q.eq != null) fq.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.billingrate, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppPosition>.Filter.Ne(x => x.billingrate, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.In(x => x.billingrate, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.Nin(x => x.billingrate, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppPosition>.Filter.Regex(x => x.billingrate, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppPosition>.Filter.And(fq));
            }
            if (query.statusQuery != null)
            {
                var q = query.statusQuery;
                var fq = new List<FilterDefinition<MgtAppPosition>>();
                if (q.eq != null) fq.Add(Builders<MgtAppPosition>.Filter.Eq(x => x.status, q.eq));
                if (q.ne != null) fq.Add(Builders<MgtAppPosition>.Filter.Ne(x => x.status, q.ne));
                if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.In(x => x.status, q.@in));
                if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppPosition>.Filter.Nin(x => x.status, q.nin));
                if (q.regex != null)
                {
                    var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                    fq.Add(Builders<MgtAppPosition>.Filter.Regex(x => x.status, new BsonRegularExpression(regex)));
                }
                if (fq.Count > 0) filters.Add(Builders<MgtAppPosition>.Filter.And(fq));
            }

            // Nested client filters using MgtAppClientQueryInput (renamed to clientid)
            if (query.clientid != null)
            {
                var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
                var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
                filters.Add(Builders<MgtAppPosition>.Filter.In(p => p.clientid, clientIds));
            }

            // Logical groups
            if (query.and != null && query.and.Any())
            {
                var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppPosition>.Filter.And(andFilters));
            }
            if (query.or != null && query.or.Any())
            {
                var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
                filters.Add(Builders<MgtAppPosition>.Filter.Or(orFilters));
            }

            // Return combined filter
            if (!filters.Any())
            {
                return Builders<MgtAppPosition>.Filter.Empty;
            }
            if (filters.Count == 1)
            {
                return filters[0];
            }
            return Builders<MgtAppPosition>.Filter.And(filters);
        }
    }
}