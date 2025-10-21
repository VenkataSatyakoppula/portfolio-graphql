using System.Collections.Generic;

namespace portfolio_graphql.GraphQL.Types
{
    public class StringQueryInput
    {
        public string? eq { get; set; }
        public string? ne { get; set; }
        public List<string>? @in { get; set; }
        public List<string>? nin { get; set; }
        public string? regex { get; set; }
    }
}