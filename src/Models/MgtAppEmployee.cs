using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HotChocolate;

namespace portfolio_graphql.Models
{
    public class MgtAppEmployee
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
        public string? employeeuserid { get; set; } = null;

        [GraphQLIgnore]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? employeemanagerid { get; set; } = null;

        public string? employeephone { get; set; } = null;
        public string? employeefirstname { get; set; } = null;
        public string? employeesalaryrate { get; set; } = null;
        public string? employeeworkemail { get; set; } = null;
        public string? employeeexpirydate { get; set; } = null;
        public string? employeelastname { get; set; } = null;
        public string? employeedob { get; set; } = null;
        public string? employeevisastatus { get; set; } = null;
        public string? employeeemail { get; set; } = null;
        public string? employeevendor { get; set; } = null;
        public string? employeetype { get; set; } = null;
        public string? employeebillrate { get; set; } = null;
        public string? employeesubstatus { get; set; } = null;
        public string? employeestatus { get; set; } = null;
    }
}