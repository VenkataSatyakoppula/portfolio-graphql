using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes
{
    public class MgtAppEmployeeInsertInput
    {
        public LinkIdInput clientid { get; set; } = new LinkIdInput();
        public LinkIdInput? employeeuserid { get; set; }
        public LinkIdInput? employeemanagerid { get; set; }

        public string? employeephone { get; set; }
        public string? employeefirstname { get; set; }
        public string? employeesalaryrate { get; set; }
        public string? employeeworkemail { get; set; }
        public string? employeeexpirydate { get; set; }
        public string? employeelastname { get; set; }
        public string? employeedob { get; set; }
        public string? employeevisastatus { get; set; }
        public string? employeeemail { get; set; }
        public string? employeevendor { get; set; }
        public string? employeetype { get; set; }
        public string? employeebillrate { get; set; }
        public string? employeesubstatus { get; set; }
        public string? employeestatus { get; set; }
    }
}