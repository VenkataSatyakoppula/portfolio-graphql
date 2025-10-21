using portfolio_graphql.GraphQL.Queries;
using portfolio_graphql.Services;
using portfolio_graphql.GraphQL.Mutations;
using portfolio_graphql.GraphQL.Types.MgtAppClientTypes;
using portfolio_graphql.GraphQL.Types.MgtAppRoleTypes;
using portfolio_graphql.GraphQL.Types.MgtAppUserTypes;
using portfolio_graphql.GraphQL.Resolvers;
using portfolio_graphql.GraphQL.DataLoaders;

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
    .AddTypeExtension<MgtAppClientMutation>()
    .AddTypeExtension<MgtAppRoleMutation>()
    .AddTypeExtension<MgtAppUserMutation>()
    .AddType<MgtAppClientType>()
    .AddType<MgtAppRoleType>()
    .AddType<MgtAppUserType>()
    .AddDataLoader<ClientByIdDataLoader>()
    .AddDataLoader<RoleByIdDataLoader>();

var app = builder.Build();

app.MapGraphQL("/api/v1/graphql");

app.Run();
