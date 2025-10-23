using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HotChocolate;

namespace portfolio_graphql.Models
{
    public class MgtAppTicket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? profileid { get; set; }

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ticketcreatedby { get; set; }

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ticketassignedto { get; set; }

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? positionid { get; set; }

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? groupid { get; set; }

        public string? ticketcreateddate { get; set; }
        public string? timesheetweek { get; set; }
        public string? tickettype { get; set; }
        public string? ticketstatus { get; set; }
        public string? ticketcategory { get; set; }
    }
}