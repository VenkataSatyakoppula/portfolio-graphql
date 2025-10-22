using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HotChocolate;

namespace portfolio_graphql.Models
{
    public class MgtAppBankDetails
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
        public string? employeeid { get; set; } = null;

        public string? bankstatus { get; set; } = null;
        public string? bankroutingno { get; set; } = null;
        public string? bankaccountno { get; set; } = null;
    }
}