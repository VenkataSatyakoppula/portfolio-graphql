using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppImmigrationTypes
{
    public class MgtAppImmigrationQueryInput
    {
        public string? _id { get; set; }

        public string? immigrationstatus { get; set; }
        public StringQueryInput? immigrationstatusQuery { get; set; }

        public string? immigrationsubstatus { get; set; }
        public StringQueryInput? immigrationsubstatusQuery { get; set; }

        public MgtAppClientQueryInput? clientid { get; set; }
        public MgtAppEmployeeQueryInput? employeeid { get; set; }

        public List<MgtAppImmigrationQueryInput>? and { get; set; }
        public List<MgtAppImmigrationQueryInput>? or { get; set; }
    }
}