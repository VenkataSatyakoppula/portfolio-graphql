using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppImmigrationTypes
{
    public class MgtappImmigrationInsertInput
    {
        public LinkIdInput clientid { get; set; } = new LinkIdInput();
        public LinkIdInput employeeid { get; set; } = new LinkIdInput();

        public string? immigrationstatus { get; set; }
        public string? immigrationsubstatus { get; set; }
    }
}