using HotChocolate;
using HotChocolate.Types;

namespace portfolio_graphql.GraphQL.Mutations
{
    [GraphQLName("DeleteManyMgtAppTimesheetsPayload")]
    public class DeleteManyMgtAppTimesheetsPayload
    {
        [GraphQLName("deletedCount")]
        [GraphQLDescription("Number of MgtAppTimesheets documents deleted.")]
        public int deletedCount { get; set; }
    }
}