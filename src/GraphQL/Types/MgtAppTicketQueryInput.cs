using MongoDB.Bson;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppProfileTypes;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;
using portfolio_graphql.GraphQL.Types.MgtAppPositionTypes;
using portfolio_graphql.GraphQL.Types.MgtAppGroupTypes;

namespace portfolio_graphql.GraphQL.Types
{
    public class MgtappTicketQueryInput
    {
        public string? _id { get; set; }
        public MgtappProfileQueryInput? profileid { get; set; }
        public MgtappUserQueryInput? ticketcreatedby { get; set; }
        public MgtappUserQueryInput? ticketassignedto { get; set; }
        public MgtappPositionQueryInput? positionid { get; set; }
        public MgtappGroupQueryInput? groupid { get; set; }

        public string? ticketcreateddate { get; set; }
        public string? timesheetweek { get; set; }
        public string? tickettype { get; set; }
        public string? ticketstatus { get; set; }
        public string? ticketcategory { get; set; }

        public List<MgtappTicketQueryInput>? and { get; set; }
        public List<MgtappTicketQueryInput>? or { get; set; }
    }
}