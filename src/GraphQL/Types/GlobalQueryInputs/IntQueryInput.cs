using System.Collections.Generic;

namespace portfolio_graphql.GraphQL.Types
{
    public class IntQueryInput
    {
        public int? eq { get; set; }
        public int? gt { get; set; }
        public int? gte { get; set; }
        public int? lt { get; set; }
        public int? lte { get; set; }
        public List<int>? @in { get; set; }
        public List<int>? nin { get; set; }
    }
}