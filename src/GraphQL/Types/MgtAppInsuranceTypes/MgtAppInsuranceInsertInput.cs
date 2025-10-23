using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppInsuranceTypes
{
    public class MgtAppInsuranceInsertInput
    {
        public LinkIdInput clientid { get; set; } = new LinkIdInput();
        public LinkIdInput employeeid { get; set; } = new LinkIdInput();

        public string? insurancestatus { get; set; }
        public string? insurancesubstatus { get; set; }
    }
}