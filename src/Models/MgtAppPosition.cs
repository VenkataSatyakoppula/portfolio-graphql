using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HotChocolate;

namespace portfolio_graphql.Models
{
    public class MgtAppPosition
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string clientid { get; set; } = string.Empty;

        [GraphQLIgnore]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? groupid { get; set; } = null;

        public string jobtitle { get; set; } = string.Empty;
        public string experience { get; set; } = string.Empty;
        public string skillset { get; set; } = string.Empty;
        public string billingrate { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
    }
}