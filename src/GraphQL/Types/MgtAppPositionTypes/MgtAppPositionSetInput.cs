using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppPositionTypes
{
    public class MgtAppPositionSetInput
    {
        public LinkIdInput? clientid { get; set; }
        public LinkIdInput? groupid { get; set; }
        public string? jobtitle { get; set; }
        public string? experience { get; set; }
        public string? skillset { get; set; }
        public string? billingrate { get; set; }
        public string? status { get; set; }
    }
}