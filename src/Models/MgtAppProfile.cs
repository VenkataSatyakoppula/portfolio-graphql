using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HotChocolate;
using System.Collections.Generic;

namespace portfolio_graphql.Models
{
    public class MgtAppProfile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;

        [GraphQLIgnore]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? clientid { get; set; } = null;

        [GraphQLIgnore]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? positionid { get; set; } = null;

        public string? resume { get; set; } = null;
        public string? profilevisastatus { get; set; } = null;
        public string? profilerate { get; set; } = null;
        public string? profilelastname { get; set; } = null;
        public string? profilefirstname { get; set; } = null;
        public string? profileemail { get; set; } = null;
        public string? profiletype { get; set; } = null;
        public string? profileexpirydate { get; set; } = null;
        public string? profiledob { get; set; } = null;
        public string? profilestatus { get; set; } = null;
        public string? profilephone { get; set; } = null;
        public string? profilevendor { get; set; } = null;
        public string? profilecomments { get; set; } = null;

        [BsonIgnoreIfNull]
        public List<string>? profilemanageravail { get; set; } = null;

        [BsonIgnoreIfNull]
        public List<string>? profileavail { get; set; } = null;
    }
}