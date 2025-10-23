using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.Models;

namespace portfolio_graphql.GraphQL.Types.MgtAppTimesheetsTypes
{
    public class TimesheetInfoInput
    {
        public string? timesheetdate { get; set; }
        public string? timesheethours { get; set; }
    }

    public class MgtAppTimesheetsInsertInput
    {
        public LinkIdInput? clientid { get; set; }
        public LinkIdInput? employeeid { get; set; }
        public string? timesheetmonth { get; set; }
        public List<TimesheetInfoInput>? timesheetinfo { get; set; }
    }
}