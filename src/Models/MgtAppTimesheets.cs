using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HotChocolate;

namespace portfolio_graphql.Models
{
    public class TimesheetInfo
    {
        public string? timesheethours { get; set; }
        public string? timesheetdate { get; set; }
    }

    public class MgtAppTimesheets
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;

        public string? timesheetmonth { get; set; }

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? employeeid { get; set; }

        [GraphQLIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? clientid { get; set; }

        public List<TimesheetInfo>? timesheetinfo { get; set; }
    }
}