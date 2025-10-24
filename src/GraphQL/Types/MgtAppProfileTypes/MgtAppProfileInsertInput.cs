using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;

namespace portfolio_graphql.GraphQL.Types.MgtAppProfileTypes
{
    public class MgtappProfileInsertInput
    {
        public LinkIdInput clientid { get; set; } = new LinkIdInput();
        public LinkIdInput? positionid { get; set; }

        public string? resume { get; set; }
        public string? profilevisastatus { get; set; }
        public string? profilerate { get; set; }
        public string? profilelastname { get; set; }
        public string? profilefirstname { get; set; }
        public string? profileemail { get; set; }
        public string? profiletype { get; set; }
        public string? profileexpirydate { get; set; }
        public string? profiledob { get; set; }
        public string? profilestatus { get; set; }
        public string? profilephone { get; set; }
        public string? profilevendor { get; set; }
        public string? profilecomments { get; set; }

        public List<string>? profilemanageravail { get; set; }
        public List<string>? profileavail { get; set; }
    }
}