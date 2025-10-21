namespace portfolio_graphql.GraphQL.Types.MgtAppUserTypes
{
    public class MgtAppUserInsertInput
    {
        public string username { get; set; } = string.Empty;
        public string useremail { get; set; } = string.Empty;
        public string clientid { get; set; } = string.Empty; // expecting client _id
        public string roleid { get; set; } = string.Empty;   // expecting role _id
    }
}