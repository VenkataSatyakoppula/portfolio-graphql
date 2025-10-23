using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppUserTypes
{
    public class MgtappUserQueryInput
    {
        public string? _id { get; set; } // string to match other collections; will convert to ObjectId in filters
        // Added direct equality filters for username and useremail
        public string? username { get; set; }
        public string? useremail { get; set; }
        public StringQueryInput? usernameQuery { get; set; }
        public StringQueryInput? useremailQuery { get; set; }
        public MgtappClientQueryInput? clientid { get; set; } // nested client object query
        public List<MgtappUserQueryInput>? and { get; set; }
        public List<MgtappUserQueryInput>? or { get; set; }
    }
}