using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppUserTypes
{
    public class MgtAppUserInsertInput
    {
        public string username { get; set; } = string.Empty;
        public string useremail { get; set; } = string.Empty;
        public LinkIdInput clientid { get; set; } = new LinkIdInput();
        public LinkIdInput roleid { get; set; } = new LinkIdInput();
    }
}