using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppUserTypes
{
    public class MgtappUserInsertInput
    {
        public string username { get; set; } = string.Empty;
        public string useremail { get; set; } = string.Empty;
        public LinkIdInput clientid { get; set; } = new LinkIdInput();
        public LinkIdInput roleid { get; set; } = new LinkIdInput();
        public LinkIdInput? groupid { get; set; } = null; // optional group link
    }
}