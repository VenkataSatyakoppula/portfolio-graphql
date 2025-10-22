namespace portfolio_graphql.GraphQL.Types
{
    // Generic input wrapper for referencing another document by ID
    public class LinkIdInput
    {
        public string link { get; set; } = string.Empty; // expects target _id as string (ObjectId)
    }
}