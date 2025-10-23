using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
namespace portfolio_graphql.GraphQL.Types.MgtAppClientTypes
{
    public class MgtappClientQueryInput
    {
        public string? _id { get; set; }
        public string? clientname { get; set; }
        public StringQueryInput? clientnameQuery { get; set; }
        public List<MgtappClientQueryInput>? and { get; set; }
        public List<MgtappClientQueryInput>? or { get; set; }
    }
}