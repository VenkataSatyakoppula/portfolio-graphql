using MongoDB.Bson;
using MongoDB.Driver;
using portfolio_graphql.GraphQL.Types.MgtAppProfileTypes;
using portfolio_graphql.Models;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppPositionTypes;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using portfolio_graphql.GraphQL.Queries;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppProfilesPayload")]
    public class DeleteManyMgtAppProfilesPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppProfile documents deleted.")]
        public int deletedCount { get; set; }
    }

    [GraphQLName("UpdateManyMgtAppProfilesPayload")]
    public class UpdateManyMgtAppProfilesPayload
    {
        [GraphQLName("modifiedCount")]
        [GraphQLDescription("Number of MgtAppProfile documents modified.")]
        public int modifiedCount { get; set; }
    }

    [ExtendObjectType("Mutation")]
    public class MgtAppProfileMutation
    {
        [GraphQLName("insertOneMgtappProfile")]
        public async Task<MgtAppProfile> InsertOneMgtAppProfile(MgtappProfileInsertInput data, [Service] MongoDbContext ctx)
        {
            if (data.clientid == null || string.IsNullOrWhiteSpace(data.clientid.link))
            {
                throw new GraphQLException("clientid.link is required.");
            }

            var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, data.clientid.link)).FirstOrDefaultAsync();
            if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");

            string? positionId = null;
            if (data.positionid != null)
            {
                if (string.IsNullOrWhiteSpace(data.positionid.link))
                {
                    throw new GraphQLException("positionid.link is required when provided.");
                }
                var position = await ctx.Positions.Find(Builders<MgtAppPosition>.Filter.Eq(x => x._id, data.positionid.link)).FirstOrDefaultAsync();
                if (position == null) throw new GraphQLException("Invalid positionid.link: position not found.");
                positionId = position._id;
            }

            var doc = new MgtAppProfile
            {
                _id = ObjectId.GenerateNewId().ToString(),
                clientid = client._id,
                positionid = positionId,
                resume = data.resume,
                profilevisastatus = data.profilevisastatus,
                profilerate = data.profilerate,
                profilelastname = data.profilelastname,
                profilefirstname = data.profilefirstname,
                profileemail = data.profileemail,
                profiletype = data.profiletype,
                profileexpirydate = data.profileexpirydate,
                profiledob = data.profiledob,
                profilestatus = data.profilestatus,
                profilephone = data.profilephone,
                profilevendor = data.profilevendor,
                profilecomments = data.profilecomments,
                profilemanageravail = data.profilemanageravail,
                profileavail = data.profileavail
            };

            await ctx.Profiles.InsertOneAsync(doc);
            return doc;
        }

        [GraphQLName("updateOneMgtappProfile")]
        public async Task<MgtAppProfile?> UpdateOneMgtAppProfile([GraphQLName("query")] MgtappProfileQueryInput query, MgtappProfileUpdateInput set, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppProfile>>();

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.clientid, client._id));
            }

            if (set.positionid != null)
            {
                if (string.IsNullOrWhiteSpace(set.positionid.link))
                {
                    throw new GraphQLException("positionid.link is required when provided.");
                }
                var position = await ctx.Positions.Find(Builders<MgtAppPosition>.Filter.Eq(x => x._id, set.positionid.link)).FirstOrDefaultAsync();
                if (position == null) throw new GraphQLException("Invalid positionid.link: position not found.");
                updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.positionid, position._id));
            }

            if (set.resume != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.resume, set.resume));
            if (set.profilevisastatus != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilevisastatus, set.profilevisastatus));
            if (set.profilerate != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilerate, set.profilerate));
            if (set.profilelastname != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilelastname, set.profilelastname));
            if (set.profilefirstname != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilefirstname, set.profilefirstname));
            if (set.profileemail != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profileemail, set.profileemail));
            if (set.profiletype != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profiletype, set.profiletype));
            if (set.profileexpirydate != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profileexpirydate, set.profileexpirydate));
            if (set.profiledob != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profiledob, set.profiledob));
            if (set.profilestatus != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilestatus, set.profilestatus));
            if (set.profilephone != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilephone, set.profilephone));
            if (set.profilevendor != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilevendor, set.profilevendor));
            if (set.profilecomments != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilecomments, set.profilecomments));

            if (set.profilemanageravail != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilemanageravail, set.profilemanageravail));
            if (set.profileavail != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profileavail, set.profileavail));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppProfile>.Update.Combine(updates);
            var options = new FindOneAndUpdateOptions<MgtAppProfile> { ReturnDocument = ReturnDocument.After };
            var result = await ctx.Profiles.FindOneAndUpdateAsync(filter, combinedUpdate, options);
            return result;
        }

        [GraphQLName("deleteOneMgtappProfile")]
        public async Task<MgtAppProfile?> DeleteOneMgtAppProfile([GraphQLName("query")] MgtappProfileQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id)
                && query.clientid == null
                && query.positionid == null
                && string.IsNullOrWhiteSpace(query.resume)
                && string.IsNullOrWhiteSpace(query.profilevisastatus)
                && string.IsNullOrWhiteSpace(query.profilerate)
                && string.IsNullOrWhiteSpace(query.profilelastname)
                && string.IsNullOrWhiteSpace(query.profilefirstname)
                && string.IsNullOrWhiteSpace(query.profileemail)
                && string.IsNullOrWhiteSpace(query.profiletype)
                && string.IsNullOrWhiteSpace(query.profileexpirydate)
                && string.IsNullOrWhiteSpace(query.profiledob)
                && string.IsNullOrWhiteSpace(query.profilestatus)
                && string.IsNullOrWhiteSpace(query.profilephone)
                && string.IsNullOrWhiteSpace(query.profilevendor)
                && string.IsNullOrWhiteSpace(query.profilecomments)
                && query.resumeQuery == null
                && query.profilevisastatusQuery == null
                && query.profilerateQuery == null
                && query.profilelastnameQuery == null
                && query.profilefirstnameQuery == null
                && query.profileemailQuery == null
                && query.profiletypeQuery == null
                && query.profileexpirydateQuery == null
                && query.profiledobQuery == null
                && query.profilestatusQuery == null
                && query.profilephoneQuery == null
                && query.profilevendorQuery == null
                && query.profilecommentsQuery == null
                && (query.and == null || !query.and.Any())
                && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("Delete requires a filter (provide _id, clientid, positionid, scalar fields, string queries, and/or logical groups).");
            }

            var filter = BuildFilter(query, ctx);
            var deleted = await ctx.Profiles.FindOneAndDeleteAsync(filter);
            return deleted;
        }

        [GraphQLName("deleteManyMgtappProfiles")]
        public async Task<DeleteManyMgtAppProfilesPayload> DeleteManyMgtAppProfiles([GraphQLName("query")] MgtappProfileQueryInput query, [Service] MongoDbContext ctx)
        {
            if (string.IsNullOrWhiteSpace(query._id)
                && query.clientid == null
                && query.positionid == null
                && string.IsNullOrWhiteSpace(query.resume)
                && string.IsNullOrWhiteSpace(query.profilevisastatus)
                && string.IsNullOrWhiteSpace(query.profilerate)
                && string.IsNullOrWhiteSpace(query.profilelastname)
                && string.IsNullOrWhiteSpace(query.profilefirstname)
                && string.IsNullOrWhiteSpace(query.profileemail)
                && string.IsNullOrWhiteSpace(query.profiletype)
                && string.IsNullOrWhiteSpace(query.profileexpirydate)
                && string.IsNullOrWhiteSpace(query.profiledob)
                && string.IsNullOrWhiteSpace(query.profilestatus)
                && string.IsNullOrWhiteSpace(query.profilephone)
                && string.IsNullOrWhiteSpace(query.profilevendor)
                && string.IsNullOrWhiteSpace(query.profilecomments)
                && query.resumeQuery == null
                && query.profilevisastatusQuery == null
                && query.profilerateQuery == null
                && query.profilelastnameQuery == null
                && query.profilefirstnameQuery == null
                && query.profileemailQuery == null
                && query.profiletypeQuery == null
                && query.profileexpirydateQuery == null
                && query.profiledobQuery == null
                && query.profilestatusQuery == null
                && query.profilephoneQuery == null
                && query.profilevendorQuery == null
                && query.profilecommentsQuery == null
                && (query.and == null || !query.and.Any())
                && (query.or == null || !query.or.Any()))
            {
                throw new GraphQLException("DeleteMany requires a filter (provide _id, clientid, positionid, scalar fields, string queries, and/or logical groups).");
            }

            var filter = BuildFilter(query, ctx);
            var result = await ctx.Profiles.DeleteManyAsync(filter);
            return new DeleteManyMgtAppProfilesPayload { deletedCount = (int)result.DeletedCount };
        }

        [GraphQLName("updateManyMgtappProfiles")]
        public async Task<UpdateManyMgtAppProfilesPayload> UpdateManyMgtAppProfiles([GraphQLName("query")] MgtappProfileQueryInput query, MgtappProfileUpdateInput set, [Service] MongoDbContext ctx)
        {
            var filter = BuildFilter(query, ctx);

            var updates = new List<UpdateDefinition<MgtAppProfile>>();

            if (set.clientid != null)
            {
                if (string.IsNullOrWhiteSpace(set.clientid.link))
                {
                    throw new GraphQLException("clientid.link is required when provided.");
                }
                var client = await ctx.Clients.Find(Builders<MgtAppClient>.Filter.Eq(x => x._id, set.clientid.link)).FirstOrDefaultAsync();
                if (client == null) throw new GraphQLException("Invalid clientid.link: client not found.");
                updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.clientid, client._id));
            }

            if (set.positionid != null)
            {
                if (string.IsNullOrWhiteSpace(set.positionid.link))
                {
                    throw new GraphQLException("positionid.link is required when provided.");
                }
                var position = await ctx.Positions.Find(Builders<MgtAppPosition>.Filter.Eq(x => x._id, set.positionid.link)).FirstOrDefaultAsync();
                if (position == null) throw new GraphQLException("Invalid positionid.link: position not found.");
                updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.positionid, position._id));
            }

            if (set.resume != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.resume, set.resume));
            if (set.profilevisastatus != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilevisastatus, set.profilevisastatus));
            if (set.profilerate != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilerate, set.profilerate));
            if (set.profilelastname != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilelastname, set.profilelastname));
            if (set.profilefirstname != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilefirstname, set.profilefirstname));
            if (set.profileemail != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profileemail, set.profileemail));
            if (set.profiletype != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profiletype, set.profiletype));
            if (set.profileexpirydate != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profileexpirydate, set.profileexpirydate));
            if (set.profiledob != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profiledob, set.profiledob));
            if (set.profilestatus != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilestatus, set.profilestatus));
            if (set.profilephone != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilephone, set.profilephone));
            if (set.profilevendor != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilevendor, set.profilevendor));
            if (set.profilecomments != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilecomments, set.profilecomments));

            if (set.profilemanageravail != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profilemanageravail, set.profilemanageravail));
            if (set.profileavail != null) updates.Add(Builders<MgtAppProfile>.Update.Set(x => x.profileavail, set.profileavail));

            if (!updates.Any())
            {
                throw new GraphQLException("No set fields provided.");
            }

            var combinedUpdate = Builders<MgtAppProfile>.Update.Combine(updates);
            var result = await ctx.Profiles.UpdateManyAsync(filter, combinedUpdate);
            return new UpdateManyMgtAppProfilesPayload { modifiedCount = (int)result.ModifiedCount };
        }

    private static FilterDefinition<MgtAppProfile> BuildFilter(MgtappProfileQueryInput? query, MongoDbContext ctx)
    {
        if (query == null)
        {
            return Builders<MgtAppProfile>.Filter.Empty;
        }

        var filters = new List<FilterDefinition<MgtAppProfile>>();

        // Primitive equality filters
        if (!string.IsNullOrWhiteSpace(query._id))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x._id, query._id));
        }
        if (!string.IsNullOrWhiteSpace(query.resume))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.resume, query.resume));
        }
        if (!string.IsNullOrWhiteSpace(query.profilevisastatus))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilevisastatus, query.profilevisastatus));
        }
        if (!string.IsNullOrWhiteSpace(query.profilerate))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilerate, query.profilerate));
        }
        if (!string.IsNullOrWhiteSpace(query.profilelastname))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilelastname, query.profilelastname));
        }
        if (!string.IsNullOrWhiteSpace(query.profilefirstname))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilefirstname, query.profilefirstname));
        }
        if (!string.IsNullOrWhiteSpace(query.profileemail))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profileemail, query.profileemail));
        }
        if (!string.IsNullOrWhiteSpace(query.profiletype))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profiletype, query.profiletype));
        }
        if (!string.IsNullOrWhiteSpace(query.profileexpirydate))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profileexpirydate, query.profileexpirydate));
        }
        if (!string.IsNullOrWhiteSpace(query.profiledob))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profiledob, query.profiledob));
        }
        if (!string.IsNullOrWhiteSpace(query.profilestatus))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilestatus, query.profilestatus));
        }
        if (!string.IsNullOrWhiteSpace(query.profilephone))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilephone, query.profilephone));
        }
        if (!string.IsNullOrWhiteSpace(query.profilevendor))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilevendor, query.profilevendor));
        }
        if (!string.IsNullOrWhiteSpace(query.profilecomments))
        {
            filters.Add(Builders<MgtAppProfile>.Filter.Eq(x => x.profilecomments, query.profilecomments));
        }

        // StringQueryInput filters
        void ApplyStringQuery(StringQueryInput? q, System.Linq.Expressions.Expression<Func<MgtAppProfile, string?>> fieldExpr)
        {
            if (q == null) return;
            var field = new ExpressionFieldDefinition<MgtAppProfile, string?>(fieldExpr);
            var fq = new List<FilterDefinition<MgtAppProfile>>();
            if (q.eq != null) fq.Add(Builders<MgtAppProfile>.Filter.Eq(field, q.eq));
            if (q.ne != null) fq.Add(Builders<MgtAppProfile>.Filter.Ne(field, q.ne));
            if (q.@in != null && q.@in.Count > 0) fq.Add(Builders<MgtAppProfile>.Filter.In(field, q.@in));
            if (q.nin != null && q.nin.Count > 0) fq.Add(Builders<MgtAppProfile>.Filter.Nin(field, q.nin));
            if (q.regex != null)
            {
                var regex = new Regex(q.regex, RegexOptions.IgnoreCase);
                fq.Add(Builders<MgtAppProfile>.Filter.Regex(field, new BsonRegularExpression(regex)));
            }
            if (fq.Count > 0) filters.Add(Builders<MgtAppProfile>.Filter.And(fq));
        }

        ApplyStringQuery(query.resumeQuery, x => x.resume);
        ApplyStringQuery(query.profilevisastatusQuery, x => x.profilevisastatus);
        ApplyStringQuery(query.profilerateQuery, x => x.profilerate);
        ApplyStringQuery(query.profilelastnameQuery, x => x.profilelastname);
        ApplyStringQuery(query.profilefirstnameQuery, x => x.profilefirstname);
        ApplyStringQuery(query.profileemailQuery, x => x.profileemail);
        ApplyStringQuery(query.profiletypeQuery, x => x.profiletype);
        ApplyStringQuery(query.profileexpirydateQuery, x => x.profileexpirydate);
        ApplyStringQuery(query.profiledobQuery, x => x.profiledob);
        ApplyStringQuery(query.profilestatusQuery, x => x.profilestatus);
        ApplyStringQuery(query.profilephoneQuery, x => x.profilephone);
        ApplyStringQuery(query.profilevendorQuery, x => x.profilevendor);
        ApplyStringQuery(query.profilecommentsQuery, x => x.profilecomments);

        // Nested client filter (renamed to clientid)
        if (query.clientid != null)
        {
            var clientFilter = MgtAppClientQuery.BuildFilter(query.clientid);
            var clientIds = ctx.Clients.Find(clientFilter).Project(c => c._id).ToList();
            filters.Add(Builders<MgtAppProfile>.Filter.In(p => p.clientid, clientIds));
        }

        // Nested position filter (renamed to positionid)
        if (query.positionid != null)
        {
            var posFilter = MgtAppPositionQuery.BuildFilter(query.positionid, ctx);
            var posIds = ctx.Positions.Find(posFilter).Project(p => p._id).ToList();
            filters.Add(Builders<MgtAppProfile>.Filter.In(p => p.positionid, posIds));
        }

        // Logical groups
        if (query.and != null && query.and.Any())
        {
            var andFilters = query.and.Select(q => BuildFilter(q, ctx)).ToArray();
            filters.Add(Builders<MgtAppProfile>.Filter.And(andFilters));
        }

        if (query.or != null && query.or.Any())
        {
            var orFilters = query.or.Select(q => BuildFilter(q, ctx)).ToArray();
            filters.Add(Builders<MgtAppProfile>.Filter.Or(orFilters));
        }

        if (!filters.Any())
        {
            return Builders<MgtAppProfile>.Filter.Empty;
        }

        if (filters.Count == 1)
        {
            return filters[0];
        }

        return Builders<MgtAppProfile>.Filter.And(filters);
    }
}
}