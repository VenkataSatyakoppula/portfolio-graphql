using portfolio_graphql.GraphQL.Queries;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Mutations;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppRoleTypes;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;
using portfolio_graphql.GraphQL.Resolvers;
using portfolio_graphql.GraphQL.DataLoaders;
using portfolio_graphql.GraphQL.Types.MgtAppPositionTypes;
using portfolio_graphql.GraphQL.Types.MgtAppGroupTypes;
using portfolio_graphql.GraphQL.Types.MgtAppProfileTypes;
using portfolio_graphql.GraphQL.Types.MgtAppEmployeeTypes;
using portfolio_graphql.GraphQL.Types.MgtAppBankDetailsTypes;

var builder = WebApplication.CreateBuilder(args);

// Add MongoDB
builder.Services.AddSingleton<MongoDbContext>();

// Add GraphQL & DataLoaders
builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
    .AddMutationType(d => d.Name("Mutation"))
    .AddTypeExtension<MgtAppClientQuery>()
    .AddTypeExtension<MgtAppRoleQuery>()
    .AddTypeExtension<MgtAppUserQuery>()
    .AddTypeExtension<MgtAppPositionQuery>()
    .AddTypeExtension<MgtAppGroupQuery>()
    .AddTypeExtension<MgtAppProfileQuery>()
    .AddTypeExtension<MgtAppEmployeeQuery>()
    .AddTypeExtension<MgtAppBankDetailsQuery>()
    .AddTypeExtension<MgtAppClientMutation>()
    .AddTypeExtension<MgtAppRoleMutation>()
    .AddTypeExtension<MgtAppUserMutation>()
    .AddTypeExtension<MgtAppGroupMutation>()
    .AddTypeExtension<MgtAppPositionMutation>()
    .AddTypeExtension<MgtAppProfileMutation>()
    .AddTypeExtension<MgtAppEmployeeMutation>()
    .AddTypeExtension<MgtAppBankDetailsMutation>()
    .AddType<MgtAppClientType>()
    .AddType<MgtAppRoleType>()
    .AddType<MgtAppUserType>()
    .AddType<MgtAppPositionType>()
    .AddType<MgtAppGroupType>()
    .AddType<MgtAppProfileType>()
    .AddType<MgtAppEmployeeType>()
    .AddType<MgtAppBankDetailsType>()
    .AddDataLoader<ClientByIdDataLoader>()
    .AddDataLoader<RoleByIdDataLoader>()
    .AddDataLoader<GroupByIdDataLoader>()
    .AddDataLoader<PositionByIdDataLoader>()
    .AddDataLoader<UserByIdDataLoader>()
    .AddDataLoader<EmployeeByIdDataLoader>();

var app = builder.Build();

app.MapGraphQL("/api/v1/graphql");

app.Run();
