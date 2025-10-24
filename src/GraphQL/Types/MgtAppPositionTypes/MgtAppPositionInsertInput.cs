using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppPositionTypes
{
    public class MgtappPositionInsertInput
    {
        public LinkIdInput clientid { get; set; } = new LinkIdInput();
        public LinkIdInput? groupid { get; set; }
        public string jobtitle { get; set; } = string.Empty;
        public string experience { get; set; } = string.Empty;
        public string skillset { get; set; } = string.Empty;
        public string billingrate { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
    }
}