using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HotChocolate;

namespace portfolio_graphql.Models
{
    public class MgtAppGroup
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;

        public string groupname { get; set; } = string.Empty;

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string clientid { get; set; } = string.Empty;
    }
}