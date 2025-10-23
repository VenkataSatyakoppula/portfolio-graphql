using MongoDB.Bson;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppProfileTypes;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;

namespace portfolio_graphql.GraphQL.Types
{
    public class MgtAppTicketQueryInput
    {
        public string? _id { get; set; }
        public MgtAppProfileQueryInput? profileid { get; set; }
        public MgtAppUserQueryInput? ticketcreatedby { get; set; }
        public MgtAppUserQueryInput? ticketassignedto { get; set; }

        public string? ticketcreateddate { get; set; }
        public string? timesheetweek { get; set; }
        public string? tickettype { get; set; }
        public string? ticketstatus { get; set; }
        public string? ticketcategory { get; set; }

        public List<MgtAppTicketQueryInput>? and { get; set; }
        public List<MgtAppTicketQueryInput>? or { get; set; }
    }
}