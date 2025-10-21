using System;

namespace portfolio_graphql.GraphQL.Types
{
    public class DateTimeQueryInput
    {
        public DateTime? eq { get; set; }
        public DateTime? gt { get; set; }
        public DateTime? gte { get; set; }
        public DateTime? lt { get; set; }
        public DateTime? lte { get; set; }
    }
}