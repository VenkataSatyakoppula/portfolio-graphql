using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppUserTypes
{
    public class MgtAppUserQueryInput
    {
        public string? _id { get; set; } // string to match other collections; will convert to ObjectId in filters
        public StringQueryInput? usernameQuery { get; set; }
        public StringQueryInput? useremailQuery { get; set; }
        public MgtAppClientQueryInput? clientid { get; set; } // nested client object query
        public List<MgtAppUserQueryInput>? and { get; set; }
        public List<MgtAppUserQueryInput>? or { get; set; }
    }
}