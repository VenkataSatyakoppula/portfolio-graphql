using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppBankDetailsTypes
{
    public class MgtappBankDetailsQueryInput
    {
        public string? _id { get; set; }
        public string? bankstatus { get; set; }
        public StringQueryInput? bankstatusQuery { get; set; }

        public string? bankroutingno { get; set; }
        public StringQueryInput? bankroutingnoQuery { get; set; }

        public string? bankaccountno { get; set; }
        public StringQueryInput? bankaccountnoQuery { get; set; }

        public MgtappClientQueryInput? clientid { get; set; }
        public MgtappEmployeeQueryInput? employeeid { get; set; }

        public List<MgtappBankDetailsQueryInput>? and { get; set; }
        public List<MgtappBankDetailsQueryInput>? or { get; set; }
    }
}