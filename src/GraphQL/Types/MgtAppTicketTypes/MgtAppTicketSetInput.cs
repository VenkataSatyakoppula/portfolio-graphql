using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppTicketTypes
{
    public class MgtappTicketUpdateInput
    {
        public LinkIdInput? profileid { get; set; }
        public LinkIdInput? ticketcreatedby { get; set; }
        public LinkIdInput? ticketassignedto { get; set; }
        public LinkIdInput? positionid { get; set; }
        public LinkIdInput? groupid { get; set; }

        public string? ticketcreateddate { get; set; }
        public string? timesheetweek { get; set; }
        public string? tickettype { get; set; }
        public string? ticketstatus { get; set; }
        public string? ticketcategory { get; set; }
    }
}