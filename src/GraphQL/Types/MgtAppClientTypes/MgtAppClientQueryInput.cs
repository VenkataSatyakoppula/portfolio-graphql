using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
namespace portfolio_graphql.GraphQL.Types.MgtAppClientTypes
{
    public class MgtAppClientQueryInput
    {
        public string? _id { get; set; }
        public string? clientname { get; set; }
        public StringQueryInput? clientnameQuery { get; set; }
        public List<MgtAppClientQueryInput>? and { get; set; }
        public List<MgtAppClientQueryInput>? or { get; set; }
    }
}