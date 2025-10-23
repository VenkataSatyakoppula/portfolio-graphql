using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppInsuranceTypes
{
    public class MgtAppInsuranceQueryInput
    {
        public string? _id { get; set; }
        public string? insurancestatus { get; set; }
        public StringQueryInput? insurancestatusQuery { get; set; }

        public string? insurancesubstatus { get; set; }
        public StringQueryInput? insurancesubstatusQuery { get; set; }

        public MgtAppClientQueryInput? clientid { get; set; }
        public MgtAppEmployeeQueryInput? employeeid { get; set; }

        public List<MgtAppInsuranceQueryInput>? and { get; set; }
        public List<MgtAppInsuranceQueryInput>? or { get; set; }
    }
}