using System.Collections.Generic;
using portfolio_graphql.GraphQL.Types;
namespace portfolio_graphql.GraphQL.Types.MgtAppRoleTypes
{
    public class MgtAppRoleQueryInput
    {
        public string? _id { get; set; }
        public string? rolename { get; set; }
        public StringQueryInput? rolenameQuery { get; set; }
        public List<MgtAppRoleQueryInput>? and { get; set; }
        public List<MgtAppRoleQueryInput>? or { get; set; }
    }
}