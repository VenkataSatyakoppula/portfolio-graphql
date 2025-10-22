using portfolio_graphql.GraphQL.Types;
namespace portfolio_graphql.GraphQL.Types.MgtAppGroupTypes
{
    public class MgtAppGroupUpdateInput
    {
        public string? groupname { get; set; }
        public LinkIdInput? clientid { get; set; } // client _id via link
    }
}