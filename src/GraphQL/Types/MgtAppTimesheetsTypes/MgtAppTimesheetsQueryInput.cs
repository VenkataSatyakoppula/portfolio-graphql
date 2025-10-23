using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppTimesheetsTypes
{
    public class TimesheetInfoQueryInput
    {
        public string? timesheethours { get; set; }
        public StringQueryInput? timesheethoursQuery { get; set; }

        public string? timesheetdate { get; set; }
        public StringQueryInput? timesheetdateQuery { get; set; }
    }

    public class MgtappTimesheetsQueryInput
    {
        public string? _id { get; set; }

        public string? timesheetmonth { get; set; }
        public StringQueryInput? timesheetmonthQuery { get; set; }

        public MgtappClientQueryInput? clientid { get; set; }
        public MgtappEmployeeQueryInput? employeeid { get; set; }

        public TimesheetInfoQueryInput? timesheetinfo { get; set; }

        public List<MgtappTimesheetsQueryInput>? and { get; set; }
        public List<MgtappTimesheetsQueryInput>? or { get; set; }
    }
}