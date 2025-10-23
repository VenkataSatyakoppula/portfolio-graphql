using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppInsuranceTypes
{
    public class MgtAppInsuranceSetInput
    {
        public LinkIdInput? clientid { get; set; }
        public LinkIdInput? employeeid { get; set; }

        public string? insurancestatus { get; set; }
        public string? insurancesubstatus { get; set; }
    }
}