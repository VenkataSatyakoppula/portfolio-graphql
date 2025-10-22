using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppBankDetailsTypes
{
    public class MgtAppBankDetailsInsertInput
    {
        public LinkIdInput clientid { get; set; } = new LinkIdInput();
        public LinkIdInput employeeid { get; set; } = new LinkIdInput();

        public string? bankstatus { get; set; }
        public string? bankroutingno { get; set; }
        public string? bankaccountno { get; set; }
    }
}