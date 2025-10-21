using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HotChocolate;

namespace portfolio_graphql.Models
{
    public class MgtAppUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string useremail { get; set; } = string.Empty;
        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string clientid { get; set; } = string.Empty;
        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string roleid { get; set; } = string.Empty;
    }
}