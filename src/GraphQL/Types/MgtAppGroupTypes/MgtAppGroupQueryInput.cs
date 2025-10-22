using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppGroupTypes
{
    public class MgtAppGroupQueryInput
    {
        public string? _id { get; set; }
        public string? groupname { get; set; }
        public StringQueryInput? groupnameQuery { get; set; }
        // Rename nested client to clientid and remove primitive clientid
        public MgtAppClientQueryInput? clientid { get; set; }
        public List<MgtAppGroupQueryInput>? and { get; set; }
        public List<MgtAppGroupQueryInput>? or { get; set; }
    }
}