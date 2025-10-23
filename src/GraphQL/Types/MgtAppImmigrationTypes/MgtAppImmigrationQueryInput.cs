using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppImmigrationTypes
{
    public class MgtappImmigrationQueryInput
    {
        public string? _id { get; set; }

        public string? immigrationstatus { get; set; }
        public StringQueryInput? immigrationstatusQuery { get; set; }

        public string? immigrationsubstatus { get; set; }
        public StringQueryInput? immigrationsubstatusQuery { get; set; }

        public MgtappClientQueryInput? clientid { get; set; }
        public MgtappEmployeeQueryInput? employeeid { get; set; }

        public List<MgtappImmigrationQueryInput>? and { get; set; }
        public List<MgtappImmigrationQueryInput>? or { get; set; }
    }
}