using System.Collections.Generic;

namespace portfolio_graphql.GraphQL.Types
{
    public class ObjectIdQueryInput
    {
        // Note: Using string to represent ObjectId to align with current model (_id as string)
        public string? eq { get; set; }
        public string? ne { get; set; }
        public List<string>? @in { get; set; }
        public List<string>? nin { get; set; }
    }
}