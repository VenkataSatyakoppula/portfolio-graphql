namespace portfolio_graphql.GraphQL.Types.MgtAppUserTypes
{
    public class MgtAppUserUpdateInput
    {
        public string? username { get; set; }
        public string? useremail { get; set; }
        public string? clientid { get; set; } // client _id
        public string? roleid { get; set; }   // role _id
    }
}