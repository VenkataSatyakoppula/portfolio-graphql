using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HotChocolate;

namespace portfolio_graphql.Models
{
    public class MgtAppInsurance
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? employeeid { get; set; }

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? clientid { get; set; }

        public string? insurancestatus { get; set; }
        public string? insurancesubstatus { get; set; }
    }
}