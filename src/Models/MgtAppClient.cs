using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace portfolio_graphql.Models
{
    public class MgtAppClient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;
        public string clientname { get; set; } = string.Empty;
    }
}
