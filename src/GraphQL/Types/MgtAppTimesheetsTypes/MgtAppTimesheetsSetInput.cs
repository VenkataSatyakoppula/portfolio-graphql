using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppTimesheetsTypes
{
    public class TimesheetInfoSetInput
    {
        public string? timesheetdate { get; set; }
        public string? timesheethours { get; set; }
    }

    public class MgtAppTimesheetsSetInput
    {
        public LinkIdInput? clientid { get; set; }
        public LinkIdInput? employeeid { get; set; }
        public string? timesheetmonth { get; set; }
        public List<TimesheetInfoSetInput>? timesheetinfo { get; set; }
    }
}