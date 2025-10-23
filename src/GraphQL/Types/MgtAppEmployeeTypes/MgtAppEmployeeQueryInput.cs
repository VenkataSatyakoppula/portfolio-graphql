using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;

namespace portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes
{
    public class MgtappEmployeeQueryInput
    {
        public string? _id { get; set; }
        public MgtappClientQueryInput? clientid { get; set; }
        public MgtappUserQueryInput? employeeuserid { get; set; }
        public MgtappUserQueryInput? employeemanagerid { get; set; }

        public string? employeephone { get; set; }
        public StringQueryInput? employeephoneQuery { get; set; }

        public string? employeefirstname { get; set; }
        public StringQueryInput? employeefirstnameQuery { get; set; }

        public string? employeesalaryrate { get; set; }
        public StringQueryInput? employeesalaryrateQuery { get; set; }

        public string? employeeworkemail { get; set; }
        public StringQueryInput? employeeworkemailQuery { get; set; }

        public string? employeeexpirydate { get; set; }
        public StringQueryInput? employeeexpirydateQuery { get; set; }

        public string? employeelastname { get; set; }
        public StringQueryInput? employeelastnameQuery { get; set; }

        public string? employeedob { get; set; }
        public StringQueryInput? employeedobQuery { get; set; }

        public string? employeevisastatus { get; set; }
        public StringQueryInput? employeevisastatusQuery { get; set; }

        public string? employeeemail { get; set; }
        public StringQueryInput? employeeemailQuery { get; set; }

        public string? employeevendor { get; set; }
        public StringQueryInput? employeevendorQuery { get; set; }

        public string? employeetype { get; set; }
        public StringQueryInput? employeetypeQuery { get; set; }

        public string? employeebillrate { get; set; }
        public StringQueryInput? employeebillrateQuery { get; set; }

        public string? employeesubstatus { get; set; }
        public StringQueryInput? employeesubstatusQuery { get; set; }

        public string? employeestatus { get; set; }
        public StringQueryInput? employeestatusQuery { get; set; }

        public List<MgtappEmployeeQueryInput>? and { get; set; }
        public List<MgtappEmployeeQueryInput>? or { get; set; }
    }
}