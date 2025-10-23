using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppImmigrationTypes
{
    public class MgtAppImmigrationSetInput
    {
        public LinkIdInput? clientid { get; set; }
        public LinkIdInput? employeeid { get; set; }

        public string? immigrationstatus { get; set; }
        public string? immigrationsubstatus { get; set; }
    }
}