using HotChocolate;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppTicketsPayload")]
    public class DeleteManyMgtAppTicketsPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppTicket documents deleted.")]
        public int deletedCount { get; set; }
    }

    [GraphQLName("UpdateManyMgtAppTicketsPayload")]
    public class UpdateManyMgtAppTicketsPayload
    {
        [GraphQLName("matchedCount")]
        [GraphQLDescription("Number of MgtAppTicket documents matched.")]
        public int matchedCount { get; set; }

        [GraphQLName("modifiedCount")]
        [GraphQLDescription("Number of MgtAppTicket documents modified.")]
        public int modifiedCount { get; set; }
    }
}