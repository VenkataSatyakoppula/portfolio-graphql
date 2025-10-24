using portfolio_graphql.GraphQL.Types;
namespace portfolio_graphql.GraphQL.Types.MgtAppGroupTypes
{
    public class MgtappGroupInsertInput
    {
        public string groupname { get; set; } = string.Empty;
        public LinkIdInput clientid { get; set; } = new LinkIdInput();
    }
}