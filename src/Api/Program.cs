using Application.SagaApprovmentEndPoints;
using Application.SagaRejectionEndPoints;
using Infrastructure;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Oakton.Resources;
using Persons;
using Persons.Contracts;
using Persons.Read.GetPersonWithSum;
using Persons.Write.CreatePersonUseCase;
using Persons.Write.Subscriptions;
using Shared;
using Shared.Infrastructure;
using Weasel.Core;
using Wolverine;
using Wolverine.Http;
using Wolverine.Kafka;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
services.AddScoped<IRepository, Repository>();
services.AddPersonServices();

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new ArgumentNullException("Please specify a valid connection string");
var martenConfig = configuration.GetSection("EventStore").Get<MartenSettings>()
        ?? throw new ArgumentNullException("Please specify a valid connection string"); ;

services.AddMarten(options =>
{
    options.Connection(connectionString);
    options.AutoCreateSchemaObjects = AutoCreate.All;
    options.UseNewtonsoftForSerialization(nonPublicMembersStorage: NonPublicMembersStorage.All);
    options.Events.DatabaseSchemaName = martenConfig.WriteSchema;
    options.Events.StreamIdentity = Marten.Events.StreamIdentity.AsString;

    if (!string.IsNullOrEmpty(martenConfig.ReadSchema))
        options.DatabaseSchemaName = martenConfig.ReadSchema;
    options.Projections.Add<PersonWithSumProjection>(ProjectionLifecycle.Async);
})
.IntegrateWithWolverine()
.UseLightweightSessions()
.AddSubscriptionWithServices<PersonApprovedToKafkaSubscription>(ServiceLifetime.Singleton, o =>
{
    o.Options.BatchSize = 10;
})
.AddAsyncDaemon(DaemonMode.HotCold);

builder.Services.AddControllers();

builder.Host.UseWolverine(opts =>
{
    opts.Policies.AutoApplyTransactions();
    opts.UseKafka("kafka:9092");
    opts.PublishMessage<PersonApproved>().ToKafkaTopic("CreatePersonUseCase.PersonApproved");
    opts.PublishMessage<PersonRejected>().ToKafkaTopic("CreatePersonUseCase.PersonRejected");
    opts.PublishMessage<PersonApprovedIntegrationEvent>().ToKafkaTopic("PersonApprovedIntegrationEvent");
    opts.ListenToKafkaTopic("CreatePersonUseCase.PersonApproved");
    opts.ListenToKafkaTopic("CreatePersonUseCase.PersonRejected");
    opts.Services.AddResourceSetupOnStartup();
    opts.Discovery.IncludeAssembly(typeof(CreatePersonEndPoint).Assembly);
    opts.Discovery.IncludeAssembly(typeof(RejectEndPoints).Assembly);
    opts.Discovery.IncludeAssembly(typeof(ApproveEndPoints).Assembly);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapWolverineEndpoints();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
