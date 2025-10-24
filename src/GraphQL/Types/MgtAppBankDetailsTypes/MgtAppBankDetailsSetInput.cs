using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppBankDetailsTypes
{
    public class MgtappBankdetailUpdateInput
    {
        public LinkIdInput? clientid { get; set; }
        public LinkIdInput? employeeid { get; set; }

        public string? bankstatus { get; set; }
        public string? bankroutingno { get; set; }
        public string? bankaccountno { get; set; }
    }
}