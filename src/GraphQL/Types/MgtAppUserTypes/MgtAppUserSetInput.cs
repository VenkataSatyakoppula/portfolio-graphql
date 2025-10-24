using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppUserTypes
{
    public class MgtappUserUpdateInput
    {
        public string? username { get; set; }
        public string? useremail { get; set; }
        public LinkIdInput? clientid { get; set; } // client _id via link
        public LinkIdInput? roleid { get; set; }   // role _id via link
        public LinkIdInput? groupid { get; set; }  // group _id via link
    }
}