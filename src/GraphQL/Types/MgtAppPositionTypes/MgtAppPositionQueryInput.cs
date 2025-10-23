using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppPositionTypes
{
    public class MgtappPositionQueryInput
    {
        public string? _id { get; set; }
        public MgtappClientQueryInput? clientid { get; set; }
        public string? groupid { get; set; }
        public string? jobtitle { get; set; }
        public StringQueryInput? jobtitleQuery { get; set; }
        public string? experience { get; set; }
        public StringQueryInput? experienceQuery { get; set; }
        public string? skillset { get; set; }
        public StringQueryInput? skillsetQuery { get; set; }
        public string? billingrate { get; set; }
        public StringQueryInput? billingrateQuery { get; set; }
        public string? status { get; set; }
        public StringQueryInput? statusQuery { get; set; }
        public List<MgtappPositionQueryInput>? and { get; set; }
        public List<MgtappPositionQueryInput>? or { get; set; }
    }
}