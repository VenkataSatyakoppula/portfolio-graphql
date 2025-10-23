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
using portfolio_graphql.GraphQL.Types.MgtAppImmigrationTypes;
using portfolio_graphql.GraphQL.Types.MgtAppInsuranceTypes;
using portfolio_graphql.GraphQL.Types.MgtAppTimesheetsTypes;
using portfolio_graphql.GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

// Add MongoDB
builder.Services.AddSingleton<MongoDbContext>();

// Add GraphQL & DataLoaders
builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
    .AddMutationType(d => d.Name("Mutation"))
    .InitializeOnStartup()
    .AddTypeExtension<MgtAppClientQuery>()
    .AddTypeExtension<MgtAppRoleQuery>()
    .AddTypeExtension<MgtAppUserQuery>()
    .AddTypeExtension<MgtAppPositionQuery>()
    .AddTypeExtension<MgtAppGroupQuery>()
    .AddTypeExtension<MgtAppProfileQuery>()
    .AddTypeExtension<MgtAppEmployeeQuery>()
    .AddTypeExtension<MgtAppBankDetailsQuery>()
    .AddTypeExtension<MgtAppImmigrationQuery>()
    .AddTypeExtension<MgtAppInsuranceQuery>()
    .AddTypeExtension<MgtAppTimesheetsQuery>()
    .AddTypeExtension<MgtAppClientMutation>()
    .AddTypeExtension<MgtAppRoleMutation>()
    .AddTypeExtension<MgtAppUserMutation>()
    .AddTypeExtension<MgtAppGroupMutation>()
    .AddTypeExtension<MgtAppPositionMutation>()
    .AddTypeExtension<MgtAppProfileMutation>()
    .AddTypeExtension<MgtAppEmployeeMutation>()
    .AddTypeExtension<MgtAppBankDetailsMutation>()
    .AddTypeExtension<MgtAppImmigrationMutation>()
    .AddTypeExtension<MgtAppInsuranceMutation>()
    .AddTypeExtension<MgtAppTimesheetsMutation>()
    .AddTypeExtension<MgtAppTicketMutation>()
    .AddType<MgtAppClientType>()
    .AddType<MgtAppRoleType>()
    .AddType<MgtAppUserType>()
    .AddType<MgtAppPositionType>()
    .AddType<MgtAppGroupType>()
    .AddType<MgtAppProfileType>()
    .AddType<MgtAppEmployeeType>()
    .AddType<MgtAppBankDetailsType>()
    .AddType<MgtAppImmigrationType>()
    .AddType<MgtAppInsuranceType>()
    .AddType<MgtAppTimesheetsType>()
    .AddType<MgtAppTicketType>()
    .AddTypeExtension<MgtAppTicketQuery>()
    .AddDataLoader<ClientByIdDataLoader>()
    .AddDataLoader<RoleByIdDataLoader>()
    .AddDataLoader<GroupByIdDataLoader>()
    .AddDataLoader<PositionByIdDataLoader>()
    .AddDataLoader<UserByIdDataLoader>()
    .AddDataLoader<EmployeeByIdDataLoader>()
    .AddDataLoader<ProfileByIdDataLoader>();

var app = builder.Build();

// Warm up Mongo connection and ensure indexes to avoid first-request latency
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    // Ping via a lightweight count and ensure common indexes
    ctx.Tickets.EstimatedDocumentCount();
    ctx.EnsureTicketIndexes();
}

app.MapGraphQL("/api/v1/graphql");

app.Run();
