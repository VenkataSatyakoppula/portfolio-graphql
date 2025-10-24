using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppPositionTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppProfileTypes
{
    public class MgtappProfileQueryInput
    {
        public string? _id { get; set; }
        // Add support for filtering by multiple IDs
        public List<string>? _id_in { get; set; }
        // Remove primitive clientid and use nested client object as clientid
        public MgtappClientQueryInput? clientid { get; set; }
        public MgtappPositionQueryInput? positionid { get; set; }

        public string? resume { get; set; }
        public StringQueryInput? resumeQuery { get; set; }

        public string? profilevisastatus { get; set; }
        public StringQueryInput? profilevisastatusQuery { get; set; }

        public string? profilerate { get; set; }
        public StringQueryInput? profilerateQuery { get; set; }

        public string? profilelastname { get; set; }
        public StringQueryInput? profilelastnameQuery { get; set; }

        public string? profilefirstname { get; set; }
        public StringQueryInput? profilefirstnameQuery { get; set; }

        public string? profileemail { get; set; }
        public StringQueryInput? profileemailQuery { get; set; }

        public string? profiletype { get; set; }
        public StringQueryInput? profiletypeQuery { get; set; }

        public string? profileexpirydate { get; set; }
        public StringQueryInput? profileexpirydateQuery { get; set; }

        public string? profiledob { get; set; }
        public StringQueryInput? profiledobQuery { get; set; }

        public string? profilestatus { get; set; }
        public StringQueryInput? profilestatusQuery { get; set; }

        public string? profilephone { get; set; }
        public StringQueryInput? profilephoneQuery { get; set; }

        public string? profilevendor { get; set; }
        public StringQueryInput? profilevendorQuery { get; set; }

        public string? profilecomments { get; set; }
        public StringQueryInput? profilecommentsQuery { get; set; }

        public List<MgtappProfileQueryInput>? and { get; set; }
        public List<MgtappProfileQueryInput>? or { get; set; }
    }
}