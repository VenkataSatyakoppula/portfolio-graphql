using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppGroupTypes
{
    public class MgtAppGroupSetInput
    {
        public string? groupname { get; set; }
        public LinkIdInput? clientid { get; set; }
    }
}